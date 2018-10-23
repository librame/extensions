using Xunit;

namespace Librame.Extensions.Encryption.Tests
{
    public class UniqueIdentifierTests
    {

        [Fact]
        public void NewTest()
        {
            var identifier = UniqueIdentifier.NewByGuid();
            Assert.NotEmpty(identifier.ToString());
        }

    }
}
