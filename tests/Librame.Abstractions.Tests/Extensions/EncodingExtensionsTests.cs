using Xunit;

namespace Librame.Extensions.Tests
{
    public class EncodingExtensionsTests
    {

        [Fact]
        public void EncodingBytesTest()
        {
            var str = nameof(EncodingExtensionsTests);
            var buffer = str.AsEncodingBytes();
            Assert.NotNull(buffer);

            Assert.Equal(str, buffer.FromEncodingBytes());
        }

    }
}
