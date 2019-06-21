using Xunit;

namespace Librame.Extensions.Core.Tests
{
    public class UniqueIdentifierTests
    {
        [Fact]
        public void AllTest()
        {
            var identifier = (string)UniqueIdentifier.EmptyByGuid;
            Assert.NotEmpty(identifier);

            identifier = UniqueIdentifier.NewByGuid();
            Assert.NotEmpty(identifier);

            identifier = UniqueIdentifier.NewByRng();
            Assert.NotEmpty(identifier);
        }
    }
}
