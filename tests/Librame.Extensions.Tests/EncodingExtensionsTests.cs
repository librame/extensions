using System.Text;
using Xunit;

namespace Librame.Extensions.Tests
{
    public class EncodingExtensionsTests
    {
        [Fact]
        public void EncodingTest()
        {
            var encoding = Encoding.UTF8;
            var encodingName = encoding.FromEncoding();
            Assert.Equal(encoding, encodingName.AsEncoding());
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
        public void Base32StringTest()
        {
            var str = nameof(EncodingExtensionsTests);
            var base32 = str.AsEncodingBase32String();
            // "IVXGG33ENFXGORLYORSW443JN5XHGVDFON2HGAAA" Length: 40
            Assert.Equal(str, base32.FromEncodingBase32String());
        }

        [Fact]
        public void Base64StringTest()
        {
            var str = nameof(EncodingExtensionsTests);
            var base64 = str.AsEncodingBase64String();
            // "RW5JB2RPBMDFEHRLBNNPB25ZVGVZDHM=" Length: 32
            Assert.Equal(str, base64.FromEncodingBase64String());
        }

        [Fact]
        public void HexStringTest()
        {
            var str = nameof(EncodingExtensionsTests);
            var hex = str.AsEncodingHexString();
            // "456E636F64696E67457874656E73696F6E735465737473"Length: 46
            Assert.Equal(str, hex.FromEncodingHexString());
        }

    }
}
