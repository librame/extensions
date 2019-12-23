using Xunit;

namespace Librame.Extensions.Encryption.Tests
{
    using Buffers;

    public class BufferExtensionsTests
    {
        [Fact]
        public void AllTest()
        {
            var source = nameof(BufferExtensionsTests);
            var plaintextBuffer = source.AsPlaintextBuffer(TestServiceProvider.Current);

            plaintextBuffer
                .UseHash((hash, buffer) =>
                {
                    buffer = hash.Md5(buffer);
                    buffer = hash.Sha1(buffer);
                    buffer = hash.Sha256(buffer);
                    buffer = hash.Sha384(buffer);
                    return hash.Sha512(buffer);
                })
                .UseKeyedHash((keyedHash, buffer) =>
                {
                    buffer = keyedHash.HmacMd5(buffer);
                    buffer = keyedHash.HmacSha1(buffer);
                    buffer = keyedHash.HmacSha256(buffer);
                    buffer = keyedHash.HmacSha384(buffer);
                    return keyedHash.HmacSha512(buffer);
                });
            
            var base64String = plaintextBuffer.AsBase64String();

            var algorithmBuffer = base64String.FromBase64StringAsAlgorithmBuffer(TestServiceProvider.Current);

            // 比较两者的缓冲区内容是否相同（下同）
            Assert.True(plaintextBuffer.Equals(algorithmBuffer));

            algorithmBuffer
                .UseSymmetric((symmetric, buffer) =>
                {
                    buffer = symmetric.EncryptDes(buffer);
                    buffer = symmetric.EncryptTripleDes(buffer);
                    return symmetric.EncryptAes(buffer);
                })
                .UseRsa((rsa, buffer) =>
                {
                    return rsa.Encrypt(buffer);
                });

            Assert.False(plaintextBuffer.Equals(algorithmBuffer));

            algorithmBuffer
                .UseRsa((rsa, buffer) =>
                {
                    return rsa.Decrypt(buffer);
                })
                .UseSymmetric((symmetric, buffer) =>
                {
                    buffer = symmetric.DecryptAes(buffer);
                    buffer = symmetric.DecryptTripleDes(buffer);
                    return symmetric.DecryptDes(buffer);
                });

            Assert.True(plaintextBuffer.Equals(algorithmBuffer));
        }

    }
}
