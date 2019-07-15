using Xunit;

namespace Librame.Extensions.Core.Tests
{
    public class RandomNumberIdentifierTests
    {
        [Fact]
        public void AllTest()
        {
            var identifier = RandomNumberIdentifier.New();
            var other = (RandomNumberIdentifier)identifier.ToString();

            Assert.True(identifier == other);
            Assert.False(identifier != other);
            Assert.True(identifier.Equals(other));
            Assert.Equal(identifier.GetHashCode(), other.GetHashCode());
            Assert.Equal(identifier.ToString(), other.ToString());
        }
    }
}
