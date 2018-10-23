using Xunit;

namespace Librame.Extensions.Tests
{
    public class DefaultableExtensionsTests
    {

        [Fact]
        public void AsValueOrDefaultTest()
        {
            var defaultInt = 1;
            int? i = null;
            Assert.Equal(defaultInt, i.AsValueOrDefault(defaultInt));

            var defaultString = "1";
            var str = string.Empty;
            Assert.Equal(defaultString, str.AsValueOrDefault(defaultString));

            str = " ";
            Assert.Equal(defaultString, str.AsValueOrDefault(defaultString));
        }

    }
}
