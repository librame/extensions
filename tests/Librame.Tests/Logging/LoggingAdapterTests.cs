using Microsoft.VisualStudio.TestTools.UnitTesting;
using Librame.Logging;

namespace Librame.Tests.Logging
{
    [TestClass()]
    public class LoggingAdapterTests
    {
        private ILoggingAdapter _adapter = null;

        public LoggingAdapterTests()
        {
            _adapter = LibrameArchitecture.AdapterManager.Logging;
        }


        [TestMethod()]
        public void CommonLoggingTest()
        {
            var logger = _adapter.GetLogger<LoggingAdapterTests>();
            Assert.IsNotNull(logger);

            logger.Info("write test");
        }


        [TestMethod()]
        public void LogWriterLoggingTest()
        {
            var logger = (_adapter.GetLogger<LoggingAdapterTests>() as Librame.Logging.EntLib.EntLibLogger);
            Assert.IsNotNull(logger);
            
            var log = LogHelper.PopulateLogEntry<LoggingAdapterTests>("This is a test message");
            if (logger.LogWriter.ShouldLog(log))
                logger.LogWriter.Write(log);
        }

    }
}