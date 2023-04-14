namespace IIS.AsyncOpenXmlReportsSample
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;
    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.Business;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using NewPlatform.Flexberry.Reports;
    using Newtonsoft.Json;

    [ApiController]
    [Route("api/[controller]")]
    public class CarListReportController : ControllerBase
    {
        /// <summary>
        /// App configuration.
        /// </summary>
        private readonly IConfiguration config;

        private readonly IWebHostEnvironment webHostEnvironment;

        private readonly IHttpContextAccessor contextAccessor;

        public const string TemplateName = "CarListTemplate.docx";

        /// <summary>
        /// Initializes a new instance of the <see cref="CarListReportController"/> class.
        /// </summary>
        /// <param name="configuration">App configuration.</param>
        public CarListReportController(IConfiguration configuration, IHttpContextAccessor contextAccessor, IWebHostEnvironment webHostEnvironment)
        {
            this.config = configuration;
            this.contextAccessor = contextAccessor;
            this.webHostEnvironment = webHostEnvironment;
        }

        [HttpGet("[action]")]
        public async Task<string> Build()
        {
            try
            {
                var userService = new CurrentHttpUserService(contextAccessor);

                var templatePath = this.webHostEnvironment.ContentRootPath;

                var responseString = string.Empty;
                var apiUrl = this.config["QuartzUrl"] + "CarListReport";

                using (var httpClient = new HttpClient())
                {
                    object input = new
                    {
                        Id = Guid.NewGuid().ToString(),
                        TemplatePath = templatePath + @"\" + this.config["TemplatesPath"],
                        TemplateName = TemplateName,
                        UserInfo = new
                        {
                            Login = userService.Login,
                            Roles = "AllAccess", // TODO: Достать роли текущего пользователя
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
                            // TODO: обработать варианты ответов (скачивать/отправлять на e-mail/высылать уведомление)
                            responseString = "Отчет формируется";
                        }
                        else
                        {
                            responseString = "Что-то пошло не так";
                        }
                    }
                }

                return responseString;
            }
            catch (Exception ex)
            {
                // TODO: обработать корректно исключение
                return ex.Message;
            }
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
