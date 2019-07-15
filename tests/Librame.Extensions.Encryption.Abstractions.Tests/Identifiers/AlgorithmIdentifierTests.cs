using Xunit;

namespace Librame.Extensions.Encryption.Tests
{
    public class AlgorithmIdentifierTests
    {
        [Fact]
        public void AllTest()
        {
            string str = AlgorithmIdentifier.Empty;
            Assert.NotEmpty(str);

            var identifier = AlgorithmIdentifier.New();
            var other = (AlgorithmIdentifier)identifier.ToString();

            Assert.True(identifier == other);
            Assert.False(identifier != other);
            Assert.True(identifier.Equals(other));
            Assert.Equal(identifier.GetHashCode(), other.GetHashCode());
            Assert.Equal(identifier.ToString(), other.ToString());
        }

    }
}
