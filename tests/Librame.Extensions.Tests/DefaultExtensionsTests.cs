using Xunit;

namespace Librame.Extensions.Tests
{
    public class DefaultExtensionsTests
    {
        private string _str;

        [Fact]
        public void EnsureSingletonTest()
        {
            _str = _str.EnsureSingleton(() => "123");
            Assert.NotEmpty(_str);

            for (var i = 0; i < 10; i++)
            {
                Assert.NotEmpty(_str.EnsureSingleton(() => string.Empty));
            }
        }


        [Fact]
        public void EnsureValueTest()
        {
            // Number
            var defaultInt = 1;
            int? nullable = null;
            Assert.Equal(defaultInt, nullable.EnsureValue(defaultInt));

            // String
            var defaultString = "1";
            var str = string.Empty;
            Assert.Equal(defaultString, str.EnsureValue(defaultString));

            str = " ";
            Assert.NotEqual(defaultString, str.EnsureValue(defaultString));
        }
    }
}
