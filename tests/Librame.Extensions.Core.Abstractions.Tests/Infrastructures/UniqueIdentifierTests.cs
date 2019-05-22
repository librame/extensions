using Xunit;

namespace Librame.Extensions.Core.Tests
{
    public class UniqueIdentifierTests
    {
        [Fact]
        public void AllTest()
        {
            var empty = UniqueIdentifier.EmptyByGuid();
            Assert.NotEmpty(empty.ToString());

            var guid = UniqueIdentifier.NewByGuid();
            Assert.NotEmpty(guid.ToString());

            var rng = UniqueIdentifier.NewByRng();
            Assert.NotEmpty(rng.ToString());
        }
    }
}
