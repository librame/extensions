using Xunit;

namespace Librame.Extensions.Core.Tests
{
    public class SnowflakeIdentifierGeneratorTests
    {
        [Fact]
        public void AllTest()
        {
            var generator = SnowflakeIdentifierGenerator.Default;

            var current = generator.Generate();
            for (var i = 0; i < 100; i++)
            {
                var next = generator.Generate();
                Assert.NotEqual(current, next);
                current = next;
            }
        }

    }
}
