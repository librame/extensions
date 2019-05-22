using Xunit;

namespace Librame.Extensions.Encryption.Tests
{
    public class InternalRsaServiceTests
    {
        [Fact]
        public void RsaAlgorithmServiceTest()
        {
            var rawPlaintextString = nameof(InternalRsaServiceTests);

            // Create IPlaintextBuffer
            var plaintextBuffer = rawPlaintextString.AsPlaintextBuffer(TestServiceProvider.Current);

            var ciphertextString = plaintextBuffer
                .AsRsa()
                .AsCiphertextString();
            
            // ciphertextString => ICiphertextBuffer
            var plaintextString = ciphertextString.AsCiphertextBuffer(plaintextBuffer.ServiceProvider)
                .FromRsa()
                .AsPlaintextString();
            
            Assert.Equal(rawPlaintextString, plaintextString);
        }

    }
}
