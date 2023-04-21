
namespace IIS.AsyncOpenXmlReportsSample
{
    using System;
    using System.Linq;
    using System.Security.Claims;
    using Microsoft.AspNetCore.Http;

    /// <summary>
    /// Реализация сервиса текущего пользователя.
    /// </summary>
    public class CurrentHttpUserService : IUserWithRolesAndEmail
    {
        private IHttpContextAccessor contextAccessor;

        public CurrentHttpUserService(IHttpContextAccessor contextAccessor)
        {
            this.contextAccessor = contextAccessor;
        }

        /// <summary>
        /// Логин пользователя.
        /// </summary>
        public string Login
        {
            get { return GetLogin(); }
            set { }
        }

        /// <summary>
        /// Домен пользователя.
        /// </summary>
        public string Domain
        {
            get { return null; }
            set { }
        }

        /// <summary>
        /// Имя пользователя.
        /// </summary>
        public string FriendlyName
        {
            get { return Login; }
            set { }
        }

        public string Roles
        {
            get { return GetRoles(); }
            set { }
        }

        public string Email
        {
            get { return GetEmail(); }
            set { }
        }

        private string GetLogin()
        {
            var currentClaims = (contextAccessor.HttpContext.User?.Identity as ClaimsIdentity)?.Claims;
            string agentClaim = currentClaims?.First(p => p.Type == "preferred_username").Value;

            return agentClaim;
        }

        private string GetRoles()
        {
            var currentClaims = (contextAccessor.HttpContext.User?.Identity as ClaimsIdentity)?.Claims;
            string roles = currentClaims?.First(p => p.Type == "resource_access").Value;

            return roles;
        }

        private string GetEmail()
        {
            var currentClaims = (contextAccessor.HttpContext.User?.Identity as ClaimsIdentity)?.Claims;
            string agentEmail = currentClaims?.First(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress").Value;

            return agentEmail;
        }
    }
}
