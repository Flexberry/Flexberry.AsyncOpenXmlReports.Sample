namespace IIS.AsyncOpenXmlReportsSample
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.Business;
    using Microsoft.AspNetCore.Mvc;
    using NewPlatform.Flexberry.Reports;

    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly IDataService dataService;

        public ReportsController(IDataService dataService)
        {
            this.dataService = dataService;
        }

        [HttpPost]
        public IActionResult Build()
        {
            var parameters = new Dictionary<string, object>();

            var template = new DocxReport(Path.Combine("Templates", "Cars.docx"));

            var lcs = new LoadingCustomizationStruct(null)
            {
                LoadingTypes = new[] { typeof(Car) },
                View = Car.Views.CarL,
            };

            var cars = dataService.LoadObjects(lcs).Cast<Car>().ToList();

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
            return File(stream, "application/vnd.openxmlformats-officedocument.wordprocessingml.document.main+xml");
        }
    }
}
