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
                var parameters = new Dictionary<string, object>();

                var template = new DocxReport(this.config["TemplatesPath"] + TemplateName);

                var lcs = new LoadingCustomizationStruct(null)
                {
                    LoadingTypes = new[] { typeof(Car) },
                    View = Car.Views.CarL,
                };

                var cars = this.dataService.LoadObjects(lcs).Cast<Car>().ToList();

                var allCarsParameters = new List<Dictionary<string, object>>();

                foreach (var car in cars)
                {
                    var singleCarParameters = new Dictionary<string, object>();

                    singleCarParameters.Add("CarNumber", car.CarNumber);
                    singleCarParameters.Add("CarDate", car.CarDate.ToString("dd.MM.yyyy"));
                    singleCarParameters.Add("CarBody", EnumCaption.GetCaptionFor(car.CarBody));

                    allCarsParameters.Add(singleCarParameters);
                }

                parameters.Add("Car", allCarsParameters);

                template.BuildWithParameters(parameters);

                var stream = new MemoryStream();
                template.SaveAs(stream);
                stream.Position = 0;
                return this.File(stream, "application/vnd.openxmlformats-officedocument.wordprocessingml.document.main+xml");
            }
            catch (Exception ex)
            {
                LogService.LogError(ex);
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
                    return this.StatusCode(400);
                }

                using (FileStream output = new (this.config["TemplatesPath"] + TemplateName, FileMode.Open))
                {
                    file.CopyTo(output);
                }

                return this.StatusCode(200);
            }
            catch (Exception ex)
            {
                LogService.LogError(ex);
                return this.StatusCode(500);
            }
        }

        [HttpGet("[action]")]
        public IActionResult DownloadTemplate()
        {
            try
            {
                var fullPath = this.config["TemplatesPath"] + TemplateName;
                var stream = new FileStream(fullPath, FileMode.Open);
                return this.File(stream, "application/vnd.openxmlformats-officedocument.wordprocessingml.document.main+xml");
            }
            catch (Exception ex)
            {
                LogService.LogError(ex);
                return this.StatusCode(500);
            }
        }
    }
}
