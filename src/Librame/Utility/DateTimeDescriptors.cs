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
using System.Runtime.InteropServices;

namespace Librame.Utility
{
    /// <summary>
    /// 倒计时描述符。
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public class CountdownDescriptor
    {
        /// <summary>
        /// 构造一个 <see cref="CountdownDescriptor"/> 实例。
        /// </summary>
        /// <param name="years">给定的年数描述符。</param>
        /// <param name="months">给定的月数描述符。</param>
        /// <param name="weeks">给定的周数描述符。</param>
        /// <param name="days">给定的天数描述符。</param>
        /// <param name="hours">给定的时数描述符。</param>
        /// <param name="minutes">给定的分数描述符。</param>
        /// <param name="seconds">给定的秒数描述符。</param>
        public CountdownDescriptor(string years = " 年 ",
            string months = " 月 ", string weeks = " 周 ", string days = " 日 ",
            string hours = " 时 ", string minutes = " 分 ", string seconds = " 秒")
        {
            Years = years;

            Months = months;
            Weeks = weeks;
            Days = days;

            Hours = hours;
            Minutes = minutes;
            Seconds = seconds;
        }


        /// <summary>
        /// 获取年数描述符。
        /// </summary>
        public string Years { get; }

        /// <summary>
        /// 获取月数描述符。
        /// </summary>
        public string Months { get; }

        /// <summary>
        /// 获取周数描述符。
        /// </summary>
        public string Weeks { get; }

        /// <summary>
        /// 获取天数描述符。
        /// </summary>
        public string Days { get; }


        /// <summary>
        /// 获取时数描述符。
        /// </summary>
        public string Hours { get; }

        /// <summary>
        /// 获取分数描述符。
        /// </summary>
        public string Minutes { get; }

        /// <summary>
        /// 获取秒数描述符。
        /// </summary>
        public string Seconds { get; }
    }


    /// <summary>
    /// 时间间隔描述符。
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public class TimeSpanDescriptor
    {
        /// <summary>
        /// 构造一个 <see cref="TimeSpanDescriptor"/> 实例。
        /// </summary>
        /// <param name="manyYearsAgo">给定的多年前描述符。</param>
        /// <param name="twoYearsAgo">给定的2年前描述符。</param>
        /// <param name="yearAgo">给定的1年前描述符。</param>
        /// <param name="halfYearAgo">给定的半年前描述符。</param>
        /// <param name="quarterAgo">给定的1季度前描述符。</param>
        /// <param name="monthAgo">给定的1月前描述符。</param>
        /// <param name="weekAgo">给定的1周前描述符。</param>
        /// <param name="daysAgo">给定的n天前描述符。</param>
        /// <param name="hoursAgo">给定的n时前描述符。</param>
        /// <param name="minutesAgo">给定的n分前描述符。</param>
        /// <param name="secondsAgo">给定的n秒前描述符。</param>
        public TimeSpanDescriptor(string manyYearsAgo = "年代久远",
            string twoYearsAgo = "2年前", string yearAgo = "1年前",
            string halfYearAgo = "半年前", string quarterAgo = "1季度前",
            string monthAgo = "1月前", string weekAgo = "1周前",
            string daysAgo = "天前", string hoursAgo = "时前",
            string minutesAgo = "分前", string secondsAgo = "秒前")
        {
            ManyYearsAgo = manyYearsAgo;
            TwoYearsAgo = twoYearsAgo;
            YearAgo = yearAgo;
            HalfYearAgo = halfYearAgo;

            QuarterAgo = quarterAgo;
            MonthAgo = monthAgo;
            WeekAgo = weekAgo;

            DaysAgo = daysAgo;
            HoursAgo = hoursAgo;
            MinutesAgo = minutesAgo;
            SecondsAgo = secondsAgo;
        }


        /// <summary>
        /// 获取多年前描述符。
        /// </summary>
        public string ManyYearsAgo { get; }

        /// <summary>
        /// 获取2年前描述符。
        /// </summary>
        public string TwoYearsAgo { get; }

        /// <summary>
        /// 获取1年前描述符。
        /// </summary>
        public string YearAgo { get; }

        /// <summary>
        /// 获取半年前描述符。
        /// </summary>
        public string HalfYearAgo { get; }


        /// <summary>
        /// 获取1季度前描述符。
        /// </summary>
        public string QuarterAgo { get; }

        /// <summary>
        /// 获取1月前描述符。
        /// </summary>
        public string MonthAgo { get; }

        /// <summary>
        /// 获取1周前描述符。
        /// </summary>
        public string WeekAgo { get; }


        /// <summary>
        /// 获取n天前描述符。
        /// </summary>
        public string DaysAgo { get; }

        /// <summary>
        /// 获取n时前描述符。
        /// </summary>
        public string HoursAgo { get; }

        /// <summary>
        /// 获取n分前描述符。
        /// </summary>
        public string MinutesAgo { get; }

        /// <summary>
        /// 获取n秒前描述符。
        /// </summary>
        public string SecondsAgo { get; }
    }
}
