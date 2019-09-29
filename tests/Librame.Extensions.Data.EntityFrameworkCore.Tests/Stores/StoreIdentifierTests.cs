using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Librame.Extensions.Data.Tests
{
    public class StoreIdentifierTests
    {
        [Fact]
        public async void AllTest()
        {
            var service = TestServiceProvider.Current.GetRequiredService<IStoreIdentifier>();

            var auditId = await service.GetAuditIdAsync().ConfigureAndResultAsync();
            Assert.NotEmpty(auditId);

            var auditPropertyId = await service.GetAuditPropertyIdAsync().ConfigureAndResultAsync();
            Assert.NotEmpty(auditPropertyId);

            var tenantId = await service.GetTenantIdAsync().ConfigureAndResultAsync();
            Assert.NotEmpty(tenantId);
        }
    }
}
