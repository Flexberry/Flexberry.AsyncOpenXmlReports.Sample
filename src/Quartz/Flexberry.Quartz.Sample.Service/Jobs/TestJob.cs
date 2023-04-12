using Flexberry.Quartz.Sample.Service.RequestsObjects;
using ICSSoft.STORMNET;
using ICSSoft.STORMNET.Business;
using Quartz;
using System;
using System.Threading.Tasks;
using Unity;

namespace Flexberry.Quartz.Sample.Service.Jobs
{
    /// <summary>
    /// Тестовая задача Quartz.
    /// </summary>
    public class TestJob : IJob
    {
        /// <summary>
        /// Вызывается Quartz.IScheduler при срабатывании Quartz.ITrigger, связанного с Quartz.IJob.
        /// </summary>
        /// <param name="context">Контекст выполнения.</param>
        /// <returns></returns>
        public Task Execute(IJobExecutionContext context)
        {
            var dataMap = context.JobDetail.JobDataMap;

            CheckParam(dataMap, "TestReportRequest");

            var request = dataMap["TestReportRequest"] as TestReportRequest;
            
            // Инициализация сервисов.
            var ds = Adapter.Container.Resolve<IDataService>();
            var user = Adapter.Container.Resolve<IUserWithRoles>();

            user.Login = request.UserLogin;
            user.Domain = request.UserDomain;
            user.FriendlyName = request.UserFriendlyName;
            user.Roles = request.UserRoles;

            var user2 = Adapter.Container.Resolve<IUserWithRoles>();

            LogService.Log.Info($"TestJob: request = {request}; user = {user2.Login}; ds = {ds.CustomizationString}");

            return Task.CompletedTask;
        }

        /// <summary>
        /// Получить свойства данного экземпляра задания.
        /// </summary>
        /// <param name="name">Имя задания.</param>
        /// <param name="group">Группа задания.</param>
        /// <returns>Подробные свойства данного экземпляра задания.</returns>
        public static IJobDetail GetTestDetail(string name, string group)
        {
            IJobDetail job = JobBuilder.Create<TestJob>()
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
        public static ITrigger GetTestTrigger(string name, string group)
        {
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity(name, group)
                .StartNow()
                .Build();

            return trigger;
        }

        /// <summary>
        /// Проверить наличие параметра в context.JobDetail.JobDataMap.
        /// </summary>
        /// <param name="dataMap">Информация по заданию.</param>
        /// <param name="name">Имя параметра.</param>
        /// <exception cref="ArgumentNullException">Если параметр не будет найден.</exception>
        private void CheckParam(JobDataMap dataMap, string name)
        {
            if (!dataMap.ContainsKey(name) || dataMap[name] == null)
            {
                throw new ArgumentNullException($"context.JobDetail.JobDataMap[{name}]");
            }
        }
    }
}
