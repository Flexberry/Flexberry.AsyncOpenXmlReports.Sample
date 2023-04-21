namespace Flexberry.Quartz.Sample.Service.Jobs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using Flexberry.Quartz.Sample.Service.Controllers.RequestObjects;
    using global::Quartz;
    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.Business;
    using ICSSoft.STORMNET.Business.LINQProvider;
    using IIS.AsyncOpenXmlReportsSample;
    using NewPlatform.Flexberry.Reports;
    using Newtonsoft.Json;
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
        /// <param name="delaySeconds">Задержка перед выполнением в секундах.</param>
        /// <returns>Триггер для данного экземпляра задания.</returns>
        public static ITrigger GetTrigger(string name, string group, int delaySeconds = 0)
        {
            var triggerBuilder = TriggerBuilder.Create().WithIdentity(name, group);

            if (delaySeconds > 0)
            {
                triggerBuilder.StartAt(DateTime.UtcNow.AddSeconds(delaySeconds));
            }
            else
            {
                triggerBuilder.StartNow();
            }

            ITrigger trigger = triggerBuilder.Build();

            return trigger;
        }

        /// <summary>
        /// Вызывается Quartz.IScheduler при срабатывании Quartz.ITrigger, связанного с Quartz.IJob.
        /// </summary>
        /// <param name="context">Контекст выполнения.</param>
        /// <returns>Task.</returns>
        public Task Execute(IJobExecutionContext context)
        {
            JobDataMap dataMap = null;
            CarListReportRequest request = null;

            if (context == null)
                throw new ArgumentNullException(nameof(context));

            try
            {
                dataMap = context.JobDetail.JobDataMap;
                request = jobTools.GetParam<CarListReportRequest>(dataMap, "CarListReportRequest");
            }
            catch (Exception ex)
            {
                LogService.Log.Error(ex);

                throw;
            }

            try
            {
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
                        .ToList()
                        .Select(car =>
                            new Dictionary<string, object>()
                                {
                                    { "CarNumber", car.CarNumber },
                                    { "CarDate", car.CarDate.ToString("dd.MM.yyyy") },
                                    { "CarBody", EnumCaption.GetCaptionFor(car.CarBody) },
                                })
                        .ToList();

                var fileDirectory = JobTools.CreateReportDirectory(request.Id);
                var fileName = JobTools.GetReportName(request.TemplateName, request.UserInfo.Login);
                var fullFileName = JobTools.GetFullReportName(fileDirectory, fileName);
                var parameters = new Dictionary<string, object> { { "Car", allCarsParameters } };
                var fullTamplateName = JobTools.GetFullTemplateName(request.TemplateName);
                var template = new DocxReport(fullTamplateName);

                template.BuildWithParameters(parameters);
                template.SaveAs(fullFileName);

                return SendResultAsync(request.Id, fileName, "Executed");
            }
            catch (Exception ex)
            {
                LogService.Log.Error(ex);

                return SendResultAsync(request.Id, string.Empty, "Unexecuted");
            }
        }

        /// <summary>
        /// Отправить результат обработки файла.
        /// </summary>
        /// <param name="requestId">Идентификатор запроса.</param>
        /// <param name="fileName">Имя файла отчета.</param>
        /// <param name="status">Статус обработки. InProgress, Unexecuted, Executed.</param>
        private static async Task SendResultAsync(string requestId, string fileName, string status)
        {
            var sendResultUrl = JobTools.GetFullUrlPath("api/CarListReport", "BuildResult");

            object input = new
            {
                Id = requestId,
                FileName = fileName,
                Status = status,
            };
            var msg = JsonConvert.SerializeObject(input);

            LogService.Log.Debug($"CarListJob: Sending {msg} to {sendResultUrl}.");

            var buffer = Encoding.UTF8.GetBytes(msg);

            using (var byteContent = new ByteArrayContent(buffer))
            {
                byteContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.PostAsync(sendResultUrl, byteContent).ConfigureAwait(true))
                    {
                        LogService.Log.Debug($"CarListJob: Sending status = {response.StatusCode}.");
                    }
                }
            }
        }
    }
}
