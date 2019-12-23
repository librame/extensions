using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Text;
using Xunit;

namespace Librame.Extensions.Encryption.Tests
{
    using Services;

    public class SigningCredentialsServiceTests
    {
        [Fact]
        public void InternalRsaKeyGeneratorServiceTest()
        {
            var provider = TestServiceProvider.Current.GetRequiredService<ISigningCredentialsService>();
            var credentials = provider.GetGlobalSigningCredentials();

            var rsa = credentials.ResolveRsa();

            var source = nameof(SigningCredentialsServiceTests);
            var encoding = Encoding.UTF8;

            var encryptString = rsa.Encrypt(source.FromEncodingString(encoding), RSAEncryptionPadding.Pkcs1)
                .AsBase64String();

            var decryptBuffer = rsa.Decrypt(encryptString.FromBase64String(), RSAEncryptionPadding.Pkcs1);

            Assert.Equal(source, decryptBuffer.AsEncodingString(encoding));
        }

    }
}
