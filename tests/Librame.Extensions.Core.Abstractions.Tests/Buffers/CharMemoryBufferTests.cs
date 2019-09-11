using System;
using Xunit;

namespace Librame.Extensions.Core.Tests
{
    public class CharMemoryBufferTests
    {
        [Fact]
        public void AllTest()
        {
            var buffer = new TestCharMemoryBuffer(Memory<char>.Empty);
            Assert.True(buffer.Memory.IsEmpty);

            var array = nameof(Extensions).ToCharArray();
            buffer.ChangeMemory(memory => array);
            Assert.False(buffer.Memory.IsEmpty);
        }


        [Fact]
        public void SplitKeyValueTest()
        {
            var buffer = new TestCharMemoryBuffer(nameof(Extensions).ToCharArray());
            var pair = buffer.SplitKeyValueByIndexOf("s");
            Assert.Equal("Exten", pair.Key);
            Assert.Equal("ions", pair.Value);

            pair = buffer.SplitKeyValueByLastIndexOf("s");
            Assert.Equal("Extension", pair.Key);
            Assert.Empty(pair.Value);
        }
    }


    public class TestCharMemoryBuffer : AbstractMemoryBuffer<char>, ICharMemoryBuffer
    {
        public TestCharMemoryBuffer(Memory<char> memory)
            : base(memory)
        {
        }

        public TestCharMemoryBuffer(char[] array)
            : base(array)
        {
        }
    }
}
