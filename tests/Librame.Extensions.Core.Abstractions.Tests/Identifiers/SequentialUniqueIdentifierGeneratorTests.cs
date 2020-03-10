using System;
using System.Collections.Generic;
using Xunit;

namespace Librame.Extensions.Core.Tests
{
    using Identifiers;

    public class SequentialUniqueIdentifierGeneratorTests
    {
        [Fact]
        public void AllTest()
        {
            var all = new Dictionary<Guid, SequentialUniqueIdentifierGenerator>();
            var clock = new TestClockService(new TestMemoryLocker(), new TestLoggerFactory());

            // MySQL
            var generator = SequentialUniqueIdentifierGenerator.MySQL;

            var current = generator.Generate(clock);
            all.Add(current, generator);

            for (var i = 0; i < 100; i++)
            {
                var next = generator.Generate(clock);
                all.Add(next, generator);
            }

            // Oracle
            generator = SequentialUniqueIdentifierGenerator.Oracle;

            current = generator.Generate(clock);
            all.Add(current, generator);

            for (var i = 0; i < 100; i++)
            {
                var next = generator.Generate(clock);
                all.Add(next, generator);
            }

            // SQL Server
            generator = SequentialUniqueIdentifierGenerator.SqlServer;

            current = generator.Generate(clock);
            all.Add(current, generator);

            for (var i = 0; i < 100; i++)
            {
                var next = generator.Generate(clock);
                all.Add(next, generator);
            }

            Assert.NotEmpty(all);
        }

    }
}
