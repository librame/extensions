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
        }
    }
}