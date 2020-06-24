using System;
using Xunit;

namespace Librame.Extensions.Tests
{
    public class GuidExtensionsTests
    {
        [Fact]
        public void AsShortStringTest()
        {
            var g = Guid.NewGuid();
            Assert.NotEmpty(g.AsShortString());
        }


        [Fact]
        public void CombIdTest()
        {
            var g = Guid.NewGuid();
            Assert.NotEqual(g, g.AsCombGuid());
            Assert.NotEqual(g, g.AsCombGuid(DateTimeOffset.UtcNow));
        }

    }
}
