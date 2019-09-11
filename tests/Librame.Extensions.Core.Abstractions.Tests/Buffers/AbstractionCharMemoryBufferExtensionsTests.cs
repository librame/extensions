using System;
using Xunit;

namespace Librame.Extensions.Core.Tests
{
    public class AbstractionCharMemoryBufferExtensionsTests
    {
        [Fact]
        public void SplitKeyValueTest()
        {
            var buffer = new TestCharReadOnlyMemoryBuffer(nameof(Extensions).ToCharArray());
            var pair = buffer.SplitKeyValueByIndexOf("s");
            Assert.Equal("Exten", pair.Key);
            Assert.Equal("ions", pair.Value);

            pair = buffer.SplitKeyValueByLastIndexOf("s");
            Assert.Equal("Extension", pair.Key);
            Assert.Empty(pair.Value);
        }
    }


    public class TestCharReadOnlyMemoryBuffer : AbstractReadOnlyMemoryBuffer<char>, ICharReadOnlyMemoryBuffer
    {
        public TestCharReadOnlyMemoryBuffer(ReadOnlyMemory<char> memory)
            : base(memory)
        {
        }

        public TestCharReadOnlyMemoryBuffer(char[] array)
            : base(array)
        {
        }
    }
}
