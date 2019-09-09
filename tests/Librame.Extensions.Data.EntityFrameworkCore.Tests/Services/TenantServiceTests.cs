using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Librame.Extensions.Data.Tests
{
    public class TenantServiceTests
    {
        [Fact]
        public async void AllTest()
        {
            var service = TestServiceProvider.Current.GetRequiredService<ITenantService>();
            var audits = await service.GetCurrentTenantAsync(null);
            Assert.Null(audits);
        }
    }
}
