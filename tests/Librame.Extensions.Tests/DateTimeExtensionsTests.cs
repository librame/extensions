using System;
using System.Linq;
using Xunit;

namespace Librame.Extensions.Tests
{
    public class DateTimeExtensionsTests
    {
        [Fact]
        public void RepeatabilityTest()
        {
            var times = new DateTime[100];
            for (var i = 0; i < 100; i++)
            {
                times[i] = DateTime.Now;
            }

            // Ticks 精度会出现重复
            var compares = times.Select(t => t.Ticks).Distinct().ToArray();
            Assert.NotEqual(times.Length, compares.Length);

            // ToFileTime() 精度会出现重复
            compares = times.Select(t => t.ToFileTime()).Distinct().ToArray();
            Assert.NotEqual(times.Length, compares.Length);

            compares = new long[100];
            for (var i = 0; i < 100; i++)
            {
                compares[i] = DateTime.Now.ToFileTime();
            }
            // ToFileTime() 精度不会出现重复
            Assert.Equal(compares.Length, compares.Distinct().Count());
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
