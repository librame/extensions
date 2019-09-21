using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Librame.Extensions.Encryption.Tests
{
    using Core;

    public class HashServiceTests
    {
        [Fact]
        public void AllTest()
        {
            var hash = TestServiceProvider.Current.GetRequiredService<IHashService>();

            var plaintextString = nameof(HashServiceTests);

            // Create IPlaintextAlgorithmBuffer
            var plaintextBuffer = plaintextString.AsPlaintextBuffer(TestServiceProvider.Current);

            var buffer = hash.Md5(plaintextBuffer);
            buffer = hash.Sha1(plaintextBuffer);
            buffer = hash.Sha256(plaintextBuffer);
            buffer = hash.Sha384(plaintextBuffer);
            buffer = hash.Sha512(plaintextBuffer);

            Assert.NotEmpty(buffer.AsBase64String());
        }

    }
}
