using Xunit;

namespace Librame.Extensions.Tests
{
    public class ConversionExtensionsTests
    {

        [Fact]
        public void Base64StringTest()
        {
            var str = nameof(ConversionExtensionsTests);
            var base64 = str.AsEncodingBytes().AsBase64String();

            Assert.Equal(str, base64.FromBase64String().FromEncodingBytes());
        }


        [Fact]
        public void HexStringTest()
        {
            var str = nameof(ConversionExtensionsTests);
            var hex = str.AsEncodingBytes().AsHexString();

            Assert.Equal(str, hex.FromHexString().FromEncodingBytes());
        }

    }
}
