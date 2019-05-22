using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Librame.Extensions.Core.Tests
{
    public class AccessTokenServiceTests
    {
        [Fact]
        public void AllTest()
        {
            var service = TestServiceProvider.Current.GetRequiredService<IAccessTokenService>();
            var token = service.GetTokenAsync(default).Result;
            Assert.NotEmpty(token);
        }

    }
}
