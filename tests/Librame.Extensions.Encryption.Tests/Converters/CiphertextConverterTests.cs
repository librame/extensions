using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Librame.Extensions.Encryption.Tests
{
    public class CiphertextConverterTests
    {
        [Fact]
        public void DefaultCiphertextAlgorithmConverterTest()
        {
            var rawCiphertextString = nameof(CiphertextConverterTests).FromEncodingString().AsBase64String();
            
            var converter = TestServiceProvider.Current.GetRequiredService<ICiphertextConverter>();

            var buffer = converter.ConvertFrom(rawCiphertextString);
            var ciphertextString = converter.ConvertTo(buffer);

            Assert.Equal(rawCiphertextString, ciphertextString);
        }

    }
}
