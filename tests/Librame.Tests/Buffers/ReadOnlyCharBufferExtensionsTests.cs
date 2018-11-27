using Xunit;

namespace Librame.Buffers.Tests
{
    using Extensions;

    public class ReadOnlyCharBufferExtensionsTests
    {

        [Fact]
        public void SplitKeyValueStringByIndexOfTest()
        {
            var str = "123:456=789:987";
            var buffer = str.AsReadOnlyCharBuffer();

            // IndexOf: char separator
            var pair = buffer.SplitKeyValueStringByIndexOf(':');
            Assert.Equal("123", pair.Key);
            Assert.Equal("456=789:987", pair.Value);

            // LastIndexOf: char separator
            pair = buffer.SplitKeyValueStringByLastIndexOf(':');
            Assert.Equal("123:456=789", pair.Key);
            Assert.Equal("987", pair.Value);

            // IndexOf: string separator
            pair = buffer.SplitKeyValueStringByIndexOf(":456=789:");
            Assert.Equal("123", pair.Key);
            Assert.Equal("987", pair.Value);

            // LastIndexOf: string separator
            pair = buffer.SplitKeyValueStringByLastIndexOf("=789:987");
            Assert.Equal("123:456", pair.Key);
            Assert.Empty(pair.Value);
        }

    }
}
