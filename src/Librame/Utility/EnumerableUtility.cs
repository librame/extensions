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
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Librame.Utility
{
    /// <summary>
    /// <see cref="Enumerable"/> 实用工具。
    /// </summary>
    public class EnumerableUtility
    {
        /// <summary>
        /// 将单个对象表示为枚举集合对象。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="item">给定的对象。</param>
        /// <returns>返回枚举集合。</returns>
        public static IEnumerable<T> AsEnumerables<T>(T item)
        {
            yield return item;
        }


        /// <summary>
        /// 将序列中的每个元素投射到新表中。
        /// </summary>
        /// <typeparam name="TSource">指定的源类型。</typeparam>
        /// <typeparam name="TResult">指定的结果类型。</typeparam>
        /// <param name="sources">给定的源集合。</param>
        /// <param name="selector">给定的选择器。</param>
        /// <param name="defaultResults">给定的默认结果集合。</param>
        /// <returns>返回结果集合。</returns>
        public static IEnumerable<TResult> AsOrDefault<TSource, TResult>(IEnumerable<TSource> sources, Func<TSource, TResult> selector, IEnumerable<TResult> defaultResults = null)
        {
            if (ReferenceEquals(sources, null) || sources.Count() < 1)
                return defaultResults;

            return sources.Select(selector);
        }

        /// <summary>
        /// 将序列中的每个元素投射到新数组中。
        /// </summary>
        /// <typeparam name="TSource">指定的源类型。</typeparam>
        /// <typeparam name="TResult">指定的结果类型。</typeparam>
        /// <param name="sources">给定的源集合。</param>
        /// <param name="selector">给定的选择器。</param>
        /// <param name="defaultResults">给定的默认结果数组。</param>
        /// <returns>返回结果数组。</returns>
        public static TResult[] AsArrayOrDefault<TSource, TResult>(IEnumerable<TSource> sources, Func<TSource, TResult> selector, TResult[] defaultResults = null)
        {
            if (ReferenceEquals(sources, null) || sources.Count() < 1)
                return defaultResults;

            return sources.Select(selector).ToArray();
        }

        /// <summary>
        /// 将序列中的每个元素投射到新列表中。
        /// </summary>
        /// <typeparam name="TSource">指定的源类型。</typeparam>
        /// <typeparam name="TResult">指定的结果类型。</typeparam>
        /// <param name="sources">给定的源集合。</param>
        /// <param name="selector">给定的选择器。</param>
        /// <param name="defaultResults">给定的默认结果集合。</param>
        /// <returns>返回结果集合。</returns>
        public static IList<TResult> AsListOrDefault<TSource, TResult>(IEnumerable<TSource> sources, Func<TSource, TResult> selector, IList<TResult> defaultResults = null)
        {
            if (ReferenceEquals(sources, null) || sources.Count() < 1)
                return defaultResults;

            return sources.Select(selector).ToList();
        }


        /// <summary>
        /// 将对象集合组合为字符串集。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="sources">给定的源集合。</param>
        /// <param name="factory">给定的解析工厂模式。</param>
        /// <param name="separator">给定的分隔符（可选）。</param>
        /// <returns>返回字符串。</returns>
        public static string JoinStrings<T>(IEnumerable<T> sources, Func<T, string> factory, string separator = StringUtility.PUNCTUATION_ENGLISH_COMMA)
        {
            return JoinStrings<T, string>(sources, factory, separator);
        }
        /// <summary>
        /// 将对象集合组合为字符串集。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <typeparam name="TValue">指定解析的值类型。</typeparam>
        /// <param name="sources">给定的源集合。</param>
        /// <param name="factory">给定的解析工厂模式。</param>
        /// <param name="separator">给定的分隔符（可选）。</param>
        /// <returns>返回字符串。</returns>
        public static string JoinStrings<T, TValue>(IEnumerable<T> sources, Func<T, TValue> factory, string separator = StringUtility.PUNCTUATION_ENGLISH_COMMA)
        {
            if (ReferenceEquals(sources, null))
                return string.Empty;

            var sb = new StringBuilder();

            bool separatorEmpty = String.IsNullOrEmpty(separator);
            int count = sources.Count();

            int i = 0;
            foreach (var s in sources)
            {
                sb.Append(factory(s));

                if (!separatorEmpty && i < count - 1)
                    sb.Append(separator);

                i++;
            }

            return sb.ToString();
        }


        /// <summary>
        /// 转换为只读列表集合。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="enumerable">给定的 <see cref="IEnumerable{T}"/>。</param>
        /// <returns>返回 <see cref="IList{T}"/>。</returns>
        public static IList<T> AsReadOnlyList<T>(IEnumerable<T> enumerable)
        {
            return new ReadOnlyCollection<T>(enumerable.ToList());
        }


        /// <summary>
        /// 循环调用方法。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="enumerable">给定的 <see cref="IEnumerable{T}"/>。</param>
        /// <param name="dispatch">给定的调用方法。</param>
        public static void Invoke<T>(IEnumerable<T> enumerable, Action<T> dispatch)
        {
            if (ReferenceEquals(enumerable, null))
                return;

            foreach (var sink in enumerable)
            {
                try
                {
                    dispatch(sink);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        /// <summary>
        /// 循环调用方法。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="enumerable">给定的 <see cref="IEnumerable{T}"/>。</param>
        /// <param name="dispatch">给定的调用方法。</param>
        public static void Invoke<T>(IEnumerable<T> enumerable, Action<int, T> dispatch)
        {
            if (ReferenceEquals(enumerable, null))
                return;

            var i = 0;
            foreach (var sink in enumerable)
            {
                try
                {
                    dispatch(i, sink);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                i++;
            }
        }

        /// <summary>
        /// 循环调用方法。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <typeparam name="TResult">指定的返回类型。</typeparam>
        /// <param name="enumerable">给定的 <see cref="IEnumerable{T}"/>。</param>
        /// <param name="dispatch">给定的调用方法。</param>
        /// <returns>返回 <see cref="IEnumerable{T}"/>。</returns>
        public static IEnumerable<TResult> Invoke<T, TResult>(IEnumerable<T> enumerable, Func<T, TResult> dispatch)
        {
            if (ReferenceEquals(enumerable, null))
                yield return default(TResult);

            foreach (var sink in enumerable)
            {
                var result = default(TResult);

                try
                {
                    result = dispatch(sink);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                yield return result;
            }
        }
        /// <summary>
        /// 循环调用方法。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <typeparam name="TResult">指定的返回类型。</typeparam>
        /// <param name="enumerable">给定的 <see cref="IEnumerable{T}"/>。</param>
        /// <param name="dispatch">给定的调用方法。</param>
        /// <returns>返回 <see cref="IEnumerable{T}"/>。</returns>
        public static IEnumerable<TResult> Invoke<T, TResult>(IEnumerable<T> enumerable, Func<int, T, TResult> dispatch)
        {
            if (ReferenceEquals(enumerable, null))
                yield return default(TResult);

            var i = 0;
            foreach (var sink in enumerable)
            {
                var result = default(TResult);

                try
                {
                    result = dispatch(i, sink);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                i++;
                yield return result;
            }
        }

    }


    /// <summary>
    /// <see cref="EnumerableUtility"/> 静态扩展。
    /// </summary>
    public static class EnumerableUtilityExtensions
    {
        /// <summary>
        /// 将单个对象表示为枚举集合对象。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="item">给定的对象。</param>
        /// <returns>返回枚举集合。</returns>
        public static IEnumerable<T> AsEnumerables<T>(this T item)
        {
            return EnumerableUtility.AsEnumerables(item);
        }


        /// <summary>
        /// 将序列中的每个元素投射到新表中。
        /// </summary>
        /// <typeparam name="TSource">指定的源类型。</typeparam>
        /// <typeparam name="TResult">指定的结果类型。</typeparam>
        /// <param name="sources">给定的源集合。</param>
        /// <param name="selector">给定的选择器。</param>
        /// <param name="defaultResults">给定的默认结果集合。</param>
        /// <returns>返回结果集合。</returns>
        public static IEnumerable<TResult> AsOrDefault<TSource, TResult>(this IEnumerable<TSource> sources, Func<TSource, TResult> selector, IEnumerable<TResult> defaultResults = null)
        {
            return EnumerableUtility.AsOrDefault(sources, selector, defaultResults);
        }

        /// <summary>
        /// 将序列中的每个元素投射到新数组中。
        /// </summary>
        /// <typeparam name="TSource">指定的源类型。</typeparam>
        /// <typeparam name="TResult">指定的结果类型。</typeparam>
        /// <param name="sources">给定的源集合。</param>
        /// <param name="selector">给定的选择器。</param>
        /// <param name="defaultResults">给定的默认结果数组。</param>
        /// <returns>返回结果数组。</returns>
        public static TResult[] AsArrayOrDefault<TSource, TResult>(this IEnumerable<TSource> sources, Func<TSource, TResult> selector, TResult[] defaultResults = null)
        {
            return EnumerableUtility.AsArrayOrDefault(sources, selector, defaultResults);
        }

        /// <summary>
        /// 将序列中的每个元素投射到新列表中。
        /// </summary>
        /// <typeparam name="TSource">指定的源类型。</typeparam>
        /// <typeparam name="TResult">指定的结果类型。</typeparam>
        /// <param name="sources">给定的源集合。</param>
        /// <param name="selector">给定的选择器。</param>
        /// <param name="defaultResults">给定的默认结果集合。</param>
        /// <returns>返回结果集合。</returns>
        public static IList<TResult> AsListOrDefault<TSource, TResult>(this IEnumerable<TSource> sources, Func<TSource, TResult> selector, IList<TResult> defaultResults = null)
        {
            return EnumerableUtility.AsListOrDefault(sources, selector, defaultResults);
        }


        /// <summary>
        /// 将对象集合组合为字符串集。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="sources">给定的源集合。</param>
        /// <param name="factory">给定的解析工厂模式。</param>
        /// <param name="separator">给定的分隔符（可选）。</param>
        /// <returns>返回字符串。</returns>
        public static string JoinStrings<T>(this IEnumerable<T> sources, Func<T, string> factory, string separator = StringUtility.PUNCTUATION_ENGLISH_COMMA)
        {
            return EnumerableUtility.JoinStrings(sources, factory, separator);
        }
        /// <summary>
        /// 将对象集合组合为字符串集。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <typeparam name="TValue">指定解析的值类型。</typeparam>
        /// <param name="sources">给定的源集合。</param>
        /// <param name="factory">给定的解析工厂模式。</param>
        /// <param name="separator">给定的分隔符（可选）。</param>
        /// <returns>返回字符串。</returns>
        public static string JoinStrings<T, TValue>(this IEnumerable<T> sources, Func<T, TValue> factory, string separator = StringUtility.PUNCTUATION_ENGLISH_COMMA)
        {
            return EnumerableUtility.JoinStrings(sources, factory, separator);
        }


        /// <summary>
        /// 转换为只读列表集合。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="enumerable">给定的 <see cref="IEnumerable{T}"/>。</param>
        /// <returns>返回 <see cref="IList{T}"/>。</returns>
        public static IList<T> AsReadOnlyList<T>(this IEnumerable<T> enumerable)
        {
            return EnumerableUtility.AsReadOnlyList(enumerable);
        }


        /// <summary>
        /// 循环调用方法。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="enumerable">给定的 <see cref="IEnumerable{T}"/>。</param>
        /// <param name="dispatch">给定的调用方法。</param>
        public static void Invoke<T>(this IEnumerable<T> enumerable, Action<T> dispatch)
        {
            EnumerableUtility.Invoke(enumerable, dispatch);
        }
        /// <summary>
        /// 循环调用方法。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="enumerable">给定的 <see cref="IEnumerable{T}"/>。</param>
        /// <param name="dispatch">给定的调用方法。</param>
        public static void Invoke<T>(this IEnumerable<T> enumerable, Action<int, T> dispatch)
        {
            EnumerableUtility.Invoke(enumerable, dispatch);
        }

        /// <summary>
        /// 循环调用方法。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <typeparam name="TResult">指定的返回类型。</typeparam>
        /// <param name="enumerable">给定的 <see cref="IEnumerable{T}"/>。</param>
        /// <param name="dispatch">给定的调用方法。</param>
        /// <returns>返回 <see cref="IEnumerable{T}"/>。</returns>
        public static IEnumerable<TResult> Invoke<T, TResult>(this IEnumerable<T> enumerable, Func<T, TResult> dispatch)
        {
            return EnumerableUtility.Invoke(enumerable, dispatch);
        }
        /// <summary>
        /// 循环调用方法。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <typeparam name="TResult">指定的返回类型。</typeparam>
        /// <param name="enumerable">给定的 <see cref="IEnumerable{T}"/>。</param>
        /// <param name="dispatch">给定的调用方法。</param>
        /// <returns>返回 <see cref="IEnumerable{T}"/>。</returns>
        public static IEnumerable<TResult> Invoke<T, TResult>(this IEnumerable<T> enumerable, Func<int, T, TResult> dispatch)
        {
            return EnumerableUtility.Invoke(enumerable, dispatch);
        }

    }
}
