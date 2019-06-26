using Xunit;

namespace Librame.Extensions.Core.Tests
{
    public class RngIdentifierTests
    {
        [Fact]
        public void AllTest()
        {
            var identifier = RngIdentifier.New();

            string str = identifier;
            Assert.NotEmpty(str);

            var identifier1 = (RngIdentifier)str;
            Assert.Equal(identifier.Buffer, identifier1.Buffer);
        }
    }
}
