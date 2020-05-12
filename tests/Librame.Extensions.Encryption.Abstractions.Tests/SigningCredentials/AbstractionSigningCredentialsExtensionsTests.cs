using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography.X509Certificates;
using Xunit;

namespace Librame.Extensions.Encryption.Tests
{
    public class AbstractionSigningCredentialsExtensionsTests
    {
        [Fact]
        public void AllTest()
        {
            var combiner = "dotnetty.com.pfx".CombineCurrentDirectory();
            var credentials = new SigningCredentials(new X509SecurityKey(new X509Certificate2(combiner, "password")),
                SecurityAlgorithms.RsaSha256);

            var rsa = credentials.ResolveRsa();
            Assert.NotNull(rsa);

            var cert = credentials.ResolveCertificate();
            Assert.NotNull(cert);
        }

    }
}
