namespace Flexberry.Quartz.Sample.Service.Jobs
{
    using System;
    using System.Linq;
    using Flexberry.Quartz.Sample.Service.Controllers.RequestObjects;
    using global::Quartz;
    using ICSSoft.STORMNET.Security;
    using Unity;

    /// <summary>
    /// Утилиты отчетов.
    /// </summary>
    public class JobTools
    {
        /// <summary>
        /// Имя параметра имени отчета.
        /// </summary>
        public const string ReportNameParam = "ReportName";

        private const string ConfigSectionName = "AllowedReportOperations";

        /// <summary>
        /// Инициализировать пользователя.
        /// </summary>
        /// <param name="userInfo">Данные пользователя.</param>
        public void InitUserInfo(UserInfo userInfo)
        {
            var user = Adapter.Container.Resolve<IUserWithRoles>();

            user.Login = userInfo.Login;
            user.Domain = userInfo.Domain;
            user.FriendlyName = userInfo.FriendlyName;
            user.Roles = userInfo.Roles;
        }

        /// <summary>
        /// Получить параметр из context.JobDetail.JobDataMap.
        /// </summary>
        /// <typeparam name="TParam">Тип параметра.</typeparam>
        /// <param name="dataMap">Информация по заданию.</param>
        /// <param name="name">Имя параметра.</param>
        /// <exception cref="ArgumentNullException">Если параметр не будет найден.</exception>
        /// <returns>Значение параметра.</returns>
        public TParam GetParam<TParam>(JobDataMap dataMap, string name)
            where TParam : class
        {
            if (!dataMap.ContainsKey(name))
            {
                throw new ArgumentException($"context.JobDetail.JobDataMap[{name}]");
            }

            TParam value = dataMap[name] as TParam;

            if (value == null)
            {
                throw new ArgumentNullException($"context.JobDetail.JobDataMap[{name}]");
            }

            return value;
        }

        /// <summary>
        /// Проверить доступность отчета в системе полномочий.
        /// </summary>
        /// <param name="dataMap">Данные отчета.</param>
        /// <returns>Доступность отчета.</returns>
        public bool AccessCheck(JobDataMap dataMap)
        {
            var reportName = GetParam<string>(dataMap, ReportNameParam);
            var confSection = Adapter.Configuration.GetSection(ConfigSectionName).GetChildren();
            var confElem = confSection.Where(x => x.Key == reportName);

            if (confElem.Any())
            {
                var operationName = confElem.First().Value;

                return AccessCheck(operationName);
            } 
            else
            {
                throw new Exception($"Report {reportName} have no setting for operation! Configuration section name: {ConfigSectionName}");
            }
        }

        /// <summary>
        /// Проверить доступность отчета в системе полномочий.
        /// </summary>
        /// <param name="operationName">Имя операции.</param>
        /// <returns>Доступность отчета.</returns>
        public bool AccessCheck(string operationName)
        {
            var securityManager = Adapter.Container.Resolve<ISecurityManager>();

            return securityManager.AccessCheck(operationName);
        }
    }
}
