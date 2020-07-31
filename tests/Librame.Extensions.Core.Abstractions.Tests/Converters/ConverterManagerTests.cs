using System.Linq;
using Xunit;

namespace Librame.Extensions.Core.Tests
{
    using Converters;

    public class ConverterManagerTests
    {
        [Fact]
        public void AllTest()
        {
            var buffer = nameof(ConverterManagerTests).FromEncodingString();

            IAlgorithmConverter converter = ConverterManager.GetAlgorithm<Base32StringConverter>();
            var algorithm = converter.ConvertTo(buffer);
            Assert.True(buffer.SequenceEqual(converter.ConvertFrom(algorithm)));

            converter = ConverterManager.GetAlgorithm<Base64StringConverter>();
            algorithm = converter.ConvertTo(buffer);
            Assert.True(buffer.SequenceEqual(converter.ConvertFrom(algorithm)));

            converter = ConverterManager.GetAlgorithm<HexStringConverter>();
            algorithm = converter.ConvertTo(buffer);
            Assert.True(buffer.SequenceEqual(converter.ConvertFrom(algorithm)));
        }

    }
}
