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

            var accessToken = await service.GeAccessTokenAsync().ConfigureAwait(true);
            Assert.NotEmpty(accessToken);

            var authorizationCode = await service.GetAuthorizationCodeAsync().ConfigureAwait(true);
            Assert.NotEmpty(authorizationCode);

            var cookieValue = await service.GetCookieValueAsync().ConfigureAwait(true);
            Assert.NotEmpty(cookieValue);
        }

    }
}
