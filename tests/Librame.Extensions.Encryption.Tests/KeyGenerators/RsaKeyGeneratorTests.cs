using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using Xunit;

namespace Librame.Extensions.Encryption.Tests
{
    public class RsaKeyGeneratorTests
    {
        [Fact]
        public void InternalRsaKeyGeneratorServiceTest()
        {
            var provider = TestServiceProvider.Current.GetRequiredService<ISigningCredentialsService>();
            var credentials = provider.GetGlobalSigningCredentials();

            var rsa = credentials.ResolveRsa();

            var str = nameof(RsaKeyGeneratorTests);

            var encryptString = rsa.Encrypt(str.AsEncodingBytes(), RSAEncryptionPadding.Pkcs1)
                .AsBase64String();

            var decryptBuffer = rsa.Decrypt(encryptString.FromBase64String(), RSAEncryptionPadding.Pkcs1);

            Assert.Equal(str, decryptBuffer.FromEncodingBytes());
        }

    }
}
