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
        /// <summary>
        /// 包含。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <param name="value">给定的字符。</param>
        /// <returns>返回布尔值。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "str")]
        public static bool CompatibleContains(this string str, char value)
        {
            #if !NET48
                return str.Contains(value, StringComparison.OrdinalIgnoreCase);
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
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "str")]
        public static bool CompatibleContains(this string str, string value)
        {
            #if !NET48
                return str.Contains(value, StringComparison.OrdinalIgnoreCase);
            #else
                return str.Contains(value);
            #endif
        }


        /// <summary>
        /// 获取哈希码。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <returns>返回整数。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "str")]
        public static int CompatibleGetHashCode(this string str)
        {
            #if !NET48
                return str.GetHashCode(StringComparison.OrdinalIgnoreCase);
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
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "str")]
        public static int CompatibleIndexOf(this string str, char value)
        {
            #if !NET48
                return str.IndexOf(value, StringComparison.OrdinalIgnoreCase);
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
        //[SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "str")]
        //public static int CompatibleLastIndexOf(this string str, char value)
        //{
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
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "str")]
        public static bool CompatibleStartsWith(this string str, char value)
        {
            #if !NET48
                return str.StartsWith(value);
            #else
                return str.StartsWith(value.ToString(CultureInfo.InvariantCulture), StringComparison.OrdinalIgnoreCase);
            #endif
        }

        ///// <summary>
        ///// 确定字符串以指定字符开始。
        ///// </summary>
        ///// <param name="str">给定的字符串。</param>
        ///// <param name="value">给定的字符。</param>
        ///// <returns>返回布尔值。</returns>
        //[SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "str")]
        //public static bool CompatibleStartsWith(this string str, string value)
        //{
        //    #if !NET48
        //        return str.StartsWith(value, StringComparison.OrdinalIgnoreCase);
        //    #else
        //        return str.StartsWith(value, StringComparison.OrdinalIgnoreCase);
        //    #endif
        //}

        /// <summary>
        /// 确定字符串以指定字符结尾。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <param name="value">给定的字符。</param>
        /// <returns>返回布尔值。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "str")]
        public static bool CompatibleEndsWith(this string str, char value)
        {
            #if !NET48
                return str.EndsWith(value);
            #else
                return str.EndsWith(value.ToString(CultureInfo.InvariantCulture), StringComparison.OrdinalIgnoreCase);
            #endif
        }

        ///// <summary>
        ///// 确定字符串以指定字符结尾。
        ///// </summary>
        ///// <param name="str">给定的字符串。</param>
        ///// <param name="value">给定的字符。</param>
        ///// <returns>返回布尔值。</returns>
        //[SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "str")]
        //public static bool CompatibleEndsWith(this string str, string value)
        //{
        //    #if !NET48
        //        return str.EndsWith(value, StringComparison.OrdinalIgnoreCase);
        //    #else
        //        return str.EndsWith(value, StringComparison.OrdinalIgnoreCase);
        //    #endif
        //}


        /// <summary>
        /// 包含。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <param name="value">给定的字符。</param>
        /// <returns>返回布尔值。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "str")]
        public static string[] CompatibleSplit(this string str, string value)
        {
            #if !NET48
                return str.Split(value);
            #else
                return Regex.Split(str, Regex.Escape(value), RegexOptions.IgnoreCase);
            #endif
        }


        /// <summary>
        /// 包含。
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
        /// 替换。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <param name="oldValue">给定的旧值。</param>
        /// <param name="newValue">给定的新值。</param>
        /// <returns>返回布尔值。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "str")]
        public static string CompatibleReplace(this string str, string oldValue, string newValue)
        {
            #if !NET48
                return str.Replace(oldValue, newValue, StringComparison.OrdinalIgnoreCase);
            #else
                return str.Replace(oldValue, newValue);
            #endif
        }

    }
}
