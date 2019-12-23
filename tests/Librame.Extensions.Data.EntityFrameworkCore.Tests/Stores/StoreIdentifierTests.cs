using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Librame.Extensions.Data.Tests
{
    using Stores;

    public class StoreIdentifierTests
    {
        [Fact]
        public async void AllTest()
        {
            var service = TestServiceProvider.Current.GetRequiredService<IStoreIdentifier>();

            var auditId = await service.GetAuditIdAsync().ConfigureAndResultAsync();
            Assert.NotEmpty(auditId);

            var entityId = await service.GetEntityIdAsync().ConfigureAndResultAsync();
            Assert.NotEmpty(entityId);

            var migrationId = await service.GetMigrationIdAsync().ConfigureAndResultAsync();
            Assert.NotEmpty(migrationId);

            var tenantId = await service.GetTenantIdAsync().ConfigureAndResultAsync();
            Assert.NotEmpty(tenantId);
        }
    }
}
