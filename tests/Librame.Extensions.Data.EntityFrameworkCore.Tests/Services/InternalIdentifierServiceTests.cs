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
            var id = await service.GetAuditIdAsync(default);
            Assert.NotEmpty(id);
        }
    }
}
