using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Librame.Extensions.Data.Tests
{
    public class InternalIdentifierServiceTests
    {
        [Fact]
        public async void AllTest()
        {
            var service = TestServiceProvider.Current.GetRequiredService<IIdentifierService>();

            var auditId = await service.GetAuditIdAsync();
            Assert.NotEmpty(auditId);

            var auditPropertyId = await service.GetAuditPropertyIdAsync();
            Assert.NotEmpty(auditPropertyId);

            var tenantId = await service.GetTenantIdAsync();
            Assert.NotEmpty(tenantId);
        }
    }
}
