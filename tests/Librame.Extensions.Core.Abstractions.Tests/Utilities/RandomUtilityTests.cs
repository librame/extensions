using System;
using Xunit;

namespace Librame.Extensions.Core.Tests
{
    using Utilities;

    public class RandomUtilityTests
    {
        [Fact]
        public void AllTest()
        {
            var buffer = RandomUtility.GenerateNumber(16);
            Assert.NotEmpty(buffer);

            var g = new Guid(buffer);
            Assert.NotEmpty(g.ToString());
        }

    }
}
