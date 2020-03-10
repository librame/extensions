using System;
using Xunit;

namespace Librame.Extensions.Encryption.Tests
{
    using Encryption.KeyGenerators;

    public class KeyDescriptorTests
    {
        [Fact]
        public void AllTest()
        {
            var descriptor = KeyDescriptor.New();

            Assert.True(descriptor.TryToGuid(out _));
            Assert.NotEmpty(descriptor.ToShortString(DateTime.Now));

            var memory = descriptor.ToReadOnlyMemory();
            Assert.False(memory.IsEmpty);
        }
    }
}
