using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Librame.Extensions.Data.Tests
{
    public class TableServiceTests
    {
        [Fact]
        public async void AllTest()
        {
            var service = TestServiceProvider.Current.GetRequiredService<IEntityService>();
            var tables = await service.RegistAsync(null);
            Assert.Null(tables);
        }
    }
}
