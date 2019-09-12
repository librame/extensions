using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace Librame.Extensions.Core.Tests
{
    public class ClockServiceTests
    {
        [Fact]
        public async void AllTest()
        {
            var service = TestServiceProvider.Current.GetRequiredService<IClockService>();

            var now = await service.GetNowAsync();
            Assert.Equal(now.Day, DateTime.Now.Day);

            var utcNow = await service.GetOffsetNowAsync();
            Assert.Equal(utcNow.Day, DateTimeOffset.Now.Day);
        }

    }
}
