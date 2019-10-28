using System;
using Xunit;

namespace Librame.Extensions.Core.Tests
{
    public class ByteMemoryBufferTests
    {
        [Fact]
        public void AllTest()
        {
            IByteMemoryBuffer buffer = new TestByteMemoryBuffer(Memory<byte>.Empty);
            Assert.True(buffer.Memory.IsEmpty);

            var array = new byte[] { 12, 34 };
            buffer.ChangeMemory(memory => array);
            Assert.False(buffer.Memory.IsEmpty);
        }
    }


    public class TestByteMemoryBuffer : MemoryBuffer<byte>, IByteMemoryBuffer
    {
        public TestByteMemoryBuffer(Memory<byte> memory)
            : base(memory)
        {
        }

        public TestByteMemoryBuffer(byte[] array)
            : base(array)
        {
        }
    }
}
