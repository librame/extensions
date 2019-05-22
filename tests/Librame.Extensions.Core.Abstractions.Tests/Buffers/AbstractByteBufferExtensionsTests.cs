using System;
using Xunit;

namespace Librame.Extensions.Core.Tests
{
    public class AbstractByteBufferExtensionsTests
    {
        [Fact]
        public void AllTest()
        {
            var buffer = new TestByteBuffer(Memory<byte>.Empty);
            Assert.True(buffer.Memory.IsEmpty);

            var array = new byte[] { 12, 34 };
            buffer.Change(m => array);
            Assert.Equal(array.Length, buffer.Memory.Length);

            buffer.Clear();
            Assert.True(buffer.Memory.IsEmpty);
        }
    }


    public class TestByteBuffer : AbstractBuffer<byte>, IByteBuffer
    {
        public TestByteBuffer(Memory<byte> memory)
            : base(memory)
        {
        }

        public TestByteBuffer(byte[] array)
            : base(array)
        {
        }

        public override IBuffer<byte> Copy()
        {
            return new TestByteBuffer(Memory);
        }

        IByteBuffer IByteBuffer.Copy()
        {
            return new TestByteBuffer(Memory);
        }
    }
}
