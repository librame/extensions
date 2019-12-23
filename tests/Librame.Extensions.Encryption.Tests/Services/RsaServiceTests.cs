using Microsoft.Extensions.DependencyInjection;
using System.Text;
using Xunit;

namespace Librame.Extensions.Encryption.Tests
{
    using Services;

    public class RsaServiceTests
    {
        [Fact]
        public void AllTest()
        {
            var rsa = TestServiceProvider.Current.GetRequiredService<IRsaService>();

            var source = nameof(RsaServiceTests);
            var encoding = Encoding.UTF8;

            var buffer = source.FromEncodingString(encoding);

            buffer = rsa.Encrypt(buffer);
            buffer = rsa.Decrypt(buffer);

            Assert.Equal(source, buffer.AsEncodingString(encoding));
        }

    }
}
