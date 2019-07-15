using Xunit;

namespace Librame.Extensions.Data.Tests
{
    public class DataStatusTests
    {
        [Fact]
        public void AllTest()
        {
            var list = typeof(DataStatus).AsEnumResults(f => f);
            Assert.NotEmpty(list);
        }

    }
}
