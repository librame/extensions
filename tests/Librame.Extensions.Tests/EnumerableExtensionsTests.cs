using System;
using System.Collections.Generic;
using Xunit;

namespace Librame.Extensions.Tests
{
    public class EnumerableExtensionsTests
    {
        [Fact]
        public void YieldEnumerableTest()
        {
            var enumerable = 10.YieldEnumerable();
            Assert.NotEmpty(enumerable);
        }

        [Fact]
        public void AsReadOnlyListTest()
        {
            var list = new List<int>
            {
                1,2,3
            }
            .AsReadOnlyList();

            Assert.True(list is IReadOnlyList<int>);
        }
    }
}
