using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Librame.Extensions.Data.Tests
{
    using Core.Services;
    using Core.Threads;
    using Data.Stores;

    public class EntityPopulatorTests
    {
        [Fact]
        public void AllTest()
        {
            var entity = new DataEntity<string>();

            var createdBy = entity.CreatedBy;
            var createdTime = entity.CreatedTime;
            var createdTimeTicks = entity.CreatedTimeTicks;

            EntityPopulator.PopulateCreationAsync<EntityPopulatorTests>(new TestClockService(), entity);

            Assert.NotEqual(createdBy, entity.CreatedBy);
            Assert.NotEqual(createdTime, entity.CreatedTime);
            Assert.NotEqual(createdTimeTicks, entity.CreatedTimeTicks);
        }


        private class TestClockService : IClockService
        {
            public IMemoryLocker Locker
                => throw new NotImplementedException();

            public ILoggerFactory LoggerFactory
                => throw new NotImplementedException();

            public Task<DateTime> GetNowAsync(DateTime timestamp, bool? isUtc = null, CancellationToken cancellationToken = default)
                => Task.FromResult(DateTime.UtcNow.AddHours(1));

            public Task<DateTimeOffset> GetOffsetNowAsync(DateTimeOffset timestamp, bool? isUtc = null, CancellationToken cancellationToken = default)
                => Task.FromResult(DateTimeOffset.UtcNow.AddHours(1));
        }

    }
}
