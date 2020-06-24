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
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Librame.Extensions
{
    /// <summary>
    /// 字符串静态扩展。
    /// </summary>
    public static class StringExtensions
    {

        #region Leading & Trailing

        /// <summary>
        /// 确保字符串以指定字符开始。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="str"/> or <paramref name="value"/> is null.
        /// </exception>
        /// <param name="str">给定的当前字符串。</param>
        /// <param name="value">给定要确保的字符。</param>
        /// <returns>返回字符串。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static string EnsureLeading(this string str, char value)
        {
            str.NotEmpty(nameof(str));
            return str.CompatibleStartsWith(value) ? str : $"{value}{str}";
        }

        /// <summary>
        /// 确保字符串以指定字符串开始。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="str"/> or <paramref name="value"/> is null.
        /// </exception>
        /// <param name="str">给定的当前字符串。</param>
        /// <param name="value">给定要确保的字符串。</param>
        /// <param name="comparisonType">给定的 <see cref="StringComparison"/>（可选；默认忽略大小写）。</param>
        /// <returns>返回字符串。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static string EnsureLeading(this string str, string value,
            StringComparison comparisonType = StringComparison.OrdinalIgnoreCase)
        {
            str.NotEmpty(nameof(str));
            return str.StartsWith(value, comparisonType) ? str : $"{value}{str}";
        }


        /// <summary>
        /// 确保字符串以指定字符结尾。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="str"/> or <paramref name="value"/> is null.
        /// </exception>
        /// <param name="str">给定的当前字符串。</param>
        /// <param name="value">给定要确保的字符。</param>
        /// <returns>返回字符串。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static string EnsureTrailing(this string str, char value)
        {
            str.NotEmpty(nameof(str));
            return str.CompatibleEndsWith(value) ? str : $"{str}{value}";
        }

        /// <summary>
        /// 确保字符串以指定字符串结尾。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="str"/> or <paramref name="value"/> is null.
        /// </exception>
        /// <param name="str">给定的当前字符串。</param>
        /// <param name="value">给定要确保的字符串。</param>
        /// <param name="comparisonType">给定的 <see cref="StringComparison"/>（可选；默认忽略大小写）。</param>
        /// <returns>返回字符串。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static string EnsureTrailing(this string str, string value,
            StringComparison comparisonType = StringComparison.OrdinalIgnoreCase)
        {
            str.NotEmpty(nameof(str));
            return str.EndsWith(value, comparisonType) ? str : $"{str}{value}";
        }

        #endregion


        #region Format

        /// <summary>
        /// 格式化字符串参数。
        /// </summary>
        /// <param name="format">给定的格式化字符串。</param>
        /// <param name="args">给定的参数数组。</param>
        /// <returns>返回字符串。</returns>
        public static string Format(this string format, params object[] args)
            => string.Format(CultureInfo.CurrentCulture, format, args);


        /// <summary>
        /// 格式化为字符串。
        /// </summary>
        /// <param name="buffer">给定的字节数组。</param>
        /// <param name="timeTicks">给定的时间周期数。</param>
        /// <returns>返回字符串。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数")]
        public static string FormatString(this byte[] buffer, long timeTicks)
        {
            buffer.NotNull(nameof(buffer));

            var i = 1L;
            foreach (var b in buffer)
                i *= b + 1;

            return "{0:x}".Format(_ = timeTicks);
        }


        /// <summary>
        /// 将数值格式化为 2 位长度的字符串（如：01）。
        /// </summary>
        /// <param name="number">给定的 32 位带符号整数。</param>
        /// <returns>返回字符串。</returns>
        public static string FormatString(this int number)
            => number.FormatString(2);

        /// <summary>
        /// 将数值格式化为指定长度的字符串。
        /// </summary>
        /// <param name="number">给定的 32 位带符号整数。</param>
        /// <param name="length">指定的长度。</param>
        /// <returns>返回字符串。</returns>
        public static string FormatString(this int number, int length)
        {
            return number.FormatString(length,
                (format, value) => string.Format(CultureInfo.InvariantCulture, format, value));
        }


        /// <summary>
        /// 将数值格式化为 2 位长度的字符串（如：01）。
        /// </summary>
        /// <param name="number">给定的 64 位带符号整数。</param>
        /// <returns>返回字符串。</returns>
        public static string FormatString(this long number)
            => number.FormatString(2);

        /// <summary>
        /// 将数值格式化为指定长度的字符串。
        /// </summary>
        /// <param name="number">给定的 64 位带符号整数。</param>
        /// <param name="length">指定的长度。</param>
        /// <returns>返回字符串。</returns>
        public static string FormatString(this long number, int length)
        {
            return number.FormatString(length,
                (format, value) => string.Format(CultureInfo.InvariantCulture, format, value));
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
        /// 将单词转换为对应首字符大写的形式。如将 hello 转换为 Hello。
        /// </summary>
        /// <param name="word">给定的英文单词。</param>
        /// <param name="separator">给定的分隔符。</param>
        /// <returns>返回字符串。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        [SuppressMessage("Microsoft.Design", "CA1308:NormalizeStringsToUppercase")]
        public static string AsPascalCasing(this string word, char separator)
        {
            word.NotEmpty(nameof(word));
            return word.Split(separator).AsPascalCasing().CompatibleJoinString(separator);
        }

        /// <summary>
        /// 将单词转换为对应首字符大写的形式。如将 hello 转换为 Hello。
        /// </summary>
        /// <param name="word">给定的英文单词。</param>
        /// <param name="separator">给定的分隔符（可选）。</param>
        /// <returns>返回字符串。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        [SuppressMessage("Microsoft.Design", "CA1308:NormalizeStringsToUppercase")]
        public static string AsPascalCasing(this string word, string separator = null)
        {
            word.NotEmpty(nameof(word));

            if (separator.IsNotEmpty() && word.CompatibleContains(separator))
                return string.Join(separator, word.CompatibleSplit(separator).AsPascalCasing());

            return char.ToUpperInvariant(word[0]) + word.Substring(1);
        }

        /// <summary>
        /// 将数组的各单词转换为对应首字符大写的形式。如将 hello 转换为 Hello。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// words 为空或空字符串数组。
        /// </exception>
        /// <param name="words">给定的英文单词数组。</param>
        /// <returns>返回字符串数组。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        [SuppressMessage("Microsoft.Design", "CA1308:NormalizeStringsToUppercase")]
        public static string[] AsPascalCasing(this string[] words)
        {
            words.NotEmpty(nameof(words));

            var array = new string[words.Length];
            words.ForEach((word, i) =>
            {
                // 首字符大写
                array[i] = char.ToUpperInvariant(word[0]) + word.Substring(1);
            });

            return array;
        }


        /// <summary>
        /// 将单词转换为对应首字符小写的形式。如将 Hello 转换为 hello。
        /// </summary>
        /// <param name="word">给定的英文单词。</param>
        /// <param name="separator">给定的分隔符。</param>
        /// <returns>返回字符串。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        [SuppressMessage("Microsoft.Design", "CA1308:NormalizeStringsToUppercase")]
        public static string AsCamelCasing(this string word, char separator)
        {
            word.NotEmpty(nameof(word));
            return word.Split(separator).AsCamelCasing().CompatibleJoinString(separator);
        }

        /// <summary>
        /// 将单词转换为对应首字符小写的形式。如将 Hello 转换为 hello。
        /// </summary>
        /// <param name="word">给定的英文单词。</param>
        /// <param name="separator">给定的分隔符（可选）。</param>
        /// <returns>返回字符串。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        [SuppressMessage("Microsoft.Design", "CA1308:NormalizeStringsToUppercase")]
        public static string AsCamelCasing(this string word, string separator = null)
        {
            word.NotEmpty(nameof(word));

            if (separator.IsNotEmpty() && word.CompatibleContains(separator))
                return string.Join(separator, word.CompatibleSplit(separator).AsCamelCasing());

            return char.ToLowerInvariant(word[0]) + word.Substring(1);
        }

        /// <summary>
        /// 包含一到多个单词，第一个单词小写，其余单词中每一个单词第一个字母大写，其余字母均小写。例如：helloWorld 等。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// words 为空或空字符串。
        /// </exception>
        /// <param name="words">给定的英文单词（多个单词以空格区分）。</param>
        /// <returns>返回字符串。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        [SuppressMessage("Microsoft.Design", "CA1308:NormalizeStringsToUppercase")]
        public static string[] AsCamelCasing(this string[] words)
        {
            words.NotEmpty(nameof(words));

            var array = new string[words.Length];

            // 首单词首字符小写
            var first = words[0];
            array[0] = char.ToLowerInvariant(first[0]) + first.Substring(1);

            if (words.Length > 1)
            {
                for (var i = 1; i < words.Length; i++)
                {
                    // 首字符小写
                    var word = words[i];
                    array[i] = char.ToLowerInvariant(word[0]) + word.Substring(1);
                }
            }

            return array;
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
            plural.NotEmpty(nameof(plural));

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
            singular.NotEmpty(nameof(singular));

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
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static KeyValuePair<string, string> SplitPair(this string pair, char separator = '=')
        {
            pair.NotEmpty(nameof(pair));

            var separatorIndex = pair.CompatibleIndexOf(separator);
            var name = pair.Substring(0, separatorIndex);
            var value = pair.Substring(separatorIndex + 1);

            return new KeyValuePair<string, string>(name, value);
        }

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
        /// <param name="separator">给定字符串包含的分隔符。</param>
        /// <returns>返回键值对。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static KeyValuePair<string, string> SplitPair(this string pair, string separator)
        {
            pair.NotEmpty(nameof(pair));

            var separatorIndex = pair.IndexOf(separator, StringComparison.OrdinalIgnoreCase);
            var name = pair.Substring(0, separatorIndex);
            var value = pair.Substring(separatorIndex + separator.Length);

            return new KeyValuePair<string, string>(name, value);
        }


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
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static KeyValuePair<string, string> SplitPairByLastIndexOf(this string pair, char separator = '=')
        {
            pair.NotEmpty(nameof(pair));

            var separatorIndex = pair.LastIndexOf(separator);
            var name = pair.Substring(0, separatorIndex);
            var value = pair.Substring(separatorIndex + 1);

            return new KeyValuePair<string, string>(name, value);
        }

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
        /// <param name="separator">给定字符串包含的分隔符。</param>
        /// <returns>返回键值对。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static KeyValuePair<string, string> SplitPairByLastIndexOf(this string pair, string separator)
        {
            pair.NotEmpty(nameof(pair));

            var separatorIndex = pair.LastIndexOf(separator, StringComparison.OrdinalIgnoreCase);
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
            => Trim(str, ",");

        /// <summary>
        /// 清除首尾英文句号。
        /// </summary>
        /// <param name="str">指定的字符串。</param>
        /// <returns>返回清除后的字符串。</returns>
        public static string TrimPeriod(this string str)
            => Trim(str, ".");

        /// <summary>
        /// 清除首尾英文分号。
        /// </summary>
        /// <param name="str">指定的字符串。</param>
        /// <returns>返回清除后的字符串。</returns>
        public static string TrimSemicolon(this string str)
            => Trim(str, ";");


        /// <summary>
        /// 清除首尾指定字符串（忽略大小写）。
        /// </summary>
        /// <param name="str">指定的字符串。</param>
        /// <param name="trim">要清除的字符串（如果为空则直接返回）。</param>
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
        /// <param name="trim">要清除的字符串（如果为空则直接返回）。</param>
        /// <param name="loops">是否循环查找（可选；默认启用）。</param>
        /// <returns>返回清除后的字符串。</returns>
        public static string TrimStart(this string str, string trim, bool loops = true)
        {
            if (str.IsNotEmpty() && trim.IsNotEmpty() && str.StartsWith(trim, StringComparison.OrdinalIgnoreCase))
            {
                str = str.Substring(trim.Length);
                if (loops)
                    str = TrimStart(str, trim);
            }

            return str;
        }

        /// <summary>
        /// 清除尾部指定字符串（忽略大小写）。
        /// </summary>
        /// <param name="str">指定的字符串。</param>
        /// <param name="trim">要清除的字符串（如果为空则直接返回）。</param>
        /// <param name="loops">是否循环查找（可选；默认启用）。</param>
        /// <returns>返回清除后的字符串。</returns>
        public static string TrimEnd(this string str, string trim, bool loops = true)
        {
            if (str.IsNotEmpty() && trim.IsNotEmpty() && str.EndsWith(trim, StringComparison.OrdinalIgnoreCase))
            {
                str = str.Substring(0, str.Length - trim.Length);
                if (loops)
                    str = TrimEnd(str, trim);
            }

            return str;
        }

        #endregion

    }
}
