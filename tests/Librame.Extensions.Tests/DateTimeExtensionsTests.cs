using System;
using Xunit;

namespace Librame.Extensions.Tests
{
    public class DateTimeExtensionsTests
    {
        [Fact]
        public void AsFileNameTest()
        {
            var fileName = DateTime.Now.AsFileName(".txt");
            var newFileName = DateTime.Now.AsFileName(".txt");
            Assert.NotEqual(fileName, newFileName);

            fileName = DateTimeOffset.Now.AsFileName(".txt");
            newFileName = DateTimeOffset.Now.AsFileName(".txt");
            Assert.NotEqual(fileName, newFileName);
        }

        [Fact]
        public void AsWeekOfYearTest()
        {
            var weekOfYear = DateTime.Now.AsWeekOfYear();
            Assert.True(weekOfYear > 0 && weekOfYear < 54);
        }

        [Fact]
        public void AsQuarterOfYearTest()
        {
            var quarterOfYear = DateTime.Now.AsQuarterOfYear();
            Assert.True(quarterOfYear > 0 && quarterOfYear < 5);
        }
    }
}
