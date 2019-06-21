using Xunit;

namespace Librame.Extensions.Encryption.Tests
{
    public class AlgorithmIdentifierTests
    {
        [Fact]
        public void NewTest()
        {
            var identifier = (string)AlgorithmIdentifier.Empty;
            Assert.NotEmpty(identifier);

            var identifier1 = (string)AlgorithmIdentifier.New();
            Assert.NotEqual(identifier1, identifier);
        }

    }
}
