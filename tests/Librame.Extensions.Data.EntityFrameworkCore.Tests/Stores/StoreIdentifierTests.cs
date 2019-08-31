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

            var auditId = await service.GetAuditIdAsync();
            Assert.NotEmpty(auditId);

            var auditPropertyId = await service.GetAuditPropertyIdAsync();
            Assert.NotEmpty(auditPropertyId);

            var tenantId = await service.GetTenantIdAsync();
            Assert.NotEmpty(tenantId);
        }
    }
}
