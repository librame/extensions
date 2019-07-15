using Xunit;

namespace Librame.Extensions.Core.Tests
{
    public class UniqueIdentifierTests
    {
        [Fact]
        public void AllTest()
        {
            string str = UniqueIdentifier.Empty;
            Assert.NotEmpty(str);

            var identifier = UniqueIdentifier.New();
            var other = (UniqueIdentifier)identifier.ToString();

            Assert.True(identifier == other);
            Assert.False(identifier != other);
            Assert.True(identifier.Equals(other));
            Assert.Equal(identifier.GetHashCode(), other.GetHashCode());
            Assert.Equal(identifier.ToString(), other.ToString());
        }
    }
}
