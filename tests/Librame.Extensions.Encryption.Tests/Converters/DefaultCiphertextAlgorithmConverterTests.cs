using Xunit;

namespace Librame.Extensions.Encryption.Tests
{
    using Services;

    public class DefaultCiphertextAlgorithmConverterTests
    {
        [Fact]
        public void DefaultCiphertextAlgorithmConverterTest()
        {
            var rawCiphertextString = nameof(DefaultCiphertextAlgorithmConverterTests).AsEncodingBytes().AsBase64String();
            
            var converter = TestServiceProvider.Current.GetRequiredService<ICiphertextAlgorithmConverter>();

            var buffer = converter.ToResult(rawCiphertextString);
            var ciphertextString = converter.ToSource(buffer);

            Assert.Equal(rawCiphertextString, ciphertextString);
        }

    }
}
