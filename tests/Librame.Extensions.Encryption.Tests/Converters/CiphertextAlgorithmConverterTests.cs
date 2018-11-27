using Xunit;

namespace Librame.Extensions.Encryption.Tests
{
    public class CiphertextAlgorithmConverterTests
    {
        [Fact]
        public void DefaultCiphertextAlgorithmConverterTest()
        {
            var rawCiphertextString = nameof(CiphertextAlgorithmConverterTests).AsEncodingBytes().AsBase64String();
            
            var converter = TestServiceProvider.Current.GetRequiredService<ICiphertextAlgorithmConverter>();

            var buffer = converter.ToResult(rawCiphertextString);
            var ciphertextString = converter.ToSource(buffer);

            Assert.Equal(rawCiphertextString, ciphertextString);
        }

    }
}
