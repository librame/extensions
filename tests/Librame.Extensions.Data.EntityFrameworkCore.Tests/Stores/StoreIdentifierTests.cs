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

            var auditId = await service.GetAuditIdAsync().ConfigureAwait(true);
            Assert.NotEmpty(auditId);

            var auditPropertyId = await service.GetAuditPropertyIdAsync().ConfigureAwait(true);
            Assert.NotEmpty(auditPropertyId);

            var tenantId = await service.GetTenantIdAsync().ConfigureAwait(true);
            Assert.NotEmpty(tenantId);
        }
    }
}
