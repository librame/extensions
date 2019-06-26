using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Librame.Extensions.Encryption.Tests
{
    public class CiphertextConverterTests
    {
        [Fact]
        public void DefaultCiphertextAlgorithmConverterTest()
        {
            var rawCiphertextString = nameof(CiphertextConverterTests).AsEncodingBytes().AsBase64String();
            
            var converter = TestServiceProvider.Current.GetRequiredService<ICiphertextConverter>();

            var buffer = converter.To(rawCiphertextString);
            var ciphertextString = converter.From(buffer);

            Assert.Equal(rawCiphertextString, ciphertextString);
        }

    }
}
