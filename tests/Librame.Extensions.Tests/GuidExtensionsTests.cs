using System;
using Xunit;

namespace Librame.Extensions.Tests
{
    public class GuidExtensionsTests
    {
        [Fact]
        public void CombIdTest()
        {
            var guid = Guid.NewGuid();
            Assert.NotEqual(guid, guid.AsCombGuid());
            Assert.NotEqual(guid, guid.AsCombGuid(DateTimeOffset.UtcNow));
        }

    }
}
