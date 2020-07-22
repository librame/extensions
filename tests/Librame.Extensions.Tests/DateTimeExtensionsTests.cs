using System;
using Xunit;

namespace Librame.Extensions.Tests
{
    public class DateTimeExtensionsTests
    {
        [Fact]
        public void ToChineseCalendarTest()
        {
            var dateTime = DateTime.Now;
            var dateTimeOffset = dateTime.ToDateTimeOffset();

            var calendarTime = dateTime.ToChineseCalendarTime();
            var calendarOffsetTime = dateTimeOffset.ToChineseCalendarTime();
            Assert.Equal(calendarTime.ToString(), calendarOffsetTime.DateTime.ToString());

            var fromCalendarTime = calendarTime.FromChineseCalendarTime();
            var fromCalendarOffsetTime = calendarOffsetTime.FromChineseCalendarTime();
            Assert.Equal(dateTime.ToString(), fromCalendarTime.ToString());
            Assert.Equal(dateTimeOffset.ToString(), fromCalendarOffsetTime.ToString());

            var calendarString = dateTime.ToChineseCalendarString();
            var calendarOffsetString = dateTimeOffset.ToChineseCalendarString();
            Assert.Equal(calendarString, calendarOffsetString);
        }


        [Fact]
        public void ToUnixTicksTest()
        {
            var dateTime = DateTime.Now;
            var dateTimeOffset = dateTime.ToDateTimeOffset();

            var unixTicks = dateTime.ToUnixTicks();
            var unixTicksOffset = dateTimeOffset.ToUnixTicks();
            Assert.Equal(unixTicks, unixTicksOffset);
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
