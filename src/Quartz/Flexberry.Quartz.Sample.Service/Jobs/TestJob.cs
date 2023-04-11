﻿using Flexberry.Quartz.Sample.Service.RequestsObjects;
using ICSSoft.STORMNET;
using ICSSoft.STORMNET.Business;
using Quartz;
using System;
using System.Threading.Tasks;

namespace Flexberry.Quartz.Sample.Service.Jobs
{
    public class TestJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            var dataMap = context.JobDetail.JobDataMap;

            CheckParam(dataMap, "TestReportRequest");
            CheckParam(dataMap, "SQLDataService");
            CheckParam(dataMap, "IUserWithRoles");

            var request = dataMap["TestReportRequest"] as TestReportRequest;
            var ds = dataMap["SQLDataService"] as SQLDataService;
            var user = dataMap["IUserWithRoles"] as IUserWithRoles;

            LogService.Log.Info($"TestJob: request = {request}; user = {user.Login}; ds = {ds.CustomizationString}");

            return Task.CompletedTask;
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

        private void CheckParam(JobDataMap dataMap, string name)
        {
            if (!dataMap.ContainsKey(name) || dataMap[name] == null)
            {
                throw new ArgumentNullException($"context.JobDetail.JobDataMap[{name}]");
            }
        }
    }
}
