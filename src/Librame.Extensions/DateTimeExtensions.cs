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

namespace Librame.Extensions
{
    /// <summary>
    /// 日期时间静态扩展。
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// 转换为文件名（除同一日期与时间对象外，其余调用可确保唯一）。
        /// </summary>
        /// <param name="dateTime">给定的日期时间。</param>
        /// <param name="extension">给定以“.”开始的扩展名。</param>
        /// <param name="containsDate">包含日期部分（可选；默认包含）。</param>
        /// <param name="connector">给定的连接符（可选；默认为“_”）。</param>
        /// <returns>返回字符串。</returns>
        public static string AsFileName(this DateTimeOffset dateTime, string extension,
            bool containsDate = true, string connector = "_")
        {
            var prefix = dateTime.ToString("HHssmm");
            if (containsDate)
                prefix = $"{dateTime.ToString("yyyyMMdd")}{connector}{prefix}";

            return ToFileName(dateTime.ToFileTime(), prefix, connector, extension);
        }

        /// <summary>
        /// 转换为文件名（除同一日期与时间对象外，其余调用可确保唯一）。
        /// </summary>
        /// <param name="dateTime">给定的日期时间。</param>
        /// <param name="extension">给定以“.”开始的扩展名。</param>
        /// <param name="containsDate">包含日期部分（可选；默认包含）。</param>
        /// <param name="connector">给定的连接符（可选；默认为“_”）。</param>
        /// <returns>返回字符串。</returns>
        public static string AsFileName(this DateTime dateTime, string extension,
            bool containsDate = true, string connector = "_")
        {
            var prefix = dateTime.ToString("HHssmm");
            if (containsDate)
                prefix = $"{dateTime.ToString("yyyyMMdd")}{connector}{prefix}";

            return ToFileName(dateTime.ToFileTime(), prefix, connector, extension);
        }

        private static string ToFileName(long fileTime, string prefix, string connector, string extension)
        {
            // 100 纳秒级唯一（连续两次通过 DateTimeOffset.Now 调用可保后五位整数不同）
            var suffix = fileTime.ToString();
            suffix = suffix.Substring(suffix.Length - 7);

            var random = new Random((int)fileTime);
            var id = AlgorithmExtensions.UPPER[random.Next(AlgorithmExtensions.UPPER.Length)];

            // 20190822_192042_3801723A.txt
            return $"{prefix}{connector}{suffix}{id}{extension}";
        }


        #region DateOfYear

        /// <summary>
        /// 转换为当年周数。
        /// </summary>
        /// <param name="dateTime">给定的日期时间。</param>
        /// <returns>返回整数。</returns>
        public static int AsWeekOfYear(this DateTime dateTime)
        {
            return ComputeWeekOfYear(dateTime.Year, dateTime.DayOfYear);
        }

        /// <summary>
        /// 转换为当年周数。
        /// </summary>
        /// <param name="dateTimeOffset">给定的日期时间。</param>
        /// <returns>返回整数。</returns>
        public static int AsWeekOfYear(this DateTimeOffset dateTimeOffset)
        {
            return ComputeWeekOfYear(dateTimeOffset.Year, dateTimeOffset.DayOfYear);
        }

        private static int ComputeWeekOfYear(int year, int dayOfYear)
        {
            // 得到今年第一天是周几
            var dayOfWeek = DateTimeOffset.Parse(year + "-1-1").DayOfWeek;
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
        {
            return dateTime.Month / 3 + (dateTime.Month % 3 > 0 ? 1 : 0);
        }

        /// <summary>
        /// 转换为当年季度数。
        /// </summary>
        /// <param name="dateTimeOffset">给定的日期时间。</param>
        /// <returns>返回整数。</returns>
        public static int AsQuarterOfYear(this DateTimeOffset dateTimeOffset)
        {
            return dateTimeOffset.Month / 3 + (dateTimeOffset.Month % 3 > 0 ? 1 : 0);
        }

        #endregion

    }
}
