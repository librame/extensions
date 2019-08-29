using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Librame.Extensions.Core.Tests
{
    public class PlatformServiceTests
    {
        [Fact]
        public async void AllTest()
        {
            var service = TestServiceProvider.Current.GetRequiredService<IPlatformService>();
            var info = await service.GetEnvironmentInfoAsync();
            Assert.NotEmpty(info.ApplicationName);
        }

    }
}
