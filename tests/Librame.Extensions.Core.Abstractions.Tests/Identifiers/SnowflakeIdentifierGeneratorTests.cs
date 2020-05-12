using Xunit;

namespace Librame.Extensions.Core.Tests
{
    using Identifiers;
    using Services;

    public class SnowflakeIdentifierGeneratorTests
    {
        [Fact]
        public async void GenerateAsyncTest()
        {
            var generator = SnowflakeIdentifierGenerator.Default;
            var clock = LocalClockService.Default;

            var current = await generator.GenerateAsync(clock);
            for (var i = 0; i < 100; i++)
            {
                var next = await generator.GenerateAsync(clock);
                Assert.NotEqual(current, next);
                current = next;
            }
        }

    }
}
