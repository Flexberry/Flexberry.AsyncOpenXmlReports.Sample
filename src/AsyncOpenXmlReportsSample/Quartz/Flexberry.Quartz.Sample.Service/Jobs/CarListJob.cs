namespace Flexberry.Quartz.Sample.Service.Jobs
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Flexberry.Quartz.Sample.Service.Controllers.RequestObjects;
    using global::Quartz;
    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.Business;
    using ICSSoft.STORMNET.Business.LINQProvider;
    using IIS.AsyncOpenXmlReportsSample;
    using NewPlatform.Flexberry.Reports;
    using Unity;

    /// <summary>
    /// Задача Quartz построение отчета списка автомобилей.
    /// </summary>
    public class CarListJob : IJob
    {
        private readonly JobTools jobTools = new JobTools();

        /// <summary>
        /// Получить свойства данного экземпляра задания.
        /// </summary>
        /// <param name="name">Имя задания.</param>
        /// <param name="group">Группа задания.</param>
        /// <returns>Подробные свойства данного экземпляра задания.</returns>
        public static IJobDetail GetDetail(string name, string group)
        {
            IJobDetail job = JobBuilder.Create<CarListJob>()
                .WithIdentity(name, group)
                .Build();

            return job;
        }

        /// <summary>
        /// Получить триггер для данного экземпляра задания.
        /// </summary>
        /// <param name="name">Имя задания.</param>
        /// <param name="group">Группа задания.</param>
        /// <returns>Триггер для данного экземпляра задания.</returns>
        public static ITrigger GetTrigger(string name, string group)
        {
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity(name, group)
                .StartNow()
                .Build();

            return trigger;
        }

        /// <summary>
        /// Вызывается Quartz.IScheduler при срабатывании Quartz.ITrigger, связанного с Quartz.IJob.
        /// </summary>
        /// <param name="context">Контекст выполнения.</param>
        /// <returns>Task.</returns>
        public Task Execute(IJobExecutionContext context)
        {
            try
            {
                var dataMap = context.JobDetail.JobDataMap;
                var request = jobTools.GetParam<CarListReportRequest>(dataMap, "CarListReportRequest");
                var user = Adapter.Container.Resolve<IUserWithRoles>();

                jobTools.InitUserInfo(request.UserInfo, user);

                if (!jobTools.AccessCheck(dataMap))
                {
                    LogService.Log.Warn($"CarListJob has no access to execute: request = {request}.");

                    return Task.CompletedTask;
                }

                // Инициализация сервисов.
                var ds = Adapter.Container.Resolve<IDataService>();
                var conf = Adapter.Configuration;

                var allCarsParameters =
                    ds.Query<Car>(Car.Views.CarL)
                        .Select(car =>
                            new Dictionary<string, object>()
                                {
                                    { "CarNumber", car.CarNumber },
                                    { "CarDate", car.CarDate.ToString("dd.MM.yyyy") },
                                    { "CarBody", EnumCaption.GetCaptionFor(car.CarBody) },
                                })
                        .ToList();

                var dtValue = DateTime.Now.ToString("yyyyMMdd_HHmmss_fff", null);
                var fileName = JobTools.ReplaceInvalidChars($"{request.TemplateName}_{request.UserInfo.Login}_{request.Id}_{dtValue}.docx");
                var fullFileName = JobTools.GetFullReportName(fileName);
                var parameters = new Dictionary<string, object> { { "Car", allCarsParameters } };
                var fullTamplateName = JobTools.GetFullTemplateName(request.TemplateName);
                var template = new DocxReport(fullTamplateName);

                template.BuildWithParameters(parameters);
                template.SaveAs(fullFileName);

                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                LogService.Log.Error(ex);
                return Task.CompletedTask; // TODO: вернуть что-то подходящее
            }
        }
    }
}
