using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace Librame.Extensions.Core.Tests
{
    using Services;

    public class ClockServiceTests
    {
        [Fact]
        public async void AllTest()
        {
            var service = TestServiceProvider.Current.GetRequiredService<IClockService>();

            var now = await service.GetNowAsync().ConfigureAwait();
            Assert.Equal(now.Day, DateTime.UtcNow.Day);

            var nowOffset = await service.GetNowOffsetAsync().ConfigureAwait();
            Assert.Equal(nowOffset.Day, DateTimeOffset.UtcNow.Day);
        }

    }
}
