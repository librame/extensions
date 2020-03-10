using Xunit;

namespace Librame.Extensions.Core.Tests
{
    using Identifiers;

    public class SnowflakeIdentifierGeneratorTests
    {
        [Fact]
        public void AllTest()
        {
            var clock = new TestClockService(new TestMemoryLocker(), new TestLoggerFactory());
            var generator = SnowflakeIdentifierGenerator.Default;

            var current = generator.Generate(clock);
            for (var i = 0; i < 100; i++)
            {
                var next = generator.Generate(clock);
                Assert.NotEqual(current, next);
                current = next;
            }
        }

    }
}
