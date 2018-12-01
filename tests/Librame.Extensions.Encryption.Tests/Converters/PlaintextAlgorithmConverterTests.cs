using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Librame.Extensions.Encryption.Tests
{
    using Converters;

    public class PlaintextAlgorithmConverterTests
    {
        [Fact]
        public void DefaultPlaintextAlgorithmConverterTest()
        {
            var rawPlaintextString = nameof(PlaintextAlgorithmConverterTests);
            
            var converter = TestServiceProvider.Current.GetRequiredService<IPlaintextAlgorithmConverter>();

            var buffer = converter.ToResult(rawPlaintextString);
            var plaintextString = converter.ToSource(buffer);

            Assert.Equal(rawPlaintextString, plaintextString);
        }

    }
}
