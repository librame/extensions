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

            var buffer = converter.To(rawPlaintextString);
            var plaintextString = converter.From(buffer);

            Assert.Equal(rawPlaintextString, plaintextString);
        }

    }
}
