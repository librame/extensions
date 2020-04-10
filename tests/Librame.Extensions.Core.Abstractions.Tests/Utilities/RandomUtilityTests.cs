using System;
using Xunit;

namespace Librame.Extensions.Core.Tests
{
    using Utilities;

    public class RandomUtilityTests
    {
        [Fact]
        public void GenerateByteArrayTest()
        {
            var buffer = RandomUtility.GenerateByteArray(16);
            Assert.NotEmpty(buffer);

            var g = new Guid(buffer);
            Assert.NotEmpty(g.ToString());
        }


        [Fact]
        public void RandomStringsTest()
        {
            var pairs = RandomUtility.GenerateStrings(20);
            Assert.NotEmpty(pairs);

            pairs = RandomUtility.GenerateStrings(20, hasSpecial: true);
            foreach (var p in pairs)
            {
                Assert.True(p.Key.HasAlgorithmSpecial());
            }
        }

    }
}
