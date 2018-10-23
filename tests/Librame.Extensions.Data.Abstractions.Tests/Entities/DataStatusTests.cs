using Xunit;

namespace Librame.Extensions.Data.Tests
{
    public class DataStatusTests
    {

        [Fact]
        public void BaseTest()
        {
            var list = typeof(DataStatus).AsEnumResults((f, v) => f);
            Assert.NotEmpty(list);
        }

    }
}
