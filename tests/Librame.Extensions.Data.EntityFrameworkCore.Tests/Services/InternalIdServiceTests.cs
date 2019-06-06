using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Librame.Extensions.Data.Tests
{
    public class InternalIdServiceTests
    {
        [Fact]
        public async void AllTest()
        {
            var service = TestServiceProvider.Current.GetRequiredService<IIdService>();
            var id = await service.GetIdAsync(default);
            Assert.NotEmpty(id);
        }
    }
}
