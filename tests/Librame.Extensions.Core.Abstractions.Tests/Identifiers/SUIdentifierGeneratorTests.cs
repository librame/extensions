using System;
using System.Collections.Generic;
using Xunit;

namespace Librame.Extensions.Core.Tests
{
    using Identifiers;

    public class SUIdentifierGeneratorTests
    {
        [Fact]
        public async void GenerateAsyncTest()
        {
            var all = new Dictionary<Guid, SUIdentifierGenerator>();
            var clock = new TestClockService(new TestLoggerFactory());

            // MySQL
            var generator = SUIdentifierGenerator.MySQL;

            var current = await generator.GenerateAsync(clock);
            all.Add(current, generator);

            for (var i = 0; i < 100; i++)
            {
                var next = await generator.GenerateAsync(clock);
                all.Add(next, generator);
            }

            // Oracle
            generator = SUIdentifierGenerator.Oracle;

            current = await generator.GenerateAsync(clock);
            all.Add(current, generator);

            for (var i = 0; i < 100; i++)
            {
                var next = await generator.GenerateAsync(clock);
                all.Add(next, generator);
            }

            // SQL Server
            generator = SUIdentifierGenerator.SQLServer;

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
