using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Librame.Extensions.Core.Tests
{
    public class InternalPlatformServiceTests
    {
        [Fact]
        public void AllTest()
        {
            var service = TestServiceProvider.Current.GetRequiredService<IPlatformService>();
            var info = service.GetEnvironmentInfoAsync(default).Result;
            Assert.NotNull(info);
            Assert.NotEmpty(info.ApplicationName);
        }

    }
}
