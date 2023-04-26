namespace IIS.AsyncOpenXmlReportsSample.Controllers.RequestObjects
{
    using Newtonsoft.Json;

    /// <summary>
    /// Результат обработки отчета.
    /// </summary>
    public class ReportResultRequest
    {
        /// <summary>
        /// Идентификатор апроса.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Имя файла.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Статус обработки.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            var settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            string msg = JsonConvert.SerializeObject(this, settings);

            return msg;
        }
    }
}
