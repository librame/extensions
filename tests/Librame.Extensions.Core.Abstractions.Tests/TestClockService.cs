using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Core.Tests
{
    using Services;
    using Threads;

    public class TestClockService : AbstractConcurrentService, IClockService
    {
        public TestClockService(IMemoryLocker locker, ILoggerFactory loggerFactory)
            : base(locker, loggerFactory)
        {
        }


        public Task<DateTime> GetNowAsync(DateTime timestamp, bool? isUtc = null,
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult(DateTime.UtcNow);
        }

        public Task<DateTimeOffset> GetOffsetNowAsync(DateTimeOffset timestamp, bool? isUtc = null,
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult(DateTimeOffset.UtcNow);
        }
    }
}
