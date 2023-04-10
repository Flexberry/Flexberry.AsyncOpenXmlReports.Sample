using Flexberry.Quartz.Sample.Service.Jobs;
using Flexberry.Quartz.Sample.Service.RequestsObjects;
using ICSSoft.STORMNET;
using ICSSoft.STORMNET.Business;
using Microsoft.AspNetCore.Mvc;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Flexberry.Quartz.Sample.Service.Controllers
{
    [Route("api/quartz")]
    [ApiController]
    public class QuartzController : ControllerBase
    {
        [HttpPost]
        [ActionName("TestReport")]
        public StatusCodeResult TestReport([FromBody] TestReportRequest request)//, [FromServices] IDataService ds, [FromServices] IUserWithRoles user)
        {
            LogService.LogDebugFormat("TestReport: params = '{0}'", request.ToString());

            /*
            user.Login = request.UserLogin;
            user.Domain = request.UserDomain;
            user.FriendlyName = request.UserFriendlyName;
            user.Roles = request.UserRoles;
            */
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
