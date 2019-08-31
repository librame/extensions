using Xunit;

namespace Librame.Extensions.Encryption.Tests
{
    public class EncryptionBufferExtensionsTests
    {
        [Fact]
        public void AllTest()
        {
            var str = nameof(EncryptionBufferExtensionsTests);
            var plaintextBuffer = str.AsPlaintextBuffer(TestServiceProvider.Current);
            
            var hashString = plaintextBuffer
                .Md5()
                .Sha1()
                .Sha256()
                .Sha384()
                .Sha512()
                .HmacMd5()
                .HmacSha1()
                .HmacSha256()
                .HmacSha384()
                .HmacSha512()
                .AsCiphertextString();

            var plaintextBufferCopy = plaintextBuffer.Copy();

            var ciphertextString = plaintextBufferCopy
                .AsDes()
                .AsTripleDes()
                .AsAes()
                .AsRsa()
                .AsCiphertextString();
            Assert.NotEmpty(ciphertextString);

            var ciphertextBuffer = ciphertextString.AsCiphertextBuffer(TestServiceProvider.Current)
                .FromRsa()
                .FromAes()
                .FromTripleDes()
                .FromDes();

            Assert.True(plaintextBuffer.Equals(ciphertextBuffer));
            Assert.Equal(hashString, ciphertextBuffer.AsCiphertextString());
        }

    }
}
