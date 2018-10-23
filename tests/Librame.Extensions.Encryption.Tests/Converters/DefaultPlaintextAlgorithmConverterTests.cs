using Xunit;

namespace Librame.Extensions.Encryption.Tests
{
    using Services;

    public class DefaultPlaintextAlgorithmConverterTests
    {
        [Fact]
        public void DefaultPlaintextAlgorithmConverterTest()
        {
            var rawPlaintextString = nameof(DefaultPlaintextAlgorithmConverterTests);
            
            var converter = TestServiceProvider.Current.GetRequiredService<IPlaintextAlgorithmConverter>();

            var buffer = converter.ToResult(rawPlaintextString);
            var plaintextString = converter.ToSource(buffer);

            Assert.Equal(rawPlaintextString, plaintextString);
        }

    }
}
