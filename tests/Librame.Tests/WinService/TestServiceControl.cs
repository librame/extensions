using Common.Logging;
using System;
using System.Timers;
using Topshelf;

namespace Librame.Tests.WinService
{
    public class TestServiceControl : ServiceControl
    {
        private Timer _timer = null;
        readonly ILog _log = LibrameArchitecture.LoggingAdapter.GetLogger<TestServiceControl>();

        public TestServiceControl()
        {
            _timer = new Timer(1000) { AutoReset = true };
            _timer.Elapsed += (sender, eventArgs) => _log.Info(DateTime.Now);
        }

        public bool Start(HostControl hostControl)
        {
            _timer.Start();
            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            _timer.Stop();
            return true;
        }

    }
}
