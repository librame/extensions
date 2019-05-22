using System;
using Xunit;

namespace Librame.Extensions.Core.Tests
{
    public class AbstractCharBufferExtensionsTests
    {
        [Fact]
        public void AllTest()
        {
            var buffer = new TestCharBuffer(Memory<char>.Empty);
            Assert.True(buffer.Memory.IsEmpty);

            var array = nameof(AbstractCharBufferExtensionsTests).ToCharArray();
            buffer.Change(m => array);
            Assert.Equal(array.Length, buffer.Memory.Length);

            buffer.Clear();
            Assert.True(buffer.Memory.IsEmpty);
        }


        [Fact]
        public void SplitKeyValueTest()
        {
            var buffer = new TestCharBuffer(nameof(Extensions).ToCharArray());
            var pair = buffer.SplitKeyValueStringByIndexOf("s");
            Assert.Equal("Exten", pair.Key);
            Assert.Equal("ions", pair.Value);

            pair = buffer.SplitKeyValueStringByLastIndexOf("s");
            Assert.Equal("Extension", pair.Key);
            Assert.Empty(pair.Value);
        }
    }


    public class TestCharBuffer : AbstractBuffer<char>, ICharBuffer
    {
        public TestCharBuffer(Memory<char> memory)
            : base(memory)
        {
        }

        public TestCharBuffer(char[] array)
            : base(array)
        {
        }

        public override IBuffer<char> Copy()
        {
            return new TestCharBuffer(Memory);
        }

        ICharBuffer ICharBuffer.Copy()
        {
            return new TestCharBuffer(Memory);
        }
    }
}
