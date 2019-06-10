using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Librame.Extensions.Core.Tests
{
    public class InternalAccessTokenServiceTests
    {
        [Fact]
        public async void AllTest()
        {
            var service = TestServiceProvider.Current.GetRequiredService<IAccessTokenService>();
            var token = await service.GetTokenAsync(default);
            Assert.NotEmpty(token);
        }

    }
}
