using Xunit;

namespace Librame.Extensions.Core.Tests
{
    public class RandomNumberAlgorithmIdentifierTests
    {
        [Fact]
        public void AllTest()
        {
            var identifier = RandomNumberAlgorithmIdentifier.New(32);
            Assert.NotEmpty((string)identifier);

            var other = new RandomNumberAlgorithmIdentifier(identifier, identifier.Converter);
            Assert.True(identifier == other);
            Assert.False(identifier != other);
            Assert.True(identifier.Equals(other));
            Assert.Equal(identifier.GetHashCode(), other.GetHashCode());
            Assert.Equal(identifier.ToString(), other.ToString());

            // Serialization
            var buffer = identifier.SerializeBinary().RtlCompress();
            var base64String = buffer.AsBase64String();
            Assert.NotEmpty(base64String);

            buffer = base64String.FromBase64String().RtlDecompress();
            var obj = buffer.DeserializeBinary();
            Assert.True(identifier.Equals(obj));
        }
    }
}
