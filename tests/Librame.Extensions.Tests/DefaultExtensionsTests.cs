using Xunit;

namespace Librame.Extensions.Tests
{
    public class DefaultExtensionsTests
    {
        [Fact]
        public void HasOrDefaultTest()
        {
            // Number
            var defaultInt = 1;
            int? nullable = null;
            Assert.Equal(defaultInt, nullable.HasOrDefault(defaultInt));

            // String
            var defaultString = "1";
            var str = string.Empty;
            Assert.Equal(defaultString, str.HasOrDefault(defaultString));

            str = " ";
            Assert.NotEqual(defaultString, str.HasOrDefault(defaultString));
        }
    }
}
