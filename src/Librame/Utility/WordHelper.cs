#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Text.RegularExpressions;

namespace Librame.Utility
{
    /// <summary>
    /// 单词助手。
    /// </summary>
    public class WordHelper
    {
        /// <summary>
        /// 清除除空格外的所有特殊字符（多个空格会被替换单个空格）。
        /// </summary>
        /// <param name="words">给定的英文单词。</param>
        /// <returns>返回字符串。</returns>
        public static string ClearSpecialCharactersWithSpace(string words)
        {
            // 将可能存在的多个空格、特殊字符替换为单个空格
            return Regex.Replace(words, @"([^0-9A-Za-z\s+].*?)", string.Empty);
        }


        /// <summary>
        /// 包含一到多个单词，每一个单词第一个字母大写，其余字母均小写。例如：HelloWorld 等。
        /// </summary>
        /// <param name="words">给定的英文单词（多个单词以空格区分）。</param>
        /// <returns>返回字符串。</returns>
        public static string AsPascalCasing(string words)
        {
            words = ClearSpecialCharactersWithSpace(words);
            var array = words.Split(' ');

            string str = string.Empty;
            foreach (var a in array)
            {
                // 首字母大写，其余字母均小写
                str += char.ToUpper(a[0]) + a.Substring(1).ToLower();
            }

            return str;
        }


        /// <summary>
        /// 包含一到多个单词，第一个单词小写，其余单词中每一个单词第一个字母大写，其余字母均小写。例如：helloWorld 等。
        /// </summary>
        /// <param name="words">给定的英文单词（多个单词以空格区分）。</param>
        /// <returns>返回字符串。</returns>
        public static string AsCamelCasing(string words)
        {
            words = ClearSpecialCharactersWithSpace(words);
            var array = words.Split(' ');

            // 首单词小写
            string str = array[0].ToLower();
            if (array.Length > 1)
            {
                for (var i = 1; i < array.Length; i++)
                {
                    var w = array[i];

                    // 首字母大写，其余字母均小写
                    str += char.ToUpper(w[0]) + w.Substring(1).ToLower();
                }
            }

            return str;
        }


        /// <summary>
        /// 复数单词单数化。
        /// </summary>
        /// <param name="plural">给定的复数化英文单词。</param>
        /// <returns>返回字符串。</returns>
        public static string AsSingularize(string plural)
        {
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
        /// <param name="singular">给定的单数化英文单词。</param>
        /// <returns>返回字符串。</returns>
        public static string AsPluralize(string singular)
        {
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

    }


    /// <summary>
    /// <see cref="WordHelper"/> 静态扩展。
    /// </summary>
    public static class WordHelperExtensions
    {
        /// <summary>
        /// 清除除空格外的所有特殊字符（多个空格会被替换单个空格）。
        /// </summary>
        /// <param name="englishWords">给定的英文单词。</param>
        /// <returns>返回字符串。</returns>
        public static string ClearSpecialCharactersWithSpace(this string englishWords)
        {
            return WordHelper.ClearSpecialCharactersWithSpace(englishWords);
        }


        /// <summary>
        /// 包含一到多个单词，每一个单词第一个字母大写，其余字母均小写。例如：HelloWorld 等。
        /// </summary>
        /// <param name="englishWords">给定的英文单词（多个单词以空格区分）。</param>
        /// <returns>返回字符串。</returns>
        public static string AsPascalCasing(this string englishWords)
        {
            return WordHelper.AsPascalCasing(englishWords);
        }


        /// <summary>
        /// 包含一到多个单词，第一个单词小写，其余单词中每一个单词第一个字母大写，其余字母均小写。例如：helloWorld 等。
        /// </summary>
        /// <param name="englishWords">给定的英文单词（多个单词以空格区分）。</param>
        /// <returns>返回字符串。</returns>
        public static string AsCamelCasing(this string englishWords)
        {
            return WordHelper.AsCamelCasing(englishWords);
        }


        /// <summary>
        /// 复数单词单数化。
        /// </summary>
        /// <param name="plural">给定的复数化英文单词。</param>
        /// <returns>返回字符串。</returns>
        public static string AsSingularize(this string plural)
        {
            return WordHelper.AsSingularize(plural);
        }


        /// <summary>
        /// 单数单词复数化。
        /// </summary>
        /// <param name="singular">给定的单数化英文单词。</param>
        /// <returns>返回字符串。</returns>
        public static string AsPluralize(this string singular)
        {
            return WordHelper.AsPluralize(singular);
        }

    }
}
