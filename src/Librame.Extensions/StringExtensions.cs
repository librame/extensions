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
using System.Text.RegularExpressions;

namespace Librame.Extensions
{
    /// <summary>
    /// 字符串静态扩展。
    /// </summary>
    public static class StringExtensions
    {

        #region FormatString

        /// <summary>
        /// 将数值格式化为 2 位长度的字符串（如：01）。
        /// </summary>
        /// <param name="number">给定的 32 位带符号整数。</param>
        /// <returns>返回字符串。</returns>
        public static string FormatString(this int number)
        {
            return number.FormatString(2);
        }
        /// <summary>
        /// 将数值格式化为指定长度的字符串。
        /// </summary>
        /// <param name="number">给定的 32 位带符号整数。</param>
        /// <param name="length">指定的长度。</param>
        /// <returns>返回字符串。</returns>
        public static string FormatString(this int number, int length)
        {
            return number.FormatString(length,
                (format, value) => string.Format(format, value));
        }


        /// <summary>
        /// 将数值格式化为 2 位长度的字符串（如：01）。
        /// </summary>
        /// <param name="number">给定的 64 位带符号整数。</param>
        /// <returns>返回字符串。</returns>
        public static string FormatString(this long number)
        {
            return number.FormatString(2);
        }
        /// <summary>
        /// 将数值格式化为指定长度的字符串。
        /// </summary>
        /// <param name="number">给定的 64 位带符号整数。</param>
        /// <param name="length">指定的长度。</param>
        /// <returns>返回字符串。</returns>
        public static string FormatString(this long number, int length)
        {
            return number.FormatString(length,
                (format, value) => string.Format(format, value));
        }


        private static string FormatString<TValue>(this TValue value, int length,
            Func<string, TValue, string> formatFactory)
            where TValue : struct
        {
            string valueString = value.ToString();

            if (valueString.Length >= length)
                return valueString;

            string format = "0:";

            for (int i = 0; i < length; i++)
                format += "0";

            format = "{" + format + "}";

            return formatFactory.Invoke(format, value);
        }

        #endregion


        #region Naming Conventions

        /// <summary>
        /// 包含一到多个单词，每一个单词第一个字母大写，其余字母均小写。例如：HelloWorld 等。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// words 为空或空字符串。
        /// </exception>
        /// <param name="words">给定的英文单词（多个单词以空格区分）。</param>
        /// <returns>返回字符串。</returns>
        public static string AsPascalCasing(this string[] words)
        {
            words.NotNull(nameof(words));

            string str = string.Empty;

            foreach (var w in words)
            {
                // 首字母大写，其余字母均小写
                str += char.ToUpper(w[0]) + w.Substring(1).ToLower();
            }

            return str;
        }


        /// <summary>
        /// 包含一到多个单词，第一个单词小写，其余单词中每一个单词第一个字母大写，其余字母均小写。例如：helloWorld 等。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// words 为空或空字符串。
        /// </exception>
        /// <param name="words">给定的英文单词（多个单词以空格区分）。</param>
        /// <returns>返回字符串。</returns>
        public static string AsCamelCasing(this string[] words)
        {
            words.NotNull(nameof(words));

            // 首单词小写
            string str = words[0].ToLower();

            if (words.Length > 1)
            {
                for (var i = 1; i < words.Length; i++)
                {
                    var w = words[i];

                    // 首字母大写，其余字母均小写
                    str += char.ToUpper(w[0]) + w.Substring(1).ToLower();
                }
            }

            return str;
        }

        #endregion


        #region Singular & Plural

        /// <summary>
        /// 复数单词单数化。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// plural 为空或空字符串。
        /// </exception>
        /// <param name="plural">给定的复数化英文单词。</param>
        /// <returns>返回字符串。</returns>
        public static string AsSingularize(this string plural)
        {
            plural.NotNullOrEmpty(nameof(plural));

            Regex plural1 = new Regex("(?<keep>[^aeiou])ies$");
            Regex plural2 = new Regex("(?<keep>[aeiou]y)s$");
            Regex plural3 = new Regex("(?<keep>[sxzh])es$");
            Regex plural4 = new Regex("(?<keep>[^sxzhyu])s$");

            if (plural1.IsMatch(plural))
                return plural1.Replace(plural, "${keep}y");
            else if (plural2.IsMatch(plural))
                return plural2.Replace(plural, "${keep}");
            else if (plural3.IsMatch(plural))
                return plural3.Replace(plural, "${keep}");
            else if (plural4.IsMatch(plural))
                return plural4.Replace(plural, "${keep}");

            return plural;
        }


        /// <summary>
        /// 单数单词复数化。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// singular 为空或空字符串。
        /// </exception>
        /// <param name="singular">给定的单数化英文单词。</param>
        /// <returns>返回字符串。</returns>
        public static string AsPluralize(this string singular)
        {
            singular.NotNullOrEmpty(nameof(singular));

            Regex plural1 = new Regex("(?<keep>[^aeiou])y$");
            Regex plural2 = new Regex("(?<keep>[aeiou]y)$");
            Regex plural3 = new Regex("(?<keep>[sxzh])$");
            Regex plural4 = new Regex("(?<keep>[^sxzhy])$");

            if (plural1.IsMatch(singular))
                return plural1.Replace(singular, "${keep}ies");
            else if (plural2.IsMatch(singular))
                return plural2.Replace(singular, "${keep}s");
            else if (plural3.IsMatch(singular))
                return plural3.Replace(singular, "${keep}es");
            else if (plural4.IsMatch(singular))
                return plural4.Replace(singular, "${keep}s");

            return singular;
        }

        #endregion


        #region SplitPair

        /// <summary>
        /// 分拆键值对字符串。
        /// </summary>
        /// <example>
        /// var pair = "key==-value";
        /// return (Key:key, Value:=-value).
        /// </example>
        /// <exception cref="ArgumentNullException">
        /// pair or separator is empty.
        /// </exception>
        /// <param name="pair">给定的键值对字符串。</param>
        /// <param name="separator">给定字符串包含的分隔符（可选；默认为等号）。</param>
        /// <returns>返回键值对。</returns>
        public static KeyValuePair<string, string> SplitPair(this string pair, string separator = "=")
        {
            pair.NotNullOrEmpty(nameof(pair));
            //separator.NotEmpty(nameof(separator));

            var separatorIndex = pair.IndexOf(separator);
            var name = pair.Substring(0, separatorIndex);
            var value = pair.Substring(separatorIndex + separator.Length);

            return new KeyValuePair<string, string>(name, value);
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
            return Trim(str, ",");
        }

        /// <summary>
        /// 清除首尾英文句号。
        /// </summary>
        /// <param name="str">指定的字符串。</param>
        /// <returns>返回清除后的字符串。</returns>
        public static string TrimPeriod(this string str)
        {
            return Trim(str, ".");
        }

        /// <summary>
        /// 清除首尾英文分号。
        /// </summary>
        /// <param name="str">指定的字符串。</param>
        /// <returns>返回清除后的字符串。</returns>
        public static string TrimSemicolon(this string str)
        {
            return Trim(str, ";");
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
        public static string TrimStart(this string str, string trim, bool loops = true)
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
        public static string TrimEnd(this string str, string trim, bool loops = true)
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
}
