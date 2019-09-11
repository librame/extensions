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

            var plaintextString = plaintextBuffer.Converter.To(plaintextBuffer);
            Assert.Equal(rawPlaintextString, plaintextString);

            var plaintextBuffer1 = plaintextBuffer.Converter.From(plaintextString);
            Assert.True(plaintextBuffer.Equals(plaintextBuffer1));
        }

    }
}
