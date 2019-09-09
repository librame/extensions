using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Librame.Extensions.Data.Tests
{
    public class AuditServiceTests
    {
        [Fact]
        public async void AllTest()
        {
            var service = TestServiceProvider.Current.GetRequiredService<IAuditService>();
            var audits = await service.RegistAsync(null);
            Assert.Null(audits);
        }
    }
}
