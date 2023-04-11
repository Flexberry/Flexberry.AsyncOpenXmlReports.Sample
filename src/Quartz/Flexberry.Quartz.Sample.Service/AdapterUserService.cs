using System;
using System.Collections.Generic;
using System.Text;

namespace Flexberry.Quartz.Sample.Service
{
    public class AdapterUserService : IUserWithRoles
    {
        public string Login { get; set; }
        public string Domain { get; set; }
        public string FriendlyName { get; set; }
        public string Roles { get; set; }
    }
}
