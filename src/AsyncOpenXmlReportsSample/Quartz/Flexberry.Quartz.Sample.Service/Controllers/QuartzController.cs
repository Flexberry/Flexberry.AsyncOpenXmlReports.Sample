namespace Flexberry.Quartz.Sample.Service.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Flexberry.Quartz.Sample.Service.Controllers.RequestObjects;
    using Flexberry.Quartz.Sample.Service.Jobs;
    using global::Quartz;
    using global::Quartz.Impl;
    using ICSSoft.STORMNET;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Контроллер запуска отчетов с использованием Quartz.
    /// </summary>
    [Route("api/quartz")]
    [ApiController]
    public class QuartzController : ControllerBase
    {
        /// <summary>
        /// Тестовый метод формирования отчета.
        /// </summary>
        /// <param name="request">Переметры запроса <see cref="SampleReportRequest">SampleReportRequest</see>.</param>
        /// <returns>Статус запроса <see cref="StatusCodeResult">StatusCodeResult</see>.</returns>
        [HttpPost]
        [ActionName("SampleReport")]
        public StatusCodeResult SampleReport([FromBody] SampleReportRequest request)
        {
            LogService.LogDebugFormat("SampleReport: params = '{0}'", request.ToString());

            var runTask = new Task(async () =>
            {
                StdSchedulerFactory factory = new StdSchedulerFactory();
                IScheduler scheduler = await factory.GetScheduler();

                await scheduler.Start();

                var job = SampleJob.GetDetail("job1_" + request.Id, "group1_" + request.Id);
                var trigger = SampleJob.GetTrigger("trigger1_" + request.Id, "group1_" + request.Id);

                // Добавим к задаче данные запроса.
                job.JobDataMap.Add(JobTools.ReportNameParam, "SampleReport");
                job.JobDataMap.Add("SampleReportRequest", request);

                await scheduler.ScheduleJob(job, trigger);
            });

            runTask.Start();

            return Ok();
        }
    }
}
