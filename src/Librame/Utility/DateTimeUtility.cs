#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;
using System.Collections.Generic;
using System.Text;

namespace Librame.Utility
{
    /// <summary>
    /// <see cref="DateTime"/> 实用工具。
    /// </summary>
    public static class DateTimeUtility
    {
        /// <summary>
        /// 计算指定日期的当周起止日期键值对（键表示开始日期，值表示结束日期）。
        /// </summary>
        /// <param name="day">要计算的日期。</param>
        /// <returns>返回 <see cref="KeyValuePair{TKey, TValue}"/>。</returns>
        public static KeyValuePair<DateTime, DateTime> ComputeStartAndFinishDatePairByWeek(this DateTime day)
        {
            int dow = (int)day.DayOfWeek;
            dow = (dow == 0) ? 7 : dow;

            DateTime start = DateTime.Today.AddDays(1 - dow - 7);
            DateTime finish = DateTime.Today.AddDays(-dow);

            return new KeyValuePair<DateTime, DateTime>(start, finish);
        }


        /// <summary>
        /// 通过总毫秒数实现格式化倒计时。
        /// </summary>
        /// <param name="milliseconds">给定的总毫秒数。</param>
        /// <param name="descriptor">给定的 <see cref="CountdownDescriptor"/>。</param>
        /// <returns>返回字符串。</returns>
        public static string FormatCountdownByMilliseconds(this int milliseconds, CountdownDescriptor descriptor)
        {
            return FormatCountdown(new TimeSpan(0, 0, 0, 0, milliseconds), descriptor);
        }
        /// <summary>
        /// 通过总秒数实现格式化倒计时。
        /// </summary>
        /// <param name="seconds">给定的总秒数。</param>
        /// <param name="descriptor">给定的 <see cref="CountdownDescriptor"/>。</param>
        /// <returns>返回字符串。</returns>
        public static string FormatCountdownBySeconds(this int seconds, CountdownDescriptor descriptor)
        {
            return FormatCountdown(new TimeSpan(0, 0, seconds), descriptor);
        }
        /// <summary>
        /// 格式化倒计时。
        /// </summary>
        /// <param name="ts">给定的时间间隔。</param>
        /// <param name="descriptor">给定的 <see cref="CountdownDescriptor"/>。</param>
        /// <returns>返回字符串。</returns>
        public static string FormatCountdown(this TimeSpan ts, CountdownDescriptor descriptor)
        {
            var sb = new StringBuilder();
            
            if (ts.Days > 0)
                sb.Append(IntUtility.AsString(ts.Days) + descriptor.Days);

            if (ts.Hours > 0)
                sb.Append(IntUtility.AsString(ts.Hours) + descriptor.Hours);

            if (ts.Minutes > 0)
                sb.Append(IntUtility.AsString(ts.Minutes) + descriptor.Minutes);

            sb.Append(IntUtility.AsString(ts.Seconds) + descriptor.Seconds);

            return sb.ToString();
        }


        /// <summary>
        /// 格式化指定时间间隔。
        /// </summary>
        /// <param name="day">给定的日期。</param>
        /// <param name="descriptor">给定的 <see cref="TimeSpanDescriptor"/>。</param>
        /// <returns>返回字符串。</returns>
        public static string FormatTimeSpan(this DateTime day, TimeSpanDescriptor descriptor)
        {
            TimeSpan span = DateTime.Now - day;
            if (span.TotalDays > 1080)
                return descriptor.ManyYearsAgo;
            else if (span.TotalDays > 720)
                return descriptor.TwoYearsAgo;
            else if (span.TotalDays > 360)
                return descriptor.YearAgo;
            else if (span.TotalDays > 180)
                return descriptor.HalfYearAgo;
            else if (span.TotalDays > 90)
                return descriptor.QuarterAgo;
            else if (span.TotalDays > 30)
                return descriptor.MonthAgo;
            else if (span.TotalDays > 7)
                return descriptor.WeekAgo;
            else if (span.TotalDays > 1)
                return ((int)Math.Floor(span.TotalDays) + descriptor.DaysAgo);
            else if (span.TotalHours > 1)
                return ((int)Math.Floor(span.TotalHours) + descriptor.HoursAgo);
            else if (span.TotalMinutes > 1)
                return ((int)Math.Floor(span.TotalMinutes) + descriptor.MinutesAgo);
            else if (span.TotalSeconds > 1)
                return ((int)Math.Floor(span.TotalSeconds) + descriptor.SecondsAgo);
            else
                return ((int)Math.Floor(span.TotalMilliseconds)).ToString();
        }

    }
}
