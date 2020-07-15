using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Librame.Extensions.Storage.Tests
{
    using Core.Combiners;
    using Services;

    public class FilePermissionServiceTests
    {
        [Fact]
        public async void AllTest()
        {
            var service = TestServiceProvider.Current.GetRequiredService<IFilePermissionService>();

            var accessToken = await service.GetAccessTokenAsync().ConfigureAwait();
            Assert.NotEmpty(accessToken);

            var authorizationCode = await service.GetAuthorizationCodeAsync().ConfigureAwait();
            Assert.NotEmpty(authorizationCode);

            var cookieValue = await service.GetCookieValueAsync().ConfigureAwait();
            Assert.NotEmpty(cookieValue);
        }

    }
}
