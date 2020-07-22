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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Librame.Extensions
{
    /// <summary>
    /// 兼容性静态扩展。
    /// </summary>
    public static class CompatibilityExtensions
    {

#if NET48

        /// <summary>
        /// 清空并行集合。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="bag">给定的 <see cref="ConcurrentBag{T}"/>。</param>
        public static void Clear<T>(this ConcurrentBag<T> bag)
        {
            if (bag.IsNull() || bag.IsEmpty)
                return;

            for (var i = 0; i < bag.Count; i++)
                bag.TryTake(out _);
        }

#endif


        /// <summary>
        /// 包含。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <param name="value">给定的字符。</param>
        /// <returns>返回布尔值。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static bool CompatibleContains(this string str, char value)
        {
            str.NotNull(nameof(str));

#if !NET48
            return str.Contains(value, StringComparison.InvariantCulture);
#else
            return str.Contains(value);
#endif
        }

        /// <summary>
        /// 包含。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <param name="value">给定的字符。</param>
        /// <returns>返回布尔值。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static bool CompatibleContains(this string str, string value)
        {
            str.NotNull(nameof(str));

#if !NET48
            return str.Contains(value, StringComparison.InvariantCulture);
#else
            return str.Contains(value);
#endif
        }


        /// <summary>
        /// 获取哈希码。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <returns>返回整数。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static int CompatibleGetHashCode(this string str)
        {
            str.NotNull(nameof(str));

#if !NET48
            return str.GetHashCode(StringComparison.Ordinal);
#else
            return str.GetHashCode();
#endif
        }


        /// <summary>
        /// 报告字符串中第一个匹配字符从零开始的索引。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <param name="value">给定的字符。</param>
        /// <returns>返回布尔值。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static int CompatibleIndexOf(this string str, char value)
        {
            str.NotNull(nameof(str));

#if !NET48
            return str.IndexOf(value, StringComparison.InvariantCulture);
#else
            return str.IndexOf(value);
#endif
        }

        ///// <summary>
        ///// 报告字符串中最后一个匹配字符从零开始的索引。
        ///// </summary>
        ///// <param name="str">给定的字符串。</param>
        ///// <param name="value">给定的字符。</param>
        ///// <returns>返回布尔值。</returns>
        //[SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        //public static int CompatibleLastIndexOf(this string str, char value)
        //{
        //    str.NotNull(nameof(str));

        //    #if !NET48
        //        return str.LastIndexOf(value);
        //    #else
        //        return str.LastIndexOf(value);
        //    #endif
        //}


        /// <summary>
        /// 确定字符串以指定字符开始。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <param name="value">给定的字符。</param>
        /// <returns>返回布尔值。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static bool CompatibleStartsWith(this string str, char value)
        {
            str.NotNull(nameof(str));

#if !NET48
            return str.StartsWith(value);
#else
            return str.StartsWith(value.ToString(CultureInfo.InvariantCulture),
                StringComparison.InvariantCulture);
#endif
        }

        ///// <summary>
        ///// 确定字符串以指定字符开始。
        ///// </summary>
        ///// <param name="str">给定的字符串。</param>
        ///// <param name="value">给定的字符。</param>
        ///// <returns>返回布尔值。</returns>
        //[SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        //public static bool CompatibleStartsWith(this string str, string value)
        //{
        //    #if !NET48
        //        return str.StartsWith(value, StringComparison.InvariantCulture);
        //    #else
        //        return str.StartsWith(value, StringComparison.InvariantCulture);
        //    #endif
        //}

        /// <summary>
        /// 确定字符串以指定字符结尾。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <param name="value">给定的字符。</param>
        /// <returns>返回布尔值。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static bool CompatibleEndsWith(this string str, char value)
        {
            str.NotNull(nameof(str));

#if !NET48
            return str.EndsWith(value);
#else
            return str.EndsWith(value.ToString(CultureInfo.InvariantCulture), StringComparison.InvariantCulture);
#endif
        }

        ///// <summary>
        ///// 确定字符串以指定字符结尾。
        ///// </summary>
        ///// <param name="str">给定的字符串。</param>
        ///// <param name="value">给定的字符。</param>
        ///// <returns>返回布尔值。</returns>
        //[SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        //public static bool CompatibleEndsWith(this string str, string value)
        //{
        //    #if !NET48
        //        return str.EndsWith(value, StringComparison.InvariantCulture);
        //    #else
        //        return str.EndsWith(value, StringComparison.InvariantCulture);
        //    #endif
        //}


        /// <summary>
        /// 分拆字符。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <param name="value">给定的字符。</param>
        /// <returns>返回布尔值。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static string[] CompatibleSplit(this string str, char value)
        {
            str.NotNull(nameof(str));

#if !NET48
            return str.Split(value);
#else
            return str.Split(value); // 在方法直接用会出现不支持 NET48 的感叹号
#endif
        }

        /// <summary>
        /// 分拆字符串。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <param name="value">给定的字符串。</param>
        /// <returns>返回布尔值。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static string[] CompatibleSplit(this string str, string value)
        {
            str.NotNull(nameof(str));

#if !NET48
            return str.Split(value);
#else
            return Regex.Split(str, Regex.Escape(value), RegexOptions.IgnoreCase);
#endif
        }


        /// <summary>
        /// 替换字符串。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <param name="oldValue">给定的旧值。</param>
        /// <param name="newValue">给定的新值。</param>
        /// <returns>返回布尔值。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static string CompatibleReplace(this string str, string oldValue, string newValue)
        {
            str.NotNull(nameof(str));

#if !NET48
            return str.Replace(oldValue, newValue, StringComparison.InvariantCulture);
#else
            return str.Replace(oldValue, newValue);
#endif
        }


        /// <summary>
        /// 组合为字符串。
        /// </summary>
        /// <param name="values">给定的字符串集合。</param>
        /// <param name="separator">给定的连接符。</param>
        /// <returns>返回布尔值。</returns>
        public static string CompatibleJoinString(this IEnumerable<string> values, char separator)
        {
#if !NET48
            return string.Join(separator, values);
#else
            return string.Join(separator.ToString(CultureInfo.InvariantCulture), values);
#endif
        }


        /// <summary>
        /// 定义 Unix 时间等于 0 的时间点。
        /// </summary>
        /// <returns>返回 <see cref="DateTime"/>。</returns>
        public static DateTime CompatibleUnixEpoch()
        {
#if !NET48
            return DateTime.UnixEpoch;
#else
            return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
#endif
        }

        /// <summary>
        /// 定义 Unix 时间等于 0 的时间点。
        /// </summary>
        /// <returns>返回 <see cref="DateTimeOffset"/>。</returns>
        public static DateTimeOffset CompatibleUnixEpochOffset()
        {
#if !NET48
            return DateTimeOffset.UnixEpoch;
#else
            var dateTime = CompatibleUnixEpoch();
            return new DateTimeOffset(dateTime, TimeZoneInfo.Utc.GetUtcOffset(dateTime));
#endif
        }

    }
}
