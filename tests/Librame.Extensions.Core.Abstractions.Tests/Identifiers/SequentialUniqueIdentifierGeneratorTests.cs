using System;
using System.Collections.Generic;
using Xunit;

namespace Librame.Extensions.Core.Tests
{
    using Identifiers;

    public class SequentialUniqueIdentifierGeneratorTests
    {
        [Fact]
        public async void GenerateAsyncTest()
        {
            var all = new Dictionary<Guid, SequentialUniqueIdentifierGenerator>();
            var clock = new TestClockService(new TestMemoryLocker(), new TestLoggerFactory());

            // MySQL
            var generator = SequentialUniqueIdentifierGenerator.MySQL;

            var current = await generator.GenerateAsync(clock);
            all.Add(current, generator);

            for (var i = 0; i < 100; i++)
            {
                var next = await generator.GenerateAsync(clock);
                all.Add(next, generator);
            }

            // Oracle
            generator = SequentialUniqueIdentifierGenerator.Oracle;

            current = await generator.GenerateAsync(clock);
            all.Add(current, generator);

            for (var i = 0; i < 100; i++)
            {
                var next = await generator.GenerateAsync(clock);
                all.Add(next, generator);
            }

            // SQL Server
            generator = SequentialUniqueIdentifierGenerator.SqlServer;

            current = await generator.GenerateAsync(clock);
            all.Add(current, generator);

            for (var i = 0; i < 100; i++)
            {
                var next = await generator.GenerateAsync(clock);
                all.Add(next, generator);
            }

            Assert.NotEmpty(all);
        }

    }
}
