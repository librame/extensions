using Xunit;

namespace Librame.Extensions.Core.Tests
{
    public class SnowflakeIdentifierTests
    {
        [Fact]
        public void AllTest()
        {
            var snowflake = new SnowflakeIdentifier();

            var current = snowflake.Generate();
            for (var i = 0; i < 100; i++)
            {
                var next = snowflake.Generate();
                Assert.NotEqual(current, next);
                current = next;
            }
        }

    }
}
