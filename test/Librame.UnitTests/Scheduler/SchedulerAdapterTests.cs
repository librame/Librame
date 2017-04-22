﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Librame.Scheduler;

namespace Librame.UnitTests.Scheduler
{
    [TestClass()]
    public class SchedulerAdapterTests
    {
        private readonly ISchedulerAdapter _adapter;

        public SchedulerAdapterTests()
        {
            _adapter = LibrameArchitecture.AdapterManager.SchedulerAdapter;
        }


        [TestMethod()]
        public void AddJobTest()
        {
            var group = "LibrameScheduler";
            var job = SchedulerHelper.BuildJob<TestJob>("TestJob", group);
            var trigger = SchedulerHelper.BuildTrigger("TestTrigger", group, (sb) =>
            {
                sb.WithIntervalInSeconds(3);
            });

            var offset = _adapter.AddJob(job, trigger);
            Assert.IsNotNull(offset);
        }

    }
}