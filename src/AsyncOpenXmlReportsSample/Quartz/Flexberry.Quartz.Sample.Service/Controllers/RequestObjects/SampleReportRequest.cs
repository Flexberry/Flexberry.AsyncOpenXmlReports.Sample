namespace Flexberry.Quartz.Sample.Service.Controllers.RequestObjects
{
    using Newtonsoft.Json;

    /// <summary>
    /// Данные для тестового запроса.
    /// </summary>
    public class SampleReportRequest
    {
        /// <summary>
        /// Идентификатор апроса.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Информация о пользователе.
        /// </summary>
        public UserInfo UserInfo { get; set; }

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