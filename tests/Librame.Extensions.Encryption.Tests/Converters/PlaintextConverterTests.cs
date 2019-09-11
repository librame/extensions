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

            var buffer = converter.From(rawPlaintextString);
            var plaintextString = converter.To(buffer);

            Assert.Equal(rawPlaintextString, plaintextString);
        }

    }
}
