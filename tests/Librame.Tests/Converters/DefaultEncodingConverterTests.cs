using Microsoft.Extensions.Logging;
using System.Text;
using Xunit;

namespace Librame.Converters.Tests
{
    using Librame.Extensions;

    public class DefaultEncodingConverterTests
    {

        [Fact]
        public void EncodingConverterTest()
        {
            var converter = Encoding.UTF8.AsEncodingConverter(new LoggerFactory());

            var str = nameof(DefaultEncodingConverterTests);

            var buffer = converter.ToResult(str);
            Assert.NotEmpty(buffer.Memory.ToArray());

            var source = converter.ToSource(buffer);
            Assert.Equal(str, source);
        }

    }
}
