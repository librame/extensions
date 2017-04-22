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

namespace Librame.Utility
{
    /// <summary>
    /// <see cref="string"/> 实用工具。
    /// </summary>
    public class StringUtility
    {
        /// <summary>
        /// 空字符串。
        /// </summary>
        public const string Empty = "";

        /// <summary>
        /// 英文逗号分隔符。
        /// </summary>
        public const string PUNCTUATION_ENGLISH_COMMA = ",";

        /// <summary>
        /// 英文分号分隔符。
        /// </summary>
        public const string PUNCTUATION_ENGLISH_SEMICOLON = ";";

        /// <summary>
        /// 英文句号分隔符。
        /// </summary>
        public const string PUNCTUATION_ENGLISH_PERIOD = ".";

        /// <summary>
        /// 等号分隔符。
        /// </summary>
        public const string SEPARATOR_EQUALS = "=";


        /// <summary>
        /// 附加字符串到当前字符串的末尾。
        /// </summary>
        /// <param name="str">给定的当前字符串。</param>
        /// <param name="append">要附加的字符串。</param>
        /// <returns>返回附加后的字符串。</returns>
        public static string Append(string str, string append)
        {
            return (str + append);
        }


        #region AsOrDefault

        /// <summary>
        /// 转换字符串格式的布尔值或返回默认值。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <param name="defaultValue">给定的默认值。</param>
        /// <returns>返回布尔值。</returns>
        public static bool AsOrDefault(string str, bool defaultValue)
        {
            return AsOrDefault(str, s => Boolean.Parse(s), defaultValue);
        }
        /// <summary>
        /// 转换字符串格式的 32 位整数值或返回默认值。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <param name="defaultValue">给定的默认值。</param>
        /// <returns>返回 32 位整数值。</returns>
        public static int AsOrDefault(string str, int defaultValue)
        {
            return AsOrDefault(str, s => Int32.Parse(s), defaultValue);
        }
        /// <summary>
        /// 转换字符串格式的 64 位整数值或返回默认值。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <param name="defaultValue">给定的默认值。</param>
        /// <returns>返回 64 位整数值。</returns>
        public static long AsOrDefault(string str, long defaultValue)
        {
            return AsOrDefault(str, s => Int64.Parse(s), defaultValue);
        }
        /// <summary>
        /// 转换指定值类型的值。
        /// </summary>
        /// <typeparam name="TValue">指定的值类型。</typeparam>
        /// <param name="str">给定的字符串。</param>
        /// <param name="convertFactory">给定的转换方法。</param>
        /// <param name="defaultValue">给定的默认值。</param>
        /// <returns>返回值。</returns>
        public static TValue AsOrDefault<TValue>(string str, Func<string, TValue> convertFactory, TValue defaultValue)
        {
            try
            {
                return convertFactory(str);
            }
            catch
            {
                return defaultValue;
            }
        }

        #endregion


        #region Trim

        /// <summary>
        /// 清除首尾英文逗号。
        /// </summary>
        /// <param name="str">指定的字符串。</param>
        /// <returns>返回清除后的字符串。</returns>
        public static string TrimComma(string str)
        {
            return Trim(str, PUNCTUATION_ENGLISH_COMMA);
        }

        /// <summary>
        /// 清除首尾英文句号。
        /// </summary>
        /// <param name="str">指定的字符串。</param>
        /// <returns>返回清除后的字符串。</returns>
        public static string TrimPeriod(string str)
        {
            return Trim(str, PUNCTUATION_ENGLISH_PERIOD);
        }

        /// <summary>
        /// 清除首尾英文分号。
        /// </summary>
        /// <param name="str">指定的字符串。</param>
        /// <returns>返回清除后的字符串。</returns>
        public static string TrimSemicolon(string str)
        {
            return Trim(str, PUNCTUATION_ENGLISH_SEMICOLON);
        }


        /// <summary>
        /// 清除首尾指定字符串（忽略大小写）。
        /// </summary>
        /// <param name="str">指定的字符串。</param>
        /// <param name="trim">要清除的字符串。</param>
        /// <param name="loops">是否循环查找（可选；默认启用）。</param>
        /// <returns>返回清除后的字符串。</returns>
        public static string Trim(string str, string trim, bool loops = true)
        {
            str = TrimStart(str, trim, loops);

            str = TrimEnd(str, trim, loops);

            return str;
        }

        /// <summary>
        /// 清除首部指定字符串（忽略大小写）。
        /// </summary>
        /// <param name="str">指定的字符串。</param>
        /// <param name="trim">要清除的字符串。</param>
        /// <param name="loops">是否循环查找（可选；默认启用）。</param>
        /// <returns>返回清除后的字符串。</returns>
        public static string TrimStart(string str, string trim, bool loops = true)
        {
            if (str.StartsWith(trim, StringComparison.OrdinalIgnoreCase))
            {
                str = str.Substring(trim.Length);

                if (loops)
                {
                    str = TrimStart(str, trim);
                }
            }

            return str;
        }

