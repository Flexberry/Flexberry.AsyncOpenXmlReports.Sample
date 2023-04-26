namespace Flexberry.Quartz.Sample.Service.Jobs
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using Flexberry.Quartz.Sample.Service.Controllers.RequestObjects;
    using global::Quartz;
    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.Business;
    using Unity;

    /// <summary>
    /// Тестовая задача Quartz.
    /// </summary>
    public class SampleJob : IJob
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
            IJobDetail job = JobBuilder.Create<SampleJob>()
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
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            var dataMap = context.JobDetail.JobDataMap;
            var request = jobTools.GetParam<SampleReportRequest>(dataMap, "SampleReportRequest");
            var user = Adapter.Container.Resolve<IUserWithRoles>();

            jobTools.InitUserInfo(request.UserInfo, user);

            if (!jobTools.AccessCheck(dataMap))
            {
                LogService.Log.Warn($"SampleJob has no access to execute: request = {request}.");

                return Task.CompletedTask;
            }

            // Инициализация сервисов.
            var ds = Adapter.Container.Resolve<IDataService>();
            var user2 = Adapter.Container.Resolve<IUserWithRoles>();

            LogService.Log.Info($"SampleJob: request = {request}; user = {user2.Login}; ds = {ds.CustomizationString}");

            try
            {
                var fileDirectory = JobTools.CreateReportDirectory(request.Id);
                var fileName = JobTools.GetFullReportName(fileDirectory , $"{request.Id}.txt");

                LogService.Log.Debug($"Creating file: {fileName}");
                File.WriteAllText(fileName, request.ToString());
            }
            catch (Exception ex)
            {
                LogService.Log.Error(ex);
            }

            return Task.CompletedTask;
        }
    }
}
