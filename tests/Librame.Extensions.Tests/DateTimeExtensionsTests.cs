using System;
using Xunit;

namespace Librame.Extensions.Tests
{
    public class DateTimeExtensionsTests
    {
        [Fact]
        public void AsFileNameTest()
        {
            var extension = ".txt";
            var current = DateTime.Now.AsFileName(extension);
            for (var i = 0; i < 100; i++)
            {
                var next = DateTime.Now.AsFileName(extension);
                Assert.NotEqual(current, next);
                current = next;
            }
        }

        [Fact]
        public void AsCombIdentifierTest()
        {
            var current = DateTime.Now.AsCombIdentifier();
            for (var i = 0; i < 100; i++)
            {
                var next = DateTime.Now.AsCombIdentifier();
                Assert.NotEqual(current, next);
                current = next;
            }
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
