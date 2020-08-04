#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;
using System.Globalization;

namespace Librame.Extensions
{
    /// <summary>
    /// 日期时间静态扩展。
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// 是日期与时间类型。
        /// </summary>
        /// <param name="type">给定的类型。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsDateTimeType(this Type type)
            => ExtensionSettings.DateTimeType.Equals(type);

        /// <summary>
        /// 是日期与时间偏移类型。
        /// </summary>
        /// <param name="type">给定的类型。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsDateTimeOffsetType(this Type type)
            => ExtensionSettings.DateTimeOffsetType.Equals(type);

        /// <summary>
        /// 是日期与时间或偏移类型。
        /// </summary>
        /// <param name="type">给定的类型。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsDateTimeOrOffsetType(this Type type)
            => type.IsDateTimeType() || type.IsDateTimeOffsetType();


        /// <summary>
        /// 转换为日期与时间偏移。
        /// </summary>
        /// <param name="dateTime">给定的 <see cref="DateTime"/>。</param>
        /// <returns>返回 <see cref="DateTimeOffset"/>。</returns>
        public static DateTimeOffset ToDateTimeOffset(this DateTime dateTime)
            => new DateTimeOffset(dateTime);


        /// <summary>
        /// 转换为相对于 Unix 时间等于 0 的时间点的周期数（可用于转换为 JavaScript 时间）。
        /// </summary>
        /// <param name="dateTime">给定的 <see cref="DateTime"/>。</param>
        /// <returns>返回长整数。</returns>
        public static long ToUnixTicks(this DateTime dateTime)
        {
            var unixEpoch = ExtensionSettings.Preference.UnixEpoch;

            return (long)TimeSpan.FromTicks(dateTime.Ticks - unixEpoch.Ticks)
                .TotalMilliseconds - GetUtcOffset();

            // GetUtcOffset
            int GetUtcOffset()
                => dateTime.ToDateTimeOffset().Offset.Hours * 60 * 60 * 1000;
        }

        /// <summary>
        /// 转换为相对于 Unix 时间等于 0 的时间点的周期数（可用于转换为 JavaScript 时间）。
        /// </summary>
        /// <param name="dateTimeOffset">给定的 <see cref="DateTimeOffset"/>。</param>
        /// <returns>返回长整数。</returns>
        public static long ToUnixTicks(this DateTimeOffset dateTimeOffset)
        {
            var unixEpochOffset = ExtensionSettings.Preference.UnixEpochOffset;

            return (long)TimeSpan.FromTicks(dateTimeOffset.Ticks - unixEpochOffset.Ticks)
                .TotalMilliseconds - GetUtcOffset();

            // GetUtcOffset
            int GetUtcOffset()
                => dateTimeOffset.Offset.Hours * 60 * 60 * 1000;
        }


        #region ToChineseCalendar

        /// <summary>
        /// 还原中国农历（阴阳合历）时间。
        /// </summary>
        /// <param name="chineseTime">给定的中国农历（阴阳合历）时间。</param>
        /// <returns>返回 <see cref="DateTime"/>。</returns>
        public static DateTime FromChineseCalendarTime(this DateTime chineseTime)
            => chineseTime.FromChineseCalendarTime(out _);

        /// <summary>
        /// 还原中国农历（阴阳合历）时间。
        /// </summary>
        /// <param name="chineseTime">给定的中国农历（阴阳合历）时间。</param>
        /// <param name="isLeapMonth">输出是否闰月。</param>
        /// <returns>返回 <see cref="DateTime"/>。</returns>
        public static DateTime FromChineseCalendarTime(this DateTime chineseTime,
            out bool isLeapMonth)
        {
            var calendar = ExtensionSettings.Preference.ChineseCalendar;

            var chineseMonth = chineseTime.Month;
            var leapMonth = calendar.GetLeapMonth(chineseTime.Year);

            isLeapMonth = false;
            if (leapMonth > 0)
            {
                if (leapMonth == chineseMonth)
                {
                    isLeapMonth = true;
                    chineseMonth++;
                }
                else if (chineseMonth > leapMonth)
                {
                    chineseMonth++;
                }
            }

            return new DateTime(chineseTime.Year, chineseMonth, chineseTime.Day,
                chineseTime.Hour, chineseTime.Minute, chineseTime.Second, chineseTime.Millisecond,
                calendar, DateTimeKind.Local);
        }


        /// <summary>
        /// 还原中国农历（阴阳合历）时间。
        /// </summary>
        /// <param name="chineseTimeOffset">给定的中国农历（阴阳合历）时间。</param>
        /// <returns>返回 <see cref="DateTimeOffset"/>。</returns>
        public static DateTimeOffset FromChineseCalendarTime(this DateTimeOffset chineseTimeOffset)
            => chineseTimeOffset.FromChineseCalendarTime(out _);

