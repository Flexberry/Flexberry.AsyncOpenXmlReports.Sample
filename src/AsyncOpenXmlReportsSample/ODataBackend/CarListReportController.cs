namespace IIS.AsyncOpenXmlReportsSample
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.Business;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using NewPlatform.Flexberry.Reports;

    [ApiController]
    [Route("api/[controller]")]
    public class CarListReportController : ControllerBase
    {
        /// <summary>
        /// App configuration.
        /// </summary>
        protected IConfiguration config;

        /// <summary>
        /// Data service.
        /// </summary>
        private readonly IDataService dataService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CarListReportController"/> class.
        /// </summary>
        /// <param name="configuration">App configuration.</param>
        /// <param name="dataService">Data service.</param>
        public CarListReportController(IConfiguration configuration, IDataService dataService)
        {
            this.dataService = dataService;
            this.config = configuration;
        }

        public const string TemplateName = "CarListTemplate.docx";

        [HttpGet("[action]")]
        public IActionResult Build()
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>();

                DocxReport template = new DocxReport(this.config["TemplatesPath"] + TemplateName);

                var lcs = new LoadingCustomizationStruct(null)
                {
                    LoadingTypes = new[] { typeof(Car) },
                    View = Car.Views.CarL,
                };

                List<Car> cars = this.dataService.LoadObjects(lcs).Cast<Car>().ToList();

                List<Dictionary<string, object>> allCarsParameters = new List<Dictionary<string, object>>();

                foreach (Car car in cars)
                {
                    Dictionary<string, object> singleCarParameters = new Dictionary<string, object>();

                    singleCarParameters.Add("CarNumber", car.CarNumber);
                    singleCarParameters.Add("CarDate", car.CarDate.ToString("dd.MM.yyyy"));
                    singleCarParameters.Add("CarBody", EnumCaption.GetCaptionFor(car.CarBody));

                    allCarsParameters.Add(singleCarParameters);
                }

                parameters.Add("Car", allCarsParameters);

                template.BuildWithParameters(parameters);

                MemoryStream stream = new MemoryStream();
                template.SaveAs(stream);
                stream.Position = 0;
                return File(stream, "application/vnd.openxmlformats-officedocument.wordprocessingml.document.main+xml");
            }
            catch (Exception ex)
            {
                return this.StatusCode(500);
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
