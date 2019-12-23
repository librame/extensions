using Xunit;

namespace Librame.Extensions.Storage.Tests
{
    public class CapacityExtensionsTests
    {
        [Fact]
        public void FormatCapacityTest()
        {
            var intSize = 123_456;
            var formatString = intSize.FormatCapacity();
            Assert.NotEmpty(formatString);
            
            var longSize = 123_456_789L;
            formatString = longSize.FormatCapacity();
            Assert.NotEmpty(formatString);
        }

    }
}
