using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Librame.Extensions.Encryption.Tests
{
    using Buffers;

    public class InternalKeyedHashAlgorithmServiceTests
    {
        [Fact]
        public void InternalKeyedHashAlgorithmServiceTest()
        {
            var keyedHash = TestServiceProvider.Current.GetRequiredService<IKeyedHashAlgorithmService>();

            var plaintextString = nameof(InternalKeyedHashAlgorithmServiceTests);

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
