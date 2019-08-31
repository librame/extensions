using Xunit;

namespace Librame.Extensions.Encryption.Tests
{
    public class EncryptionBufferTests
    {
        [Fact]
        public void AllTest()
        {
            var rawPlaintextString = nameof(EncryptionBufferTests);

            // Create IPlaintextAlgorithmBuffer
            var plaintextBuffer = rawPlaintextString.AsPlaintextBuffer(TestServiceProvider.Current);

            var plaintextString = plaintextBuffer.Converter.From(plaintextBuffer);
            Assert.Equal(rawPlaintextString, plaintextString);

            var plaintextBuffer1 = plaintextBuffer.Converter.To(plaintextString);
            // 值虽相同但引用地址不同，不适合用 Equals 比较
            //Assert.True(plaintextBuffer.Memory.Equals(plaintextBuffer1.Memory));
            Assert.Equal(plaintextString, plaintextBuffer.Converter.From(plaintextBuffer1));
        }

    }
}
