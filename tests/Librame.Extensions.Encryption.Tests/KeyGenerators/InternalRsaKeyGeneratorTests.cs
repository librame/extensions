using System.Security.Cryptography;
using Xunit;

namespace Librame.Extensions.Encryption.Tests
{
    public class InternalRsaKeyGeneratorTests
    {
        [Fact]
        public void InternalRsaKeyGeneratorServiceTest()
        {
            var provider = TestServiceProvider.Current.GetRequiredService<ISigningCredentialsProvider>();
            var credentials = provider.GetGlobalSigningCredentials();

            var rsa = credentials.ResolveRsa();

            var str = nameof(InternalRsaKeyGeneratorTests);

            var encryptString = rsa.Encrypt(str.AsEncodingBytes(), RSAEncryptionPadding.Pkcs1)
                .AsBase64String();

            var decryptBuffer = rsa.Decrypt(encryptString.FromBase64String(), RSAEncryptionPadding.Pkcs1);

            Assert.Equal(str, decryptBuffer.FromEncodingBytes());
        }

    }
}
