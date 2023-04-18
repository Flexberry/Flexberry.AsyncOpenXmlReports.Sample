namespace Flexberry.Quartz.Sample.Service.Jobs
{
    using System;
    using System.IO;
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

        /// <summary>
        /// Имя раздела в файле конфигурации, в котором указаны соответствия отчетов и операций.
        /// </summary>
        public const string AllowedReportOperationsConfigSectionName = "AllowedReportOperations";

        /// <summary>
        /// Имя параметра в файле конфигурации, который содержит путь хранения файлов отчета.
        /// </summary>
        public const string UploadUrlConfigParamName = "UploadUrl";

        /// <summary>
        /// Имя параметра в файле конфигурации, который содержит путь хранения файлов шаблона.
        /// </summary>
        public const string TemplatesPathConfigParamName = "TemplatesPath";

        /// <summary>
        /// Замена недопустимых символов в имени файла на символ "_" (подчеркивание).
        /// </summary>
        /// <param name="filename">Имя файла.</param>
        /// <param name="replaceValue">На что заменить некорректные символы.</param>
        /// <returns>Имя файла без запрещенных символов.</returns>
        public static string ReplaceInvalidChars(string filename, string replaceValue = "_")
        {
            if (filename == null)
                return null;

            return string.Join(replaceValue, filename.Split(Path.GetInvalidFileNameChars()));
        }

        /// <summary>
        /// Получить полный путь до файла отчета.
        /// </summary>
        /// <param name="reportFileName">Имя файла отчета.</param>
        /// <returns>Путь до файла отчета + имя файла отчета.</returns>
        public static string GetFullReportName(string reportFileName)
        {
            return Path.Combine(Adapter.Configuration[UploadUrlConfigParamName], reportFileName);
        }

        /// <summary>
        /// Получить полный путь до файла шаблона.
        /// </summary>
        /// <param name="templateFileName">Имя файла шаблона.</param>
        /// <returns>Путь до файла шаблона + имя файла шаблона.</returns>
        public static string GetFullTemplateName(string templateFileName)
        {
            return Path.Combine(Adapter.Configuration[TemplatesPathConfigParamName], templateFileName);
        }

        /// <summary>
        /// Инициализировать пользователя.
        /// </summary>
        /// <param name="userInfo">Данные пользователя.</param>
        public void InitUserInfo(UserInfo userInfo, IUserWithRoles user)
        {
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
            var confSection = Adapter.Configuration.GetSection(AllowedReportOperationsConfigSectionName).GetChildren();
            var confElem = confSection.Where(x => x.Key == reportName);

            if (confElem.Any())
            {
                var operationName = confElem.First().Value;

                return AccessCheck(operationName);
            }
            else
            {
                throw new Exception($"Report {reportName} have no setting for operation! Configuration section name: {AllowedReportOperationsConfigSectionName}");
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
