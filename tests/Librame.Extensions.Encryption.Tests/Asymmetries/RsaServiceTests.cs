using Xunit;

namespace Librame.Extensions.Encryption.Tests
{
    public class RsaServiceTests
    {
        [Fact]
        public void AllTest()
        {
            var rawPlaintextString = nameof(RsaServiceTests);

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
