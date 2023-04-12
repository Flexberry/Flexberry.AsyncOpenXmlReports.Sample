using Newtonsoft.Json;

namespace Flexberry.Quartz.Sample.Service.RequestsObjects
{
    /// <summary>
    /// Данные для тестового запроса.
    /// </summary>
    public class TestReportRequest
    {
        /// <summary>
        /// Идентификатор апроса.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Идентификатор пользователя.
        /// </summary>
        public string UserLogin { get; set; }

        /// <summary>
        /// Домен пользователя.
        /// </summary>
        public string UserDomain { get; set; }

        /// <summary>
        /// Имя пользователя.
        /// </summary>
        public string UserFriendlyName { get; set; }

        /// <summary>
        /// Роли пользователя, разделенные запятоыми.
        /// </summary>
        public string UserRoles { get; set; }

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