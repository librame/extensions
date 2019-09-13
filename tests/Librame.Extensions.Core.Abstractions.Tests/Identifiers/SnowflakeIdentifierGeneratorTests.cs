using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Librame.Extensions.Core.Tests
{
    public class SnowflakeIdentifierGeneratorTests
    {
        public class TestClockService : AbstractDisposable, IClockService
        {
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

            protected override void DisposeCore()
            {
            }
        }


        [Fact]
        public void AllTest()
        {
            var clock = new TestClockService();
            var generator = SnowflakeIdentifierGenerator.Default;

            var current = generator.Generate(clock);
            for (var i = 0; i < 100; i++)
            {
                var next = generator.Generate(clock);
                Assert.NotEqual(current, next);
                current = next;
            }
        }

    }
}
