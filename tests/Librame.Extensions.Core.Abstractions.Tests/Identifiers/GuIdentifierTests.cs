using Xunit;

namespace Librame.Extensions.Core.Tests
{
    public class GuIdentifierTests
    {
        [Fact]
        public void AllTest()
        {
            Assert.NotEmpty((string)GuIdentifier.Empty);

            var identifier = GuIdentifier.New();

            string str = identifier;
            Assert.NotEmpty(str);

            var identifier1 = (GuIdentifier)str;
            Assert.Equal(identifier.Buffer, identifier1.Buffer);
        }
    }
}
