namespace Flexberry.Quartz.Sample.Service.Jobs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Flexberry.Quartz.Sample.Service.Controllers.RequestObjects;
    using global::Quartz;
    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.Business;
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

                Dictionary<string, object> parameters = new Dictionary<string, object>();

                DocxReport template = new DocxReport(request.TemplatePath + @"\" + request.TemplateName);

                var lcs = new LoadingCustomizationStruct(null)
                {
                    LoadingTypes = new[] { typeof(Car) },
                    View = Car.Views.CarL,
                };

                List<Car> cars = ds.LoadObjects(lcs).Cast<Car>().ToList();

                List<Dictionary<string, object>> allCarsParameters = new List<Dictionary<string, object>>();

                foreach (Car car in cars)
                {
                    Dictionary<string, object> singleCarParameters = new Dictionary<string, object>();

                    singleCarParameters.Add("CarNumber", car.CarNumber);
                    singleCarParameters.Add("CarDate", car.CarDate.ToString("dd.MM.yyyy"));
                    singleCarParameters.Add("CarBody", EnumCaption.GetCaptionFor(car.CarBody));

                    allCarsParameters.Add(singleCarParameters);
                }

                parameters.Add("Car", allCarsParameters);

                template.BuildWithParameters(parameters);

                // TODO: сохранять файл не в папку с шаблонами; генерировать имя файла уникально
                template.SaveAs(request.TemplatePath + @"\Result-" + request.UserInfo.Login + "-" + DateTime.Now.ToString("dd.MM.yyyy") + ".docx");

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
