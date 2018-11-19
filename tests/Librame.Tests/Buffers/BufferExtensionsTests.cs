using Xunit;

namespace Librame.Buffers.Tests
{
    using Extensions;

    public class BufferExtensionsTests
    {

        [Fact]
        public void BufferStringTest()
        {
            var str = "123:456=789:987";
            var bs = str.AsStringBuffer();

            // IndexOf: char separator
            var pair = bs.SplitKeyValueStringByIndexOf(':');
            Assert.Equal("123", pair.Key);
            Assert.Equal("456=789:987", pair.Value);

            // LastIndexOf: char separator
            pair = bs.SplitKeyValueStringByLastIndexOf(':');
            Assert.Equal("123:456=789", pair.Key);
            Assert.Equal("987", pair.Value);

            // IndexOf: string separator
            pair = bs.SplitKeyValueStringByIndexOf(":456=789:");
            Assert.Equal("123", pair.Key);
            Assert.Equal("987", pair.Value);

            // LastIndexOf: string separator
            pair = bs.SplitKeyValueStringByLastIndexOf("=789:987");
            Assert.Equal("123:456", pair.Key);
            Assert.Empty(pair.Value);
        }

    }
}
