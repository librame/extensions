using Xunit;

namespace Librame.Extensions.Encryption.Tests
{
    public class InternalRsaAlgorithmServiceTests
    {
        [Fact]
        public void RsaAlgorithmServiceTest()
        {
            var rawPlaintextString = nameof(InternalRsaAlgorithmServiceTests);

            // Create IPlaintextAlgorithmBuffer
            var plaintextBuffer = rawPlaintextString.AsPlaintextBuffer(TestServiceProvider.Current);

            var ciphertextString = plaintextBuffer
                .AsRsa()
                .AsCiphertextString();
            
            // ciphertextString => ICiphertextAlgorithmBuffer
            var plaintextString = ciphertextString.AsCiphertextBuffer(plaintextBuffer.ServiceProvider)
                .FromRsa()
                .AsPlaintextString();
            
            Assert.Equal(rawPlaintextString, plaintextString);
        }

    }
}
