using Flexberry.Quartz.Sample.Service.RequestsObjects;
using ICSSoft.STORMNET;
using ICSSoft.STORMNET.Business;
using Quartz;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Flexberry.Quartz.Sample.Service.Jobs
{
    public class TestJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            var dataMap = context.JobDetail.JobDataMap;

            if (dataMap.ContainsKey("TestReportRequest"))
            {
                var request = dataMap["TestReportRequest"] as TestReportRequest;

                LogService.Log.Info("TestJob: request = " + request.ToString());

                return Task.CompletedTask;
            } 
            else
            {
                throw new ArgumentNullException("context.JobDetail.JobDataMap[TestReportRequest]");
            }
        }

        public static IJobDetail GetTestDetail(string name, string group)
        {
            IJobDetail job = JobBuilder.Create<TestJob>()
                .WithIdentity(name, group)
                .Build();

            return job;
        }

        public static ITrigger GetTestTrigger(string name, string group)
        {
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity(name, group)
                .StartNow()
                .Build();

            return trigger;
        }
    }
}
