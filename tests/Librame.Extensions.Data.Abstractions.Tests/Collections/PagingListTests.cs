using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Librame.Extensions.Data.Tests
{
    public class PagingListTests
    {
        public class TestPaging
        {
            public int Id { get; set; }

            public string Name { get; set; }
        }


        [Fact]
        public void AllTest()
        {
            var list = new List<TestPaging>();

            for (var i = 0; i < 99; i++)
            {
                list.Add(new TestPaging
                {
                    Id = i + 1,
                    Name = nameof(PagingListTests) + (i + 1)
                });
            }

            var paging = list.AsPagingListByIndex(2, 20);
            Assert.Equal(20, paging.Count);
            Assert.Equal(21, paging.First().Id);
        }

    }
}
