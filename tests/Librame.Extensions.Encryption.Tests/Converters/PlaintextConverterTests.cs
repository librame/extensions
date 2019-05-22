using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Librame.Extensions.Encryption.Tests
{
    public class PlaintextConverterTests
    {
        [Fact]
        public void DefaultPlaintextAlgorithmConverterTest()
        {
            var rawPlaintextString = nameof(PlaintextConverterTests);
            
            var converter = TestServiceProvider.Current.GetRequiredService<IPlaintextConverter>();

            var buffer = converter.ToResult(rawPlaintextString);
            var plaintextString = converter.ToSource(buffer);

            Assert.Equal(rawPlaintextString, plaintextString);
        }

    }
}
