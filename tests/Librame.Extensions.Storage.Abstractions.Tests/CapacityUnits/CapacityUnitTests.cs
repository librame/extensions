using Xunit;

namespace Librame.Extensions.Storage.Tests
{
    public class CapacityUnitTests
    {

        [Fact]
        public void FormatCapacityUnitStringTest()
        {
            var intSize = 123_456;
            var formatString = intSize.FormatCapacityUnitString();
            Assert.NotEmpty(formatString);
            
            var longSize = 123_456_789L;
            formatString = longSize.FormatCapacityUnitString();
            Assert.NotEmpty(formatString);
        }

    }
}
