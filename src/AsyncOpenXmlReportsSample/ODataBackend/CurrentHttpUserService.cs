
namespace IIS.AsyncOpenXmlReportsSample
{
    using System;
    using System.Linq;
    using System.Security.Claims;
    using Microsoft.AspNetCore.Http;

    public class CurrentHttpUserService : ICSSoft.Services.CurrentUserService.IUser
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

        private string GetLogin()
        {
            var currentClaims = (contextAccessor.HttpContext.User?.Identity as ClaimsIdentity)?.Claims;
            string agentClaim = currentClaims?.FirstOrDefault(p => p.Type == "preferred_username").Value;

            return agentClaim;
        }

        private string GetEmail()
        {
            var currentClaims = (contextAccessor.HttpContext.User?.Identity as ClaimsIdentity)?.Claims;
            string agentEmail = currentClaims?.FirstOrDefault(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress").Value;

            return agentEmail;
        }

        /// <summary>
        /// Получить имя текущего пользователя из текущего HTTP контекста.
        /// </summary>
        /// <returns>
        /// Имя текущего пользователя.
        /// </returns>
        private string GetIdentityName()
        {
            string identity = GetLogin();
            return identity;
        }
    }
}