        /// <summary>
        /// 清除尾部指定字符串（忽略大小写）。
        /// </summary>
        /// <param name="str">指定的字符串。</param>
        /// <param name="trim">要清除的字符串。</param>
        /// <param name="loops">是否循环查找（可选；默认启用）。</param>
        /// <returns>返回清除后的字符串。</returns>
        public static string TrimEnd(string str, string trim, bool loops = true)
        {
            if (str.EndsWith(trim, StringComparison.OrdinalIgnoreCase))
            {
                str = str.Substring(0, (str.Length - trim.Length));

                if (loops)
                {
                    str = TrimEnd(str, trim);
                }
            }

            return str;
        }

        #endregion

    }


    /// <summary>
    /// <see cref="StringUtility"/> 静态扩展。
    /// </summary>
    public static class StringUtilityExtensions
    {
        /// <summary>
        /// 附加字符串到当前字符串的末尾。
        /// </summary>
        /// <param name="str">给定的当前字符串。</param>
        /// <param name="append">要附加的字符串。</param>
        /// <returns>返回附加后的字符串。</returns>
        public static string Append(this string str, string append)
        {
            return (str + append);
        }


        #region AsOrDefault

        /// <summary>
        /// 转换字符串格式的布尔值或返回默认值。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <param name="defaultValue">给定的默认值。</param>
        /// <returns>返回布尔值。</returns>
        public static bool AsOrDefault(this string str, bool defaultValue)
        {
            return StringUtility.AsOrDefault(str, defaultValue);
        }
        /// <summary>
        /// 转换字符串格式的 32 位整数值或返回默认值。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <param name="defaultValue">给定的默认值。</param>
        /// <returns>返回 32 位整数值。</returns>
        public static int AsOrDefault(this string str, int defaultValue)
        {
            return StringUtility.AsOrDefault(str, defaultValue);
        }
        /// <summary>
        /// 转换字符串格式的 64 位整数值或返回默认值。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <param name="defaultValue">给定的默认值。</param>
        /// <returns>返回 64 位整数值。</returns>
        public static long AsOrDefault(this string str, long defaultValue)
        {
            return StringUtility.AsOrDefault(str, defaultValue);
        }
        /// <summary>
        /// 转换指定值类型的值。
        /// </summary>
        /// <typeparam name="TValue">指定的值类型。</typeparam>
        /// <param name="str">给定的字符串。</param>
        /// <param name="convertFactory">给定的转换方法。</param>
        /// <param name="defaultValue">给定的默认值。</param>
        /// <returns>返回值。</returns>
        public static TValue AsOrDefault<TValue>(this string str, Func<string, TValue> convertFactory, TValue defaultValue)
        {
            return StringUtility.AsOrDefault(str, convertFactory, defaultValue);
        }

        #endregion


        #region Trim

        /// <summary>
        /// 清除首尾英文逗号。
        /// </summary>
        /// <param name="str">指定的字符串。</param>
        /// <returns>返回清除后的字符串。</returns>
        public static string TrimComma(this string str)
        {
            return StringUtility.TrimComma(str);
        }

        /// <summary>
        /// 清除首尾英文句号。
        /// </summary>
        /// <param name="str">指定的字符串。</param>
        /// <returns>返回清除后的字符串。</returns>
        public static string TrimPeriod(this string str)
        {
            return StringUtility.TrimPeriod(str);
        }

        /// <summary>
        /// 清除首尾英文分号。
        /// </summary>
        /// <param name="str">指定的字符串。</param>
        /// <returns>返回清除后的字符串。</returns>
        public static string TrimSemicolon(this string str)
        {
            return StringUtility.TrimSemicolon(str);
        }


        /// <summary>
        /// 清除首尾指定字符串（忽略大小写）。
        /// </summary>
        /// <param name="str">指定的字符串。</param>
        /// <param name="trim">要清除的字符串。</param>
        /// <param name="loops">是否循环查找（可选；默认启用）。</param>
        /// <returns>返回清除后的字符串。</returns>
        public static string Trim(this string str, string trim, bool loops = true)
        {
            return StringUtility.Trim(str, trim, loops);
        }

        /// <summary>
        /// 清除首部指定字符串（忽略大小写）。
        /// </summary>
        /// <param name="str">指定的字符串。</param>
        /// <param name="trim">要清除的字符串。</param>
        /// <param name="loops">是否循环查找（可选；默认启用）。</param>
        /// <returns>返回清除后的字符串。</returns>
        public static string TrimStart(this string str, string trim, bool loops = true)
        {
            return StringUtility.TrimStart(str, trim, loops);
        }

        /// <summary>
        /// 清除尾部指定字符串（忽略大小写）。
        /// </summary>
        /// <param name="str">指定的字符串。</param>
        /// <param name="trim">要清除的字符串。</param>
        /// <param name="loops">是否循环查找（可选；默认启用）。</param>
        /// <returns>返回清除后的字符串。</returns>
        public static string TrimEnd(this string str, string trim, bool loops = true)
        {
            return StringUtility.TrimEnd(str, trim, loops);
        }

        #endregion

    }
}
