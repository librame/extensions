using System;
using Xunit;

namespace Librame.Extensions.Tests
{
    public class DateTimeExtensionsTests
    {

        [Fact]
        public void AsWeekOfYearTest()
        {
            var weekOfYear = DateTime.Now.AsWeekOfYear();

            Assert.True(weekOfYear > 0 && weekOfYear < 54);
        }

    }
}
