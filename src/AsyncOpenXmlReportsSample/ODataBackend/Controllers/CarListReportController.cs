namespace IIS.AsyncOpenXmlReportsSample
{
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.Business;
    using IIS.AsyncOpenXmlReportsSample.Controllers.RequestObjects;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Newtonsoft.Json;

    /// <summary>
    /// Контроллер построения демонстрационных отчетов для списковой формы Cars.
    /// Также предоставляет функции загрузки(изменения) и выгрузки шаблона для этого отчета.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class CarListReportController : ControllerBase
    {
        public const string TemplateName = "CarListTemplate.docx";

        /// <summary>
        /// App configuration.
        /// </summary>
        private readonly IConfiguration config;

        private readonly IUserWithRolesAndEmail userService;

        private readonly IDataService dataService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CarListReportController"/> class.
        /// </summary>
        /// <param name="configuration">App configuration.</param>
        /// <param name="userService">Экземпляр текущего UserService.</param>
        /// <param name="dataService">Экземпляр текущего DataService.</param>
        public CarListReportController(
            IConfiguration configuration,
            IUserWithRolesAndEmail userService,
            IDataService dataService)
        {
            this.config = configuration;
            this.userService = userService;
            this.dataService = dataService;

            this.CheckAndCreateTemplateFile();
        }

        /// <summary>
        /// Запуск построения отчета.
        /// </summary>
        [HttpGet("[action]")]
        public async Task<string> Build()
        {
            string userName = this.userService.Login;
            string userRoles = this.userService.Roles;
            string userEmail = this.userService.Email;

            Guid reportId = Guid.NewGuid();

            UserReport report = new UserReport()
            {
                UserName = userName,
                UserEmail = userEmail,
                ReportId = reportId,
                ReportTaskStartTime = DateTime.Now,
                Status = ReportStatusType.InProgress,
            };

            this.dataService.UpdateObject(report);

            try
            {
                string apiUrl = this.config["QuartzUrl"] + "CarListReport";

                using (var httpClient = new HttpClient())
                {
                    object input = new
                    {
                        Id = reportId.ToString(),
                        TemplateName = TemplateName,
                        UserInfo = new
                        {
                            Login = userName,
                            Roles = userRoles,
                        },
                    };

                    var msg = JsonConvert.SerializeObject(input);
                    var buffer = Encoding.UTF8.GetBytes(msg);
                    var byteContent = new ByteArrayContent(buffer);
                    byteContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

                    using (var response = await httpClient.PostAsync(apiUrl, byteContent))
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            return "Отчет формируется";
                        }
                        else
                        {
                            report.Status = ReportStatusType.Unexecuted;
                            this.dataService.UpdateObject(report);

                            return "Что-то пошло не так";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogService.Log.Error(ex);

                report.Status = ReportStatusType.Unexecuted;
                this.dataService.UpdateObject(report);

                return ex.Message;
            }
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public IActionResult BuildResult([FromBody] ReportResultRequest request)
        {
            LogService.Log.Info($"CarListReportController.BuildResult request = {request}");

            return Ok();
        }

        /// <summary>
        /// Згрузить файл-шаблона. Содержимое базового шаблона меняется на содержимое загружаемого файла.
        /// Таким образом изменяется стандартный шаблон.
        /// </summary>
        /// <param name="file">Файл шаблона.</param>
        /// <returns>Результат выполнения операции.</returns>
        [HttpPost]
        public IActionResult UploadTemplate(IFormFile file)
        {
            try
            {
                if (file is null)
                {
                    return Content("File not found");
                }

                using (FileStream fileStream = new FileStream(this.config["TemplatesPath"] + TemplateName, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    file.CopyTo(fileStream);
                }

                return this.StatusCode(200);
            }
            catch (Exception ex)
            {
                LogService.Log.Error(ex);

                return this.StatusCode(500);
            }
        }

        /// <summary>
        /// Скачать текущий файл-шаблона.
        /// </summary>
        /// <returns>Файл шаблона.</returns>
        [HttpGet("[action]")]
        public IActionResult DownloadTemplate()
        {
            try
            {
                string templateDirectory = this.config["TemplatesPath"];
                string templateFileFullPath = Path.Combine(templateDirectory, TemplateName);
                MemoryStream memoryStream = LoadFile(templateFileFullPath);

                return this.File(memoryStream, "application/vnd.openxmlformats-officedocument.wordprocessingml.document.main+xml");
            }
            catch (Exception ex)
            {
                LogService.LogError(ex);

                return this.StatusCode(500);
            }
        }

        /// <summary>
        /// Проверить, есть ли файл шаблона в папке, указанной в конфиге, если нет то создать. Это нужно например для докера, т.к
        /// файл шаблона общий для нескольких сервисов, и должен храниться по определенному пути.
        /// </summary>
        private void CheckAndCreateTemplateFile()
        {
            string templateDirectory = this.config["TemplatesPath"];
            string templateFileFullPath = Path.Combine(templateDirectory, TemplateName);

            if (!System.IO.File.Exists(templateFileFullPath))
            {
                string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string defaultProjectTemplateFile = Path.Combine(baseDirectory, "Templates", TemplateName);

                MemoryStream memoryStream = LoadFile(defaultProjectTemplateFile);

                Directory.CreateDirectory(templateDirectory);

                using (FileStream fileStream = new FileStream(templateFileFullPath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    memoryStream.WriteTo(fileStream);
                }

                memoryStream.Close();
            }
        }

        /// <summary>
        /// Загрузить файл по указанному пути.
        /// </summary>
        /// <param name="path">Путь к файлу.</param>
        /// <returns>Содержимое файла в виде memorySrtream.</returns>
        private MemoryStream LoadFile(string path)
        {
            MemoryStream memoryStream = new MemoryStream();

            using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                fileStream.CopyTo(memoryStream);
            }

            memoryStream.Position = 0;

            return memoryStream;
        }
    }
}
