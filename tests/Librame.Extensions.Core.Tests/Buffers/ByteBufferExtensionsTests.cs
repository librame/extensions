using Xunit;

namespace Librame.Extensions.Core.Tests
{
    public class ByteBufferExtensionsTests
    {

        [Fact]
        public void AsBase64StringTest()
        {
            var base64String = "JUmlxL8G806eU4R5eSU+mEmlxL8G806e";
            var buffer = base64String.AsByteBufferFromBase64String();
            
            Assert.Equal(base64String, buffer.AsBase64String());
        }


        [Fact]
        public void AsHexStringTest()
        {
            var hexString = "ED191ADBBF263E49B0E396B782DB0D59";
            var buffer = hexString.AsByteBufferFromHexString();

            Assert.Equal(hexString, buffer.AsHexString());
        }

    }
}
