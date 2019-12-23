using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Librame.Extensions.Encryption.Tests
{
    using KeyGenerators;

    public class KeyGeneratorTests
    {
        [Fact]
        public void InternalKeyGeneratorServiceTest()
        {
            var keyGenerator = TestServiceProvider.Current.GetRequiredService<IKeyGenerator>();

            var buffer = keyGenerator.GetKey64();
            Assert.Equal(8, buffer.Length);

            buffer = keyGenerator.GetKey128();
            Assert.Equal(16, buffer.Length);

            buffer = keyGenerator.GetKey192();
            Assert.Equal(24, buffer.Length);

            buffer = keyGenerator.GetKey256();
            Assert.Equal(32, buffer.Length);

            buffer = keyGenerator.GetKey384();
            Assert.Equal(48, buffer.Length);

            buffer = keyGenerator.GetKey512();
            Assert.Equal(64, buffer.Length);

            buffer = keyGenerator.GetKey1024();
            Assert.Equal(128, buffer.Length);

            buffer = keyGenerator.GetKey2048();
            Assert.Equal(256, buffer.Length);
        }

    }
}
