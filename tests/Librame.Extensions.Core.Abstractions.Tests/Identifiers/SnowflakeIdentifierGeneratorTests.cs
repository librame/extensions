using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Librame.Extensions.Core.Tests
{
    using Identifiers;
    using Services;
    using Threads;

    public class SnowflakeIdentifierGeneratorTests
    {
        public sealed class NoneLoggerFactory : ILoggerFactory
        {
            public void AddProvider(ILoggerProvider provider)
                => throw new NotImplementedException();

            public ILogger CreateLogger(string categoryName)
                => null;

            public void Dispose()
            {
            }
        }

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


        [Fact]
        public void AllTest()
        {
            var clock = new TestClockService(new MemoryLockerTests.TestMemoryLocker(), new NoneLoggerFactory());
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
