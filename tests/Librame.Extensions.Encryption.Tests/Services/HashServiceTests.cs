using Microsoft.Extensions.DependencyInjection;
using System.Text;
using Xunit;

namespace Librame.Extensions.Encryption.Tests
{
    using Services;

    public class HashServiceTests
    {
        [Fact]
        public void AllTest()
        {
            var hash = TestServiceProvider.Current.GetRequiredService<IHashService>();

            var source = nameof(HashServiceTests);
            var encoding = Encoding.UTF8;

            var buffer = source.FromEncodingString(encoding);

            buffer = hash.Md5(buffer);
            buffer = hash.Sha1(buffer);
            buffer = hash.Sha256(buffer);
            buffer = hash.Sha384(buffer);
            buffer = hash.Sha512(buffer);

            Assert.NotEmpty(buffer.AsBase64String());
        }

    }
}
