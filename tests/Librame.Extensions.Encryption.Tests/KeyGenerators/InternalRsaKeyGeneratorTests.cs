using Xunit;

namespace Librame.Extensions.Encryption.Tests
{
    using Services;

    public class InternalRsaKeyGeneratorTests
    {
        [Fact]
        public void InternalRsaKeyGeneratorServiceTest()
        {
            var keyGenerator = TestServiceProvider.Current.GetRequiredService<IRsaKeyGenerator>();
            var key = keyGenerator.GenerateKeyParameters();

            var str = nameof(InternalRsaKeyGeneratorTests);
            var rsa = str.AsRsaBase64String(key.Parameters);

            Assert.Equal(str, rsa.FromRsaBase64String(key.Parameters));
        }

    }
}
