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

            var accessToken = await service.GeAccessTokenAsync();
            Assert.NotEmpty(accessToken);

            var authorizationCode = await service.GetAuthorizationCodeAsync();
            Assert.NotEmpty(authorizationCode);

            var cookieValue = await service.GetCookieValueAsync();
            Assert.NotEmpty(cookieValue);
        }

    }
}
