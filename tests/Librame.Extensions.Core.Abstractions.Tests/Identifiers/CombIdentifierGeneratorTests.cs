using System;
using System.Collections.Generic;
using Xunit;

namespace Librame.Extensions.Core.Tests
{
    using Identifiers;
    using Services;

    public class CombIdentifierGeneratorTests
    {
        [Fact]
        public async void GenerateAsyncTest()
        {
            var all = new Dictionary<Guid, CombIdentifierGenerator>();
            var clock = LocalClockService.Default;

            // MySQL
            var generator = CombIdentifierGenerator.MySQL;

            var current = await generator.GenerateAsync(clock);
            all.Add(current, generator);

            for (var i = 0; i < 100; i++)
            {
                var next = await generator.GenerateAsync(clock);
                all.Add(next, generator);
            }

            // Oracle
            generator = CombIdentifierGenerator.Oracle;

            current = await generator.GenerateAsync(clock);
            all.Add(current, generator);

            for (var i = 0; i < 100; i++)
            {
                var next = await generator.GenerateAsync(clock);
                all.Add(next, generator);
            }

            // SQL Server
            generator = CombIdentifierGenerator.SQLServer;

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
