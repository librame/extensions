using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Core.Tests
{
    using Services;

    public class TestClockService : AbstractService, IClockService
    {
        public TestClockService(ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
        }

        public DateTime GetNow(DateTime? timestamp = null, bool? isUtc = null)
            => DateTime.UtcNow;

        public Task<DateTime> GetNowAsync(DateTime? timestamp = null, bool? isUtc = null,
            CancellationToken cancellationToken = default)
            => Task.FromResult(DateTime.UtcNow);

        public DateTimeOffset GetOffsetNow(DateTimeOffset? timestamp = null, bool? isUtc = null)
            => DateTimeOffset.UtcNow;

        public Task<DateTimeOffset> GetOffsetNowAsync(DateTimeOffset? timestamp = null, bool? isUtc = null,
            CancellationToken cancellationToken = default)
            => Task.FromResult(DateTimeOffset.UtcNow);
    }
}
