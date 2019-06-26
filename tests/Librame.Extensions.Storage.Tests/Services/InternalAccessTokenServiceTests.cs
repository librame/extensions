using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Librame.Extensions.Storage.Tests
{
    public class InternalAccessTokenServiceTests
    {
        [Fact]
        public async void AllTest()
        {
            var service = TestServiceProvider.Current.GetRequiredService<IStorageTokenService>();
            var token = await service.GetTokenAsync(default);
            Assert.NotEmpty(token);
        }

    }
}
