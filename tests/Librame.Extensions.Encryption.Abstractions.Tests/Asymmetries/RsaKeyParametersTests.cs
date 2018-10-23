using System.Security.Cryptography;
using Xunit;

namespace Librame.Extensions.Encryption.Tests
{
    public class RsaKeyParametersTests
    {

        [Fact]
        public void CreateTest()
        {
            var parameters = RsaKeyParameters.Create(RSA.Create().ExportParameters(true));
            Assert.NotEmpty(parameters.KeyId);
        }

    }
}
