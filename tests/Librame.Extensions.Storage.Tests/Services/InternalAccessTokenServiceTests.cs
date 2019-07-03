using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Librame.Extensions.Storage.Tests
{
    public class InternalAccessTokenServiceTests
    {
        [Fact]
        public async void AllTest()
        {
            var service = TestServiceProvider.Current.GetRequiredService<IFilePermissionService>();
            var token = await service.GeAccessTokenAsync(default);
            Assert.NotEmpty(token);
        }

    }
}
