namespace Flexberry.Quartz.Sample.Service.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Flexberry.Quartz.Sample.Service.Jobs;
    using Flexberry.Quartz.Sample.Service.RequestsObjects;
    using global::Quartz;
    using global::Quartz.Impl;
    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.Security;
    using Microsoft.AspNetCore.Mvc;
    using Unity;

    /// <summary>
    /// Контроллер запуска отчетов с использованием Quartz.
    /// </summary>
    [Route("api/quartz")]
    [ApiController]
    public class QuartzController : ControllerBase
    {
        private const string ConfigSectionName = "AllowedReportOperations";

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

            var checkResult = CheckOperation("SampleReport");

            if (checkResult != null)
                return checkResult;

            var runTask = new Task(async () =>
            {
                StdSchedulerFactory factory = new StdSchedulerFactory();
                IScheduler scheduler = await factory.GetScheduler();

                await scheduler.Start();

                var job = SampleJob.GetDetail("job1_" + request.Id, "group1_" + request.Id);
                var trigger = SampleJob.GetTrigger("trigger1_" + request.Id, "group1_" + request.Id);

                // Добавим к задаче данные запроса.
                job.JobDataMap.Add("SampleReportRequest", request);

                await scheduler.ScheduleJob(job, trigger);
            });

            runTask.Start();

            return Ok();
        }

        private StatusCodeResult CheckOperation(string reportName)
        {
            var confSection = Adapter.Configuration.GetSection(ConfigSectionName).GetChildren();
            var confElem = confSection.Where(x => x.Key == reportName);

            if (confElem.Any())
            {
                var oper = confElem.First().Value;
                var securityManager = Adapter.Container.Resolve<ISecurityManager>();

                if (securityManager == null)
                    return BadRequest();

                if (!securityManager.AccessCheck(oper))
                    return Unauthorized();
            }
            else
            {
                LogService.LogError($"Report {reportName} have no setting for operation! Configuration section name: {ConfigSectionName}");
                return NotFound();
            }

            return null;
        }
    }
}
