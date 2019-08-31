using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Librame.Extensions.Encryption.Tests
{
    public class KeyedHashServiceTests
    {
        [Fact]
        public void AllTest()
        {
            var keyedHash = TestServiceProvider.Current.GetRequiredService<IKeyedHashService>();

            var plaintextString = nameof(KeyedHashServiceTests);

            // Create IPlaintextAlgorithmBuffer
            var plaintextBuffer = plaintextString.AsPlaintextBuffer(TestServiceProvider.Current);

            var buffer = keyedHash.HmacMd5(plaintextBuffer);
            buffer = keyedHash.HmacSha1(plaintextBuffer);
            buffer = keyedHash.HmacSha256(plaintextBuffer);
            buffer = keyedHash.HmacSha384(plaintextBuffer);
            buffer = keyedHash.HmacSha512(plaintextBuffer);

            Assert.NotEmpty(buffer.AsBase64String());
        }

    }
}
