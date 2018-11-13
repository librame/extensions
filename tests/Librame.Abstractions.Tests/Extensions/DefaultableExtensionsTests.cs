using Xunit;

namespace Librame.Extensions.Tests
{
    public class DefaultableExtensionsTests
    {

        [Fact]
        public void AsValueOrDefaultTest()
        {
            // Number
            var defaultInt = 1;

            int? nullable = null;
            Assert.Equal(defaultInt, nullable.AsValueOrDefault(defaultInt));

            int num = 0;
            Assert.Equal(defaultInt, num.AsValueOrDefault(defaultInt));

            // String
            var defaultString = "1";

            var str = string.Empty;
            Assert.Equal(defaultString, str.AsValueOrDefault(defaultString));

            str = " ";
            Assert.Equal(defaultString, str.AsValueOrDefault(defaultString));
        }

    }
}
