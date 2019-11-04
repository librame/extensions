using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Librame.Extensions.Tests
{
    public class GuidExtensionsTests
    {
        [Fact]
        public void CombIdTest()
        {
            var timestamp = DateTimeOffset.UtcNow;

            var guid = Guid.NewGuid();
            Assert.NotEqual(guid, guid.AsCombGuid(timestamp));

            var guids = 10.GenerateGuids();
            Assert.Equal(10, guids.Count());

            var combids = guids.AsCombGuids(timestamp);
            Assert.False(guids.SequenceEqual(combids));

            var newCombids = 10.GenerateCombIds(timestamp, out IEnumerable<Guid> newGuids);
            Assert.False(newCombids.SequenceEqual(newGuids));
        }

    }
}
