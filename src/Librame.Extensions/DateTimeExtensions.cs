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
        /// 转换为文件名。
        /// </summary>
        /// <param name="dateTime">给定的日期时间。</param>
        /// <param name="extension">给定以 . 开始的扩展名。</param>
        /// <param name="containsDate">包含日期部分（可选；默认包含）。</param>
        /// <returns>返回字符串。</returns>
        public static string AsFileName(this DateTime dateTime, string extension,
            bool containsDate = true)
        {
            return dateTime.AsCombFileTime(containsDate)
                + extension.NotEmpty(nameof(extension));
        }

        /// <summary>
        /// 转换为文件名。
        /// </summary>
        /// <param name="dateTimeOffset">给定的日期时间。</param>
        /// <param name="extension">给定以 . 开始的扩展名。</param>
        /// <param name="containsDate">包含日期部分（可选；默认包含）。</param>
        /// <returns>返回字符串。</returns>
        public static string AsFileName(this DateTimeOffset dateTimeOffset, string extension,
            bool containsDate = true)
        {
            return dateTimeOffset.AsCombFileTime(containsDate)
                + extension.NotEmpty(nameof(extension));
        }


        /// <summary>
        /// 转换为有顺序的文件时间。
        /// </summary>
        /// <param name="dateTime">给定的 <see cref="DateTime"/>。</param>
        /// <param name="containsDate">包含日期部分（可选；默认包含）。</param>
        /// <returns>返回长度 14/22 的字符串。</returns>
        public static string AsCombFileTime(this DateTime dateTime,
            bool containsDate = true)
        {
            if (containsDate)
            {
                // 长度 22
                return CombineFileTime(dateTime.ToString("yyyyMMddHHmmssfff", CultureInfo.InvariantCulture),
                    dateTime.ToFileTime().ToString(CultureInfo.InvariantCulture));
            }

            // 长度 14
            return CombineFileTime(dateTime.ToString("HHmmssfff", CultureInfo.InvariantCulture),
                dateTime.ToFileTime().ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// 转换为有顺序的文件时间。
        /// </summary>
        /// <param name="dateTimeOffset">给定的 <see cref="DateTimeOffset"/>。</param>
        /// <param name="containsDate">包含日期部分（可选；默认包含）。</param>
        /// <returns>返回长度 14/22 的字符串。</returns>
        public static string AsCombFileTime(this DateTimeOffset dateTimeOffset,
            bool containsDate = true)
        {
            if (containsDate)
            {
                // 长度 22
                return CombineFileTime(dateTimeOffset.ToString("yyyyMMddHHmmssfff", CultureInfo.InvariantCulture),
                    dateTimeOffset.ToFileTime().ToString(CultureInfo.InvariantCulture));
            }

            // 长度 14
            return CombineFileTime(dateTimeOffset.ToString("HHmmssfff", CultureInfo.InvariantCulture),
                dateTimeOffset.ToFileTime().ToString(CultureInfo.InvariantCulture));
        }

        private static string CombineFileTime(string dateTime, string fileTime)
        {
            // 截取后 5 位为时间戳
            return dateTime + fileTime.Substring(fileTime.Length - 5);
        }


        #region DateOfYear

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
