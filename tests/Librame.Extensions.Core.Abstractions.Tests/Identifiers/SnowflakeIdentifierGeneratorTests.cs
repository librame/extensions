using Xunit;

namespace Librame.Extensions.Core.Tests
{
    using Identifiers;

    public class SnowflakeIdentifierGeneratorTests
    {
        [Fact]
        public async void GenerateAsyncTest()
        {
            var clock = new TestClockService(new TestMemoryLocker(), new TestLoggerFactory());
            var generator = SnowflakeIdentifierGenerator.Default;

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
