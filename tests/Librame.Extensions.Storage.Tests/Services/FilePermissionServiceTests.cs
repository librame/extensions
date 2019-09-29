using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Librame.Extensions.Storage.Tests
{
    public class FilePermissionServiceTests
    {
        [Fact]
        public async void AllTest()
        {
            var service = TestServiceProvider.Current.GetRequiredService<IFilePermissionService>();

            var accessToken = await service.GeAccessTokenAsync().ConfigureAndResultAsync();
            Assert.NotEmpty(accessToken);

            var authorizationCode = await service.GetAuthorizationCodeAsync().ConfigureAndResultAsync();
            Assert.NotEmpty(authorizationCode);

            var cookieValue = await service.GetCookieValueAsync().ConfigureAndResultAsync();
            Assert.NotEmpty(cookieValue);
        }

    }
}