        /// <summary>
        /// 还原中国农历（阴阳合历）时间。
        /// </summary>
        /// <param name="chineseTimeOffset">给定的中国农历（阴阳合历）时间。</param>
        /// <param name="isLeapMonth">输出是否闰月。</param>
        /// <returns>返回 <see cref="DateTimeOffset"/>。</returns>
        public static DateTimeOffset FromChineseCalendarTime(this DateTimeOffset chineseTimeOffset,
            out bool isLeapMonth)
            => new DateTimeOffset(chineseTimeOffset.DateTime.FromChineseCalendarTime(out isLeapMonth),
                chineseTimeOffset.Offset);


        /// <summary>
        /// 转为中国农历（阴阳合历）时间。
        /// </summary>
        /// <param name="dateTime">给定的 <see cref="DateTime"/>。</param>
        /// <param name="toLocalTimeFuncIfNot">给定如果当前日期时间不是本地日期时间的转换方法（可选）。</param>
        /// <returns>返回 <see cref="DateTime"/>。</returns>
        public static DateTime ToChineseCalendarTime(this DateTime dateTime,
            Func<DateTime, DateTime> toLocalTimeFuncIfNot = null)
            => dateTime.ToChineseCalendarTime(out _, toLocalTimeFuncIfNot);

        /// <summary>
        /// 转为中国农历（阴阳合历）时间。
        /// </summary>
        /// <param name="dateTime">给定的 <see cref="DateTime"/>。</param>
        /// <param name="isLeapMonth">输出是否闰月。</param>
        /// <param name="toLocalTimeFuncIfNot">给定如果当前日期时间不是本地日期时间的转换方法（可选）。</param>
        /// <returns>返回 <see cref="DateTime"/>。</returns>
        public static DateTime ToChineseCalendarTime(this DateTime dateTime,
            out bool isLeapMonth, Func<DateTime, DateTime> toLocalTimeFuncIfNot = null)
        {
            var calendar = ExtensionSettings.Preference.ChineseCalendar;

            var chineseYear = calendar.GetYear(dateTime);
            var chineseMonth = calendar.GetMonth(dateTime);
            var chineseDay = calendar.GetDayOfMonth(dateTime);

            var leapMonth = calendar.GetLeapMonth(chineseYear);

            isLeapMonth = false;
            if (leapMonth > 0)
            {
                if (leapMonth == chineseMonth)
                {
                    isLeapMonth = true;
                    chineseMonth--;
                }
                else if (chineseMonth > leapMonth)
                {
                    chineseMonth--;
                }
            }

            if (dateTime.Kind != DateTimeKind.Local)
            {
                if (toLocalTimeFuncIfNot.IsNull())
                    toLocalTimeFuncIfNot = dt => dt.ToDateTimeOffset().ToLocalTime().DateTime;

                dateTime = toLocalTimeFuncIfNot.Invoke(dateTime);
            }
            
            return new DateTime(chineseYear, chineseMonth, chineseDay,
                calendar.GetHour(dateTime),
                calendar.GetMinute(dateTime),
                calendar.GetSecond(dateTime),
                (int)calendar.GetMilliseconds(dateTime),
                DateTimeKind.Local);
        }


        /// <summary>
        /// 转换为中国农历（阴阳合历）时间偏移。
        /// </summary>
        /// <param name="dateTimeOffset">给定的 <see cref="DateTimeOffset"/>。</param>
        /// <returns>返回 <see cref="DateTimeOffset"/>。</returns>
        public static DateTimeOffset ToChineseCalendarTime(this DateTimeOffset dateTimeOffset)
            => dateTimeOffset.ToChineseCalendarTime(out _);

        /// <summary>
        /// 转换为中国农历（阴阳合历）时间偏移。
        /// </summary>
        /// <param name="dateTimeOffset">给定的 <see cref="DateTimeOffset"/>。</param>
        /// <param name="isLeapMonth">输出是否闰月。</param>
        /// <returns>返回 <see cref="DateTimeOffset"/>。</returns>
        public static DateTimeOffset ToChineseCalendarTime(this DateTimeOffset dateTimeOffset,
            out bool isLeapMonth)
            => dateTimeOffset.DateTime.ToChineseCalendarTime(out isLeapMonth,
                dt => dateTimeOffset.ToLocalTime().DateTime);


