using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Librame.Extensions.Data.Tests
{
    public class InternalAuditServiceTests
    {
        [Fact]
        public async void AllTest()
        {
            var service = TestServiceProvider.Current.GetRequiredService<IAuditService>();
            var audits = await service.GetAuditsAsync(null);
            Assert.Empty(audits);
        }
    }
}
