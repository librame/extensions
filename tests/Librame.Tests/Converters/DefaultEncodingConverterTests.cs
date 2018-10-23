using Microsoft.Extensions.Logging;
using System.Text;
using Xunit;

namespace Librame.Converters.Tests
{
    public class DefaultEncodingConverterTests
    {

        [Fact]
        public void EncodingConverterTest()
        {
            var converter = Encoding.UTF8.AsDefaultEncodingConverter(new LoggerFactory().CreateLogger<DefaultEncodingConverter>());

            var str = nameof(DefaultEncodingConverterTests);

            var buffer = converter.ToResult(str);
            Assert.NotEmpty(buffer.Memory.ToArray());

            var source = converter.ToSource(buffer);
            Assert.Equal(str, source);
        }

    }
}
