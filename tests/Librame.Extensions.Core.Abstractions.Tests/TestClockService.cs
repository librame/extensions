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

        public DateTime GetNow(DateTime timestamp, bool? isUtc = null)
            => DateTime.UtcNow;

        public Task<DateTime> GetNowAsync(DateTime timestamp, bool? isUtc = null,
            CancellationToken cancellationToken = default)
            => Task.FromResult(DateTime.UtcNow);

        public DateTimeOffset GetOffsetNow(DateTimeOffset timestamp, bool? isUtc = null)
            => DateTimeOffset.UtcNow;

        public Task<DateTimeOffset> GetOffsetNowAsync(DateTimeOffset timestamp, bool? isUtc = null,
            CancellationToken cancellationToken = default)
            => Task.FromResult(DateTimeOffset.UtcNow);
    }
}
