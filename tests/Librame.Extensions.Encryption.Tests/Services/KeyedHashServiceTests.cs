using Microsoft.Extensions.DependencyInjection;
using System.Text;
using Xunit;

namespace Librame.Extensions.Encryption.Tests
{
    using Services;

    public class KeyedHashServiceTests
    {
        [Fact]
        public void AllTest()
        {
            var keyedHash = TestServiceProvider.Current.GetRequiredService<IKeyedHashService>();

            var source = nameof(HashServiceTests);
            var encoding = Encoding.UTF8;

            var buffer = source.FromEncodingString(encoding);

            buffer = keyedHash.HmacMd5(buffer);
            buffer = keyedHash.HmacSha1(buffer);
            buffer = keyedHash.HmacSha256(buffer);
            buffer = keyedHash.HmacSha384(buffer);
            buffer = keyedHash.HmacSha512(buffer);

            Assert.NotEmpty(buffer.AsBase64String());
        }

    }
}
