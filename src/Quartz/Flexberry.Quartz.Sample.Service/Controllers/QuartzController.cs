using Flexberry.Quartz.Sample.Service.Jobs;
using Flexberry.Quartz.Sample.Service.RequestsObjects;
using ICSSoft.STORMNET;
using Microsoft.AspNetCore.Mvc;
using Quartz;
using Quartz.Impl;
using System;
using System.Threading.Tasks;

namespace Flexberry.Quartz.Sample.Service.Controllers
{
    [Route("api/quartz")]
    [ApiController]
    public class QuartzController : ControllerBase
    {
        /// <summary>
        /// Тестовый метод формирования отчета.
        /// </summary>
        /// <param name="request">Переметры запроса <see cref="TestReportRequest">TestReportRequest</see>./></param>
        /// <param name="ds">Сервис данных.</param>
        /// <param name="user">Сервис пользователя.</param>
        /// <returns>Статус запроса <see cref="StatusCodeResult">StatusCodeResult</see>./></returns>
        [HttpPost]
        [ActionName("TestReport")]
        public StatusCodeResult TestReport([FromBody] TestReportRequest request)
        {
            LogService.LogDebugFormat("TestReport: params = '{0}'", request.ToString());

            var runTask = new Task(async () => {
                StdSchedulerFactory factory = new StdSchedulerFactory();
                IScheduler scheduler = await factory.GetScheduler();

                // and start it off
                await scheduler.Start();

                var job = TestJob.GetTestDetail("job1_" + request.Id, "group1_" + request.Id);
                var trigger = TestJob.GetTestTrigger("trigger1_" + request.Id, "group1_" + request.Id);

                job.JobDataMap.Add("TestReportRequest", request);

                await scheduler.ScheduleJob(job, trigger);
            });

            runTask.Start();

            return Ok();
        }
    }
}
