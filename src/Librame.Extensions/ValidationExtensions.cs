﻿#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace Librame.Extensions
{
    /// <summary>
    /// 验证静态扩展。
    /// </summary>
    public static class ValidationExtensions
    {

        #region NullOrEmpty

        /// <summary>
        /// 是否为 NULL。
        /// </summary>
        /// <typeparam name="TSource">指定的源类型。</typeparam>
        /// <param name="source">给定的源实例。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsNull<TSource>(this TSource source)
            => null == source;

        /// <summary>
        /// 是否不为 NULL。
        /// </summary>
        /// <typeparam name="TSource">指定的源类型。</typeparam>
        /// <param name="source">给定的源实例。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsNotNull<TSource>(this TSource source)
            => null != source;


        /// <summary>
        /// 是否为 NULL 或空格。
        /// </summary>
        /// <remarks>
        /// 详情参考 <see cref="string.IsNullOrWhiteSpace(string)"/>。
        /// </remarks>
        /// <param name="str">给定的字符串。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsWhiteSpace(this string str)
            => string.IsNullOrWhiteSpace(str);

        /// <summary>
        /// 是否不为 NULL 或空格。
        /// </summary>
        /// <remarks>
        /// 详情参考 <see cref="string.IsNullOrWhiteSpace(string)"/>。
        /// </remarks>
        /// <param name="str">给定的字符串。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsNotWhiteSpace(this string str)
            => !string.IsNullOrWhiteSpace(str);


        /// <summary>
        /// 是否为 NULL 或空字符串。
        /// </summary>
        /// <remarks>
        /// 详情参考 <see cref="string.IsNullOrEmpty(string)"/>。
        /// </remarks>
        /// <param name="str">给定的字符串。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsEmpty(this string str)
            => string.IsNullOrEmpty(str);

        /// <summary>
        /// 是否不为 NULL 或空字符串。
        /// </summary>
        /// <remarks>
        /// 详情参考 <see cref="string.IsNullOrEmpty(string)"/>。
        /// </remarks>
        /// <param name="str">给定的字符串。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsNotEmpty(this string str)
            => !string.IsNullOrEmpty(str);


        /// <summary>
        /// 是否为 NULL 或空集合。
        /// </summary>
        /// <typeparam name="TSources">指定的源集合类型。</typeparam>
        /// <param name="sources">给定的 <see cref="IEnumerable"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsEmpty<TSources>(this TSources sources)
            where TSources : IEnumerable
        {
            if (sources.IsNull())
                return true;

            if (sources is ICollection collection)
                return collection.Count < 1;

            var enumerator = sources.GetEnumerator();
            return !enumerator.MoveNext();
        }

        /// <summary>
        /// 是否不为 NULL 或空集合。
        /// </summary>
        /// <typeparam name="TSources">指定的源集合类型。</typeparam>
        /// <param name="sources">给定的 <see cref="IEnumerable"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsNotEmpty<TSources>(this TSources sources)
            where TSources : IEnumerable
            => !sources.IsEmpty();


        /// <summary>
        /// 是否为 NULL 或空集合。
        /// </summary>
        /// <remarks>
        /// 详情参考  <see cref="Enumerable.Any{TSource}(IEnumerable{TSource})"/>。
        /// </remarks>
        /// <typeparam name="TSource">指定的源类型。</typeparam>
        /// <param name="sources">给定的 <see cref="IEnumerable{TSource}"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsEmpty<TSource>(this IEnumerable<TSource> sources)
        {
            if (sources.IsNull())
                return true;

            return !sources.Any();
        }

        /// <summary>
        /// 是否不为 NULL 或空集合。
        /// </summary>
        /// <remarks>
        /// 详情参考  <see cref="Enumerable.Any{TSource}(IEnumerable{TSource})"/>。
        /// </remarks>
        /// <typeparam name="TSource">指定的源类型。</typeparam>
        /// <param name="sources">给定的 <see cref="IEnumerable{TSource}"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsNotEmpty<TSource>(this IEnumerable<TSource> sources)
            => !sources.IsEmpty();

        #endregion


        #region Compare

        /// <summary>
        /// 是否为倍数。
        /// </summary>
        /// <param name="value">给定的数字。</param>
        /// <param name="multiples">给定的倍数。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsMultiples(this int value, int multiples)
            => 0 == value % multiples;


        /// <summary>
        /// 是否大于或大于等于对比值。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="value">给定的值。</param>
        /// <param name="compare">给定的比较值。</param>
        /// <param name="equals">是否比较等于（可选；默认不比较）。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsGreater<T>(this T value, T compare, bool equals = false)
            where T : IComparable<T>
        {
            value.NotNull(nameof(value));
            return equals ? value.CompareTo(compare) >= 0 : value.CompareTo(compare) > 0;
        }

        /// <summary>
        /// 是否小于或小于等于对比值。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="value">给定的值。</param>
        /// <param name="compare">给定的比较值。</param>
        /// <param name="equals">是否比较等于（可选；默认不比较）。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsLesser<T>(this T value, T compare, bool equals = false)
            where T : IComparable<T>
        {
            value.NotNull(nameof(value));
            return equals ? value.CompareTo(compare) <= 0 : value.CompareTo(compare) < 0;
        }


        /// <summary>
        /// 是否不超出范围对比值。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="value">给定的值。</param>
        /// <param name="compareMinimum">给定的最小比较值。</param>
        /// <param name="compareMaximum">给定的最大比较值。</param>
        /// <param name="equalMinimum">是否比较等于最小值（可选；默认不比较）。</param>
        /// <param name="equalMaximum">是否比较等于最大值（可选；默认不比较）。</param>
        public static bool IsNotOutOfRange<T>(this T value, T compareMinimum, T compareMaximum,
            bool equalMinimum = false, bool equalMaximum = false)
            where T : IComparable<T>
            => !value.IsOutOfRange(compareMinimum, compareMaximum, equalMinimum, equalMaximum);

        /// <summary>
        /// 是否超出范围对比值。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="value">给定的值。</param>
        /// <param name="compareMinimum">给定的最小比较值。</param>
        /// <param name="compareMaximum">给定的最大比较值。</param>
        /// <param name="equalMinimum">是否比较等于最小值（可选；默认不比较）。</param>
        /// <param name="equalMaximum">是否比较等于最大值（可选；默认不比较）。</param>
        public static bool IsOutOfRange<T>(this T value, T compareMinimum, T compareMaximum,
            bool equalMinimum = false, bool equalMaximum = false)
            where T : IComparable<T>
        {
            if (value.IsLesser(compareMinimum, equalMinimum))
                return true;

            return value.IsGreater(compareMaximum, equalMaximum);
        }

        #endregion


        #region Digit & Letter & AlgorithmSpecial

        /// <summary>
        /// 具有数字。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <returns>返回布尔值。</returns>
        public static bool HasDigit(this string str)
            => str.Any(IsDigit);

        /// <summary>
        /// 是数字。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsDigit(this string str)
            => str.All(IsDigit);

        /// <summary>
        /// 是数字。
        /// </summary>
        /// <param name="c">给定的字符。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsDigit(this char c)
            => c >= '0' && c <= '9';


        /// <summary>
        /// 具有小写字母。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <returns>返回布尔值。</returns>
        public static bool HasLower(this string str)
            => str.Any(IsLower);

        /// <summary>
        /// 是小写字母。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsLower(this string str)
            => str.All(IsLower);

        /// <summary>
        /// 是小写字母。
        /// </summary>
        /// <param name="c">给定的字符。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsLower(this char c)
            => c >= 'a' && c <= 'z';


        /// <summary>
        /// 具有大写字母。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <returns>返回布尔值。</returns>
        public static bool HasUpper(this string str)
            => str.Any(IsUpper);

        /// <summary>
        /// 是大写字母。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsUpper(this string str)
            => str.All(IsUpper);

        /// <summary>
        /// 是大写字母。
        /// </summary>
        /// <param name="c">给定的字符。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsUpper(this char c)
            => c >= 'A' && c <= 'Z';


        /// <summary>
        /// 具有算法特殊符号。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <returns>返回布尔值。</returns>
        public static bool HasAlgorithmSpecial(this string str)
            => str.Any(IsAlgorithmSpecial);

        /// <summary>
        /// 是算法特殊符号。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsAlgorithmSpecial(this string str)
            => str.All(IsAlgorithmSpecial);

        /// <summary>
        /// 是算法特殊符号。
        /// </summary>
        /// <param name="c">给定的字符。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsAlgorithmSpecial(this char c)
            => ExtensionSettings.Preference.AlgorithmSpecialSymbols.CompatibleContains(c);


        /// <summary>
        /// 具有大小写字母。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <returns>返回布尔值。</returns>
        public static bool HasLetter(this string str)
            => str.HasUpper() && str.HasLower();

        /// <summary>
        /// 是大小写字母。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsLetter(this string str)
            => str.HasLetter() && str.All(IsLetter);

        /// <summary>
        /// 是大小写字母。
        /// </summary>
        /// <param name="c">给定的字符。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsLetter(this char c)
            => !(!c.IsUpper() && !c.IsLower());


        /// <summary>
        /// 具有大小写字母和数字。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <returns>返回布尔值。</returns>
        public static bool HasLetterAndDigit(this string str)
            => str.HasLetter() && str.HasDigit();

        /// <summary>
        /// 是大小写字母和数字。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsLetterAndDigit(this string str)
            => str.HasLetterAndDigit() && str.All(IsLetterAndDigit);

        /// <summary>
        /// 是大小写字母和数字。
        /// </summary>
        /// <param name="c">给定的字符。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsLetterAndDigit(this char c)
            => !(!c.IsLetter() && !c.IsDigit());


        /// <summary>
        /// 具有算法安全性（包括大小写字母、数字和部分特殊符号）。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <returns>返回布尔值。</returns>
        public static bool HasAlgorithmSafety(this string str)
            => str.HasLetterAndDigit() && str.HasAlgorithmSpecial();

        /// <summary>
        /// 是算法安全性（包括大小写字母、数字和部分特殊符号）。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsAlgorithmSafety(this string str)
            => str.HasAlgorithmSafety() && str.All(IsAlgorithmSafety);

        /// <summary>
        /// 是算法安全性（包括大小写字母、数字和部分特殊符号）。
        /// </summary>
        /// <param name="c">给定的字符。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsAlgorithmSafety(this char c)
            => !(!c.IsLetterAndDigit() && !c.IsAlgorithmSpecial());

        #endregion

    }
}
