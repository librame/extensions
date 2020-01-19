using System.ComponentModel;
using System.Text;
using Xunit;

namespace Librame.Extensions.Core.Tests
{
    using Converters;

    public class EncodingStringConverterTests
    {
        [Fact]
        public void AllTest()
        {
            var test = Encoding.UTF8;
            var converter = EncodingStringConverter.Default;

            var value = converter.ConvertToString(test);
            Assert.NotEmpty(value);

            var source = converter.ConvertFromString<Encoding>(value);
            Assert.NotNull(source);
            Assert.Equal(test, source);
        }
    }
}
