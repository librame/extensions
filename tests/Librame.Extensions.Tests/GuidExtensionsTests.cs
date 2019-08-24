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
            var guid = Guid.NewGuid();
            Assert.NotEqual(guid, guid.AsCombId());

            var guids = 10.GenerateGuids();
            Assert.Equal(10, guids.Count());

            var combids = guids.AsCombIds();
            Assert.False(guids.SequenceEqual(combids));

            var newCombids = 10.GenerateCombIds(out IEnumerable<Guid> newGuids);
            Assert.False(newCombids.SequenceEqual(newGuids));
        }

    }
}
