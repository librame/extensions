using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Librame.Extensions.Tests
{
    public class EnumerableExtensionsTests
    {
        [Fact]
        public void YieldEnumerableTest()
        {
            var enumerable = 10.YieldEnumerable();

            foreach (var item in enumerable)
                Assert.Equal(10, item);
        }

        [Fact]
        public async Task YieldAsyncEnumerableTest()
        {
            var enumerable = Task.FromResult(10).YieldAsyncEnumerable();
            
            await foreach (var item in enumerable)
                Assert.Equal(10, item);
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


        [Fact]
        public async Task ForEachAsyncTest()
        {
            var list = new List<int>
            {
                1,2,3,4,5,6,7,8,9
            };
            var asyncList = list.Select(i => Task.FromResult(i)).AsAsyncEnumerable();
            
            var sum = 0;
            await asyncList.ForEachAsync((n, i) => sum += n, (n, i) => i == 5); // 索引等于5则跳出
            Assert.Equal(21, sum); // 1+...7
        }


        [Fact]
        public void ForEachTest()
        {
            var list = new List<int>
            {
                1,2,3,4,5,6,7,8,9
            };

            var sum = 0;
            list.ForEach((n, i) => sum += n, (n, i) => i == 5); // 索引等于5则跳出
            Assert.Equal(21, sum); // 1+...7
        }


        [Fact]
        public void TrimTest()
        {
            var list = new List<int>
            {
                0,0,1,2,3,0,4,5,0,6,7,0,8,1,9,9,9
            };

            var trimStart = list.TrimStart(0);
            var countStart = trimStart.Count();
            Assert.Equal(list.Count - 2, countStart); // 前2

            var trimEnd = trimStart.TrimEnd(9);
            var countEnd = trimEnd.Count();
            Assert.Equal(countStart - 3, countEnd); // 后3

            var trim = trimEnd.Trim(1);
            Assert.Equal(countEnd - 2, trim.Count()); // 前后各1
        }
    }
}
