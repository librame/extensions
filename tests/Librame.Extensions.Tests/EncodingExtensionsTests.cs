using System.Text;
using Xunit;

namespace Librame.Extensions.Tests
{
    public class EncodingExtensionsTests
    {
        private readonly Encoding _encoding
            = Encoding.UTF8;


        [Fact]
        public void NameTest()
        {
            var encodingName = _encoding.AsName();
            Assert.Equal(_encoding, encodingName.FromName());
        }

        [Fact]
        public void EncodingStringTest()
        {
            var str = nameof(EncodingExtensionsTests);
            var buffer = str.FromEncodingString(_encoding);
            Assert.Equal(str, buffer.AsEncodingString(_encoding));
        }

        [Fact]
        public void Base32StringTest()
        {
            var str = nameof(EncodingExtensionsTests);
            var base32 = str.AsEncodingBase32String(_encoding);
            // "IVXGG33ENFXGORLYORSW443JN5XHGVDFON2HGAAA" Length: 40
            Assert.Equal(str, base32.FromEncodingBase32String(_encoding));
        }

        [Fact]
        public void Base64StringTest()
        {
            var str = nameof(EncodingExtensionsTests);
            var base64 = str.AsEncodingBase64String(_encoding);
            // "RW5jb2RpbmdFeHRlbnNpb25zVGVzdHM=" Length: 32
            Assert.Equal(str, base64.FromEncodingBase64String(_encoding));
        }

        [Fact]
        public void HexStringTest()
        {
            var str = nameof(EncodingExtensionsTests);
            var hex = str.AsEncodingHexString(_encoding);
            // "456E636F64696E67457874656E73696F6E735465737473"Length: 46
            Assert.Equal(str, hex.FromEncodingHexString(_encoding));
        }

    }
}
