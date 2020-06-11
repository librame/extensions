using System.Collections.Generic;
using Xunit;

namespace Librame.Extensions.Data.Tests
{
    using Collections;

    public class TreeingTests
    {
        [Fact]
        public void AllTest()
        {
            var list = new List<TestTreeing>();

            for (var i = 0; i < 99; i++)
            {
                var test = new TestTreeing
                {
                    Id = i + 1,
                    Name = nameof(TreeingTests) + (i + 1)
                };

                if (i < 10) test.ParentId = 0;
                if (i >= 10 && i < 50) test.ParentId = 3;
                if (i >= 50 && i < 99) test.ParentId = 7;

                list.Add(test);
            }

            var treeing = list.AsTreeing<TestTreeing, int>();
            Assert.Equal(10, treeing.Count); // ParentId=0 Count
        }

    }
}
