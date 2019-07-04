using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace Librame.Extensions.Core.Tests
{
    public class InternalHumanizationServiceTests
    {
        [Fact]
        public async void AllTest()
        {
            var service = TestServiceProvider.Current.GetRequiredService<IHumanizationService>();

            var now = DateTime.Now.AddMinutes(-2);
            var nowString = await service.HumanizeAsync(now);
            Assert.NotEqual(now.ToString(), nowString);

            var utcNow = DateTimeOffset.Now.AddDays(-3);
            var utcNowString = await service.HumanizeAsync(utcNow);
            Assert.NotEqual(utcNow.ToString(), utcNowString);
        }

    }
}
