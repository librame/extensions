using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Librame.Extensions.Core.Tests
{
    public class EnvironmentServiceTests
    {
        [Fact]
        public async void AllTest()
        {
            var service = TestServiceProvider.Current.GetRequiredService<IEnvironmentService>();
            var info = await service.GetEnvironmentInfoAsync().ConfigureAndResultAsync();
            Assert.NotEmpty(info.ApplicationName);
        }

    }
}
