using Xunit;

namespace Librame.Extensions.Encryption.Tests
{
    public class AlgorithmIdentifierTests
    {

        [Fact]
        public void NewTest()
        {
            var identifier = AlgorithmIdentifier.New();
            Assert.Equal(identifier, AlgorithmIdentifier.Parse(identifier.ToString()));
        }

    }
}
