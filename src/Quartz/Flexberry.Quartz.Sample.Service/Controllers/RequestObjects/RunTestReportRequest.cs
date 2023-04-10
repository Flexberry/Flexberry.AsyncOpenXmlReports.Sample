using Newtonsoft.Json;

namespace Flexberry.Quartz.Sample.Service.RequestsObjects
{
    public class TestReportRequest
    {
        public string Id { get; set; }
        public string UserLogin { get; set; }
        public string UserDomain { get; set; }
        public string UserFriendlyName { get; set; }
        public string UserRoles { get; set; }

        public override string ToString()
        {
            var settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            string msg = JsonConvert.SerializeObject(this, settings);

            return msg;
        }
    }
}