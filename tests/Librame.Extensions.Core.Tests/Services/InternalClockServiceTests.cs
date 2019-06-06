using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace Librame.Extensions.Core.Tests
{
    public class InternalClockServiceTests
    {
        [Fact]
        public void AllTest()
        {
            var service = TestServiceProvider.Current.GetRequiredService<IClockService>();
            var utcnow = service.GetUtcNowAsync(default).Result;
            Assert.Equal(utcnow.Day, DateTimeOffset.Now.Day);
        }

    }
}
