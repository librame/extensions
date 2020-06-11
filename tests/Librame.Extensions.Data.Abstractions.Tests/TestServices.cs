using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data.Tests
{
    using Core.Services;

    public class TestClockService : IClockService
    {
        public ILoggerFactory LoggerFactory
            => throw new NotImplementedException();

        public DateTime GetNow(DateTime? timestamp = null, bool? isUtc = null)
            => DateTime.UtcNow.AddHours(1);

        public Task<DateTime> GetNowAsync(DateTime? timestamp = null, bool? isUtc = null,
            CancellationToken cancellationToken = default)
            => Task.FromResult(DateTime.UtcNow.AddHours(1));

        public DateTimeOffset GetNowOffset(DateTimeOffset? timestamp = null, bool? isUtc = null)
            => DateTimeOffset.UtcNow.AddHours(1);

        public Task<DateTimeOffset> GetNowOffsetAsync(DateTimeOffset? timestamp = null, bool? isUtc = null,
            CancellationToken cancellationToken = default)
            => Task.FromResult(DateTimeOffset.UtcNow.AddHours(1));
    }
}
