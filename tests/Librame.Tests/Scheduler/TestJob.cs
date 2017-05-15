using System;
using Common.Logging;
using Quartz;

namespace Librame.Tests.Scheduler
{
    public sealed class TestJob : IJob
    {
        private readonly ILog _logger = LibrameArchitecture.LoggingAdapter.GetLogger<TestJob>();

        public void Execute(IJobExecutionContext context)
        {
            _logger.InfoFormat("Test Job 测试");
        }

    }

}
