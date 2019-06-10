using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Librame.Extensions.Core.Tests
{
    public class InternalPlatformServiceTests
    {
        [Fact]
        public async void AllTest()
        {
            var service = TestServiceProvider.Current.GetRequiredService<IPlatformService>();
            var info = await service.GetEnvironmentInfoAsync(default);
            Assert.NotEmpty(info.ApplicationName);
        }

    }
}
