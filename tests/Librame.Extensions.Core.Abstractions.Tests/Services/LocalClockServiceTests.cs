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
            var now = await LocalClockService.Default.GetNowAsync().ConfigureAndResultAsync();
            Assert.Equal(now.Day, DateTime.UtcNow.Day);

            var nowOffset = await LocalClockService.Default.GetNowOffsetAsync().ConfigureAndResultAsync();
            Assert.Equal(nowOffset.Day, DateTimeOffset.UtcNow.Day);
        }

    }
}
