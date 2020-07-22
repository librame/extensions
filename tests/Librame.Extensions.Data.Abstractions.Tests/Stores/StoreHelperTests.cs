using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Librame.Extensions.Data.Tests
{
    using Data.Stores;

    public class StoreHelperTests
    {
        [Fact]
        public async Task AllTest()
        {
            var entity = new DataTabulation<string, string>();

            var createdBy = entity.CreatedBy;
            var createdTime = entity.CreatedTime;
            var createdTimeTicks = entity.CreatedTimeTicks;

            Thread.Sleep(1000);

            await entity.PopulateCreationAsync(new TestClockService(),
                StoreHelper.CreatedByTypeName<StoreHelperTests>()).ConfigureAwait();

            Assert.NotEqual(createdBy, entity.CreatedBy);
            Assert.NotEqual(createdTime, entity.CreatedTime);
            Assert.NotEqual(createdTimeTicks, entity.CreatedTimeTicks);
        }

    }
}
