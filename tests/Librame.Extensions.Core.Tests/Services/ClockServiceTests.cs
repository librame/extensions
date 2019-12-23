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

            var now = await service.GetNowAsync(DateTime.UtcNow, true).ConfigureAndResultAsync();
            Assert.Equal(now.Day, DateTime.UtcNow.Day);

            var utcNow = await service.GetOffsetNowAsync(DateTimeOffset.UtcNow, true).ConfigureAndResultAsync();
            Assert.Equal(utcNow.Day, DateTimeOffset.UtcNow.Day);
        }

    }
}
