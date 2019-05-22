using System.Text;
using Xunit;

namespace Librame.Extensions.Tests
{
    public class EncodingExtensionsTests
    {
        [Fact]
        public void AsEncodingTest()
        {
            var encoding = Encoding.UTF8;
            Assert.Equal(encoding, encoding.AsName().AsEncoding());
        }

        [Fact]
        public void EncodingBytesTest()
        {
            var str = nameof(EncodingExtensionsTests);

            var buffer = str.AsEncodingBytes();
            Assert.NotNull(buffer);

            Assert.Equal(str, buffer.FromEncodingBytes());
        }

        [Fact]
        public void Base64StringTest()
        {
            var str = nameof(EncodingExtensionsTests);
            var base64 = str.AsEncodingBytes().AsBase64String();

            Assert.Equal(str, base64.FromBase64String().FromEncodingBytes());
        }

        [Fact]
        public void HexStringTest()
        {
            var str = nameof(EncodingExtensionsTests);
            var hex = str.AsEncodingBytes().AsHexString();

            Assert.Equal(str, hex.FromHexString().FromEncodingBytes());
        }

    }
}
