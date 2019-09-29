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

            var plaintextString = plaintextBuffer.Converter.ConvertTo(plaintextBuffer);
            Assert.Equal(rawPlaintextString, plaintextString);

            var plaintextBuffer1 = plaintextBuffer.Converter.ConvertFrom(plaintextString);
            Assert.True(plaintextBuffer.Equals(plaintextBuffer1));
        }

    }
}