        /// <summary>
        /// 转换为中国农历（阴阳合历）。
        /// </summary>
        /// <param name="dateTime">给定的 <see cref="DateTime"/>。</param>
        /// <param name="formatter">给定的格式化方式（输入参数依次为天干地支、生肖、月、日）。</param>
        /// <returns>返回 <see cref="DateTime"/>。</returns>
        public static string ToChineseCalendarString(this DateTime dateTime,
            Func<(string sexagenCycle, string animalSign, string month, string day), string> formatter = null)
        {
            var chineseTime = dateTime.ToChineseCalendarTime(out var isLeapMonth);

            if (formatter.IsNull())
                formatter = f => $"{f.sexagenCycle}年【{f.animalSign}年】 {f.month}月{f.day}";

            (var sexagenCycle, var animalSign) = GetLunisolarYear(chineseTime.Year);

            return formatter.Invoke((sexagenCycle, animalSign,
                GetLunisolarMonth(chineseTime.Month), GetLunisolarDay(chineseTime.Day)));

            // GetLunisolarYear
            (string sexagenCycle, string animalSign) GetLunisolarYear(int year)
            {
                //var calendar = ExtensionSettings.Preference.ChineseCalendar;

                //var sexagenaryYear = calendar.GetSexagenaryYear(dateTime);
                //var celestialStem = calendar.GetCelestialStem(sexagenaryYear);
                //var terrestrialBranch = calendar.GetTerrestrialBranch(sexagenaryYear);

                int csIndex = (year - 4) % 10;
                int tbIndex = (year - 4) % 12;

                return (ExtensionSettings.Preference.TenCelestialStems[csIndex]
                    + ExtensionSettings.Preference.TwelveTerrestrialBranches[tbIndex],
                    ExtensionSettings.Preference.TwelveAnimalSigns[tbIndex]);
            }

            // GetLunisolarMonth
            string GetLunisolarMonth(int month)
                => ExtensionSettings.Preference.ChineseMonths[month - 1];

            // GetLunisolarDay
            string GetLunisolarDay(int day)
            {
                var days = ExtensionSettings.Preference.ChineseDays;
                var tenDays = ExtensionSettings.Preference.ChineseTenDays;

                if (day != 20 && day != 30)
                    return string.Concat(tenDays[(day - 1) / 10], days[(day - 1) % 10]);

                return string.Concat(days[(day - 1) / 10], tenDays[1]);
            }
        }

        /// <summary>
        /// 转换为中国农历（阴阳合历）。
        /// </summary>
        /// <param name="dateTimeOffset">给定的 <see cref="DateTimeOffset"/>。</param>
        /// <returns>返回 <see cref="DateTimeOffset"/>。</returns>
        public static string ToChineseCalendarString(this DateTimeOffset dateTimeOffset)
            => dateTimeOffset.LocalDateTime.ToChineseCalendarString();

        #endregion


        #region DateOfYear and QuarterOfYear

        /// <summary>
        /// 转换为当年周数。
        /// </summary>
        /// <param name="dateTime">给定的日期时间。</param>
        /// <returns>返回整数。</returns>
        public static int AsWeekOfYear(this DateTime dateTime)
            => ComputeWeekOfYear(dateTime.Year, dateTime.DayOfYear);

        /// <summary>
        /// 转换为当年周数。
        /// </summary>
        /// <param name="dateTimeOffset">给定的日期时间。</param>
        /// <returns>返回整数。</returns>
        public static int AsWeekOfYear(this DateTimeOffset dateTimeOffset)
            => ComputeWeekOfYear(dateTimeOffset.Year, dateTimeOffset.DayOfYear);

        private static int ComputeWeekOfYear(int year, int dayOfYear)
        {
            // 得到今年第一天是周几
            var dayOfWeek = DateTimeOffset.Parse(year + "-1-1", CultureInfo.CurrentCulture).DayOfWeek;
            var firstWeekend = (int)dayOfWeek;

            // 计算第一周的差额（如果是周日，则 firstWeekend 为 0，第一周也就是从周日开始）
            var weekDay = firstWeekend == 0 ? 1 : (7 - firstWeekend + 1);

            //（今天是一年当中的第几天 减去 第一周周末）/7 等于 距第一周有多少周 再加上第一周的1 就是今天是今年的第几周了
            // 刚好考虑了惟一的特殊情况就是，今天刚好在第一周内，那么距第一周就是0 再加上第一周的1 最后还是1
            return Convert.ToInt32(Math.Ceiling((dayOfYear - weekDay) / 7.0)) + 1;
        }


        /// <summary>
        /// 转换为当年季度数。
        /// </summary>
        /// <param name="dateTime">给定的日期时间。</param>
        /// <returns>返回整数。</returns>
        public static int AsQuarterOfYear(this DateTime dateTime)
            => dateTime.Month / 3 + (dateTime.Month % 3 > 0 ? 1 : 0);

        /// <summary>
        /// 转换为当年季度数。
        /// </summary>
        /// <param name="dateTimeOffset">给定的日期时间。</param>
        /// <returns>返回整数。</returns>
        public static int AsQuarterOfYear(this DateTimeOffset dateTimeOffset)
            => dateTimeOffset.Month / 3 + (dateTimeOffset.Month % 3 > 0 ? 1 : 0);

        #endregion

    }
}
