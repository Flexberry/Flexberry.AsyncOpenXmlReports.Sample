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

    [ApiController]
    [Route("api/[controller]")]
    public class CarListReportController : ControllerBase
    {
        /// <summary>
        /// App configuration.
        /// </summary>
        private readonly IConfiguration config;

        private readonly IUserWithRolesAndEmail userService;

        private readonly IDataService dataService;

        public const string TemplateName = "CarListTemplate.docx";

        /// <summary>
        /// Initializes a new instance of the <see cref="CarListReportController"/> class.
        /// </summary>
        /// <param name="configuration">App configuration.</param>
        public CarListReportController(IConfiguration configuration,
            IUserWithRolesAndEmail userService,
            IDataService dataService)
        {
            this.config = configuration;
            this.userService = userService;
            this.dataService = dataService;
        }

        [HttpGet("[action]")]
        public async Task<string> Build()
        {
            string userName = userService.Login;
            string userRoles = userService.Roles;
            string userEmail = userService.Email;

            Guid reportId = Guid.NewGuid();

            UserReport report = new UserReport()
            {
                UserName = userName,
                UserEmail = userEmail,
                ReportId = reportId,
                ReportTaskStartTime = DateTime.Now,
                Status = ReportStatusType.InProgress,
            };

            dataService.UpdateObject(report);

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
                            dataService.UpdateObject(report);

                            return "Что-то пошло не так";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogService.Log.Error(ex);

                report.Status = ReportStatusType.Unexecuted;
                dataService.UpdateObject(report);

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
                return this.StatusCode(500);
            }
        }

        [HttpGet("[action]")]
        public IActionResult DownloadTemplate()
        {
            try
            {
                string fullPath = this.config["TemplatesPath"] + TemplateName;

                MemoryStream memoryStream = new MemoryStream();

                using (FileStream fileStream = new FileStream(fullPath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    fileStream.CopyTo(memoryStream);
                }

                memoryStream.Position = 0;
                return this.File(memoryStream, "application/vnd.openxmlformats-officedocument.wordprocessingml.document.main+xml");
            }
            catch (Exception ex)
            {
                return this.StatusCode(500);
            }
        }
    }
}
