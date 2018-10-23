using Xunit;

namespace Librame.Extensions.Encryption.Tests
{
    public class InternalSymmetricAlgorithmServiceTests
    {
        [Fact]
        public void AesTest()
        {
            var rawPlaintextString = nameof(InternalSymmetricAlgorithmServiceTests);

            // Create IPlaintextAlgorithmBuffer
            var plaintextBuffer = rawPlaintextString.AsPlaintextBuffer(TestServiceProvider.Current);

            var ciphertextString = plaintextBuffer
                .AsAes()
                .AsCiphertextString();

            // ciphertextString => ICiphertextAlgorithmBuffer
            var plaintextString = ciphertextString.AsCiphertextBuffer(plaintextBuffer.ServiceProvider)
                .FromAes()
                .AsPlaintextString();

            Assert.Equal(rawPlaintextString, plaintextString);
        }


        [Fact]
        public void DesTest()
        {
            var rawPlaintextString = nameof(InternalSymmetricAlgorithmServiceTests);

            // Create IPlaintextAlgorithmBuffer
            var plaintextBuffer = rawPlaintextString.AsPlaintextBuffer(TestServiceProvider.Current);

            var ciphertextString = plaintextBuffer
                .AsDes()
                .AsCiphertextString();

            // ciphertextString => ICiphertextAlgorithmBuffer
            var plaintextString = ciphertextString.AsCiphertextBuffer(plaintextBuffer.ServiceProvider)
                .FromDes()
                .AsPlaintextString();

            Assert.Equal(rawPlaintextString, plaintextString);
        }


        [Fact]
        public void TripleDesTest()
        {
            var rawPlaintextString = nameof(InternalSymmetricAlgorithmServiceTests);

            // Create IPlaintextAlgorithmBuffer
            var plaintextBuffer = rawPlaintextString.AsPlaintextBuffer(TestServiceProvider.Current);

            var ciphertextString = plaintextBuffer
                .AsTripleDes()
                .AsCiphertextString();

            // ciphertextString => ICiphertextAlgorithmBuffer
            var plaintextString = ciphertextString.AsCiphertextBuffer(plaintextBuffer.ServiceProvider)
                .FromTripleDes()
                .AsPlaintextString();

            Assert.Equal(rawPlaintextString, plaintextString);
        }

    }
}
