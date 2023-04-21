
namespace IIS.AsyncOpenXmlReportsSample
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using ICSSoft.STORMNET;
    using Microsoft.AspNetCore.Http;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Реализация сервиса текущего пользователя.
    /// </summary>
    public class CurrentHttpUserService : IUserWithRolesAndEmail
    {
        private IHttpContextAccessor contextAccessor;

        /// <summary>
        /// Инициализация контекста сервиса текущего пользователя.
        /// </summary>
        /// <param name="contextAccessor">Контекст пользователя.</param>
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

        /// <summary>
        /// Роли пользователя.
        /// </summary>
        public string Roles
        {
            get { return GetRoles(); }
            set { }
        }

        /// <summary>
        /// Почта пользователя.
        /// </summary>
        public string Email
        {
            get { return GetEmail(); }
            set { }
        }

        /// <summary>
        /// Получить логин пользователя.
        /// </summary>
        /// <returns>Логин пользователя.</returns>
        private string GetLogin()
        {
            var currentClaims = GetCurrentClaims();
            string agentClaim = currentClaims?.First(p => p.Type == "preferred_username").Value;

            return agentClaim;
        }

        /// <summary>
        /// Получить список ролей пользоватля.
        /// </summary>
        /// <returns>Список ролей пользоватля.</returns>
        private string GetRoles()
        {
            var currentClaims = GetCurrentClaims();
            string roles = currentClaims?.First(p => p.Type == "resource_access").Value;

            try
            {
                var roles_json = JObject.Parse(roles);
                var roles_token = roles_json.SelectToken("ember-app.roles");

                roles = string.Join(",", roles_token.Values<string>());
            }
            catch (Exception ex)
            {
                LogService.Log.Error($"GetRoles({roles})", ex);
            }

            return roles;
        }

        /// <summary>
        /// Получить значение почты пользователя.
        /// </summary>
        /// <returns>Почта пользователя.</returns>
        private string GetEmail()
        {
            var currentClaims = GetCurrentClaims();
            string agentEmail = currentClaims?.First(p => p.Type.EndsWith("/emailaddress", StringComparison.InvariantCultureIgnoreCase)).Value;

            return agentEmail;
        }

        /// <summary>
        /// Получить список текущих настроек пользователя.
        /// </summary>
        /// <returns>Список настроек пользователя.</returns>
        private List<Claim> GetCurrentClaims()
        {
            var lstResult = (contextAccessor.HttpContext.User?.Identity as ClaimsIdentity)?.Claims.ToList();

            return lstResult;
        }
    }
}
