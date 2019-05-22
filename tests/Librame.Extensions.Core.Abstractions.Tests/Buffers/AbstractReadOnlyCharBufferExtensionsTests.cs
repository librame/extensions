using System;
using Xunit;

namespace Librame.Extensions.Core.Tests
{
    public class AbstractReadOnlyCharBufferExtensionsTests
    {
        [Fact]
        public void SplitKeyValueTest()
        {
            var buffer = new TestReadOnlyCharBuffer(nameof(Extensions).ToCharArray());
            var pair = buffer.SplitKeyValueStringByIndexOf("s");
            Assert.Equal("Exten", pair.Key);
            Assert.Equal("ions", pair.Value);

            pair = buffer.SplitKeyValueStringByLastIndexOf("s");
            Assert.Equal("Extension", pair.Key);
            Assert.Empty(pair.Value);
        }
    }


    public class TestReadOnlyCharBuffer : AbstractReadOnlyBuffer<char>, IReadOnlyCharBuffer
    {
        public TestReadOnlyCharBuffer(ReadOnlyMemory<char> memory)
            : base(memory)
        {
        }

        public TestReadOnlyCharBuffer(char[] array)
            : base(array)
        {
        }

        public override IReadOnlyBuffer<char> Copy()
        {
            return new TestReadOnlyCharBuffer(Memory);
        }

        IReadOnlyCharBuffer IReadOnlyCharBuffer.Copy()
        {
            return new TestReadOnlyCharBuffer(Memory);
        }
    }
}
