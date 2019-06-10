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
using System.IO;

namespace Librame.Extensions
{
    /// <summary>
    /// 异常静态扩展。
    /// </summary>
    public static class ExceptionExtensions
    {
        /// <summary>
        /// 得到内部异常消息。
        /// </summary>
        /// <param name="ex">给定的异常。</param>
        /// <returns>返回消息字符串。</returns>
        public static string AsInnerMessage(this Exception ex)
        {
            if (ex.InnerException.IsNotNull())
                return ex.InnerException.AsInnerMessage();

            return ex.Message;
        }


        #region NotNull

        /// <summary>
        /// 得到不为 NULL 或默认值。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> 为空或默认值。
        /// </exception>
        /// <typeparam name="TSource">指定的源类型。</typeparam>
        /// <param name="source">给定的源。</param>
        /// <param name="paramName">给定的参数名。</param>
        /// <returns>返回源或抛出异常。</returns>
        public static TSource NotNull<TSource>(this TSource source, string paramName)
            where TSource : class
        {
            return source ?? throw new ArgumentNullException(paramName);
        }


        /// <summary>
        /// 得到不为 NULL 或空字符串的字符串。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="str"/> 为空或空字符串。
        /// </exception>
        /// <param name="str">给定的字符串。</param>
        /// <param name="paramName">给定的参数名。</param>
        /// <returns>返回字符串或抛出异常。</returns>
        public static string NotNullOrEmpty(this string str, string paramName)
        {
            return !str.IsNullOrEmpty() ? str : throw new ArgumentNullException(paramName);
        }

        /// <summary>
        /// 得到不为 NULL 或空集合的集合。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="sources"/> 为空或空集合。
        /// </exception>
        /// <typeparam name="TSource">指定的源类型。</typeparam>
        /// <param name="sources">给定的 <see cref="IEnumerable{TSource}"/>。</param>
        /// <param name="paramName">给定的参数名。</param>
        /// <returns>返回集合或抛出异常。</returns>
        public static IEnumerable<TSource> NotNullOrEmpty<TSource>(this IEnumerable<TSource> sources, string paramName)
        {
            return !sources.IsNullOrEmpty() ? sources : throw new ArgumentNullException(paramName);
        }

        /// <summary>
        /// 得到不为 NULL 或 EMPTY 的列表。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// items 为 DEFAULT 或空列表。
        /// </exception>
        /// <typeparam name="T">指定的元素类型。</typeparam>
        /// <param name="items">给定的元素集合。</param>
        /// <param name="paramName">给定的参数名。</param>
        /// <returns>返回集合或抛出异常。</returns>
        public static IList<T> NotNullOrEmpty<T>(this IList<T> items, string paramName)
        {
            if (items.IsNullOrEmpty())
                throw new ArgumentNullException(paramName);

            return items;
        }

        /// <summary>
        /// 得到不为 NULL 或 EMPTY 的数组。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// items 为 DEFAULT 或空数组。
        /// </exception>
        /// <typeparam name="T">指定的元素类型。</typeparam>
        /// <param name="items">给定的元素集合。</param>
        /// <param name="paramName">给定的参数名。</param>
        /// <returns>返回集合或抛出异常。</returns>
        public static T[] NotNullOrEmpty<T>(this T[] items, string paramName)
        {
            if (items.IsNullOrEmpty())
                throw new ArgumentNullException(paramName);

            return items;
        }

        #endregion


        #region Compare

        /// <summary>
        /// 得到不大于（等于）对比值。
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// value is (equal or) greater than.
        /// </exception>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="value">给定的值。</param>
        /// <param name="compare">给定的比较值。</param>
        /// <param name="paramName">给定的参数名。</param>
        /// <param name="equals">是否比较等于（可选；默认不比较）。</param>
        /// <returns>返回值或抛出异常。</returns>
        public static T NotGreater<T>(this T value, T compare, string paramName, bool equals = false)
            where T : IComparable<T>
        {
            if (value.IsGreater(compare, equals))
            {
                var message = $"The \"{paramName}\" value \"{value}\" is (equal or) greater than \"{compare}\".";
                throw new ArgumentOutOfRangeException(paramName, message);
            }

            return value;
        }

        /// <summary>
        /// 得到不小于（等于）的值。
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// value is (equal or) lesser than.
        /// </exception>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="value">给定的值。</param>
        /// <param name="compare">给定的比较值。</param>
        /// <param name="paramName">给定的参数名。</param>
        /// <param name="equals">是否比较等于（可选；默认不比较）。</param>
        /// <returns>返回值或抛出异常。</returns>
        public static T NotLesser<T>(this T value, T compare, string paramName, bool equals = false)
            where T : IComparable<T>
        {
            if (value.IsLesser(compare, equals))
            {
                var message = $"The \"{paramName}\" value \"{value}\" is (equal or) lesser than \"{compare}\".";
                throw new ArgumentOutOfRangeException(paramName, message);
            }

            return value;
        }

        /// <summary>
        /// 得到不超出范围的值。
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// value is out of range.
        /// </exception>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="value">给定的值。</param>
        /// <param name="compareMinimum">给定的最小比较值。</param>
        /// <param name="compareMaximum">给定的最大比较值。</param>
        /// <param name="paramName">给定的参数名。</param>
        /// <param name="equalMinimum">是否比较等于最小值（可选；默认不比较）。</param>
        /// <param name="equalMaximum">是否比较等于最大值（可选；默认不比较）。</param>
        /// <returns>返回值或抛出异常。</returns>
        public static T NotOutOfRange<T>(this T value, T compareMinimum, T compareMaximum, string paramName,
            bool equalMinimum = false, bool equalMaximum = false)
            where T : IComparable<T>
        {
            if (value.IsOutOfRange(compareMinimum, compareMaximum, equalMinimum, equalMaximum))
            {
                var message = $"The \"{paramName}\" value \"{value}\" is out of range (min: \"{compareMinimum}\", max: \"{compareMaximum}\").";
                throw new ArgumentOutOfRangeException(paramName, message);
            }

            return value;
        }

        #endregion


        /// <summary>
        /// 得到文件存在的路径。
        /// </summary>
        /// <exception cref="FileNotFoundException">
        /// path 文件不存在。
        /// </exception>
        /// <param name="path">给定的文件路径。</param>
        /// <returns>返回路径字符串或抛出异常。</returns>
        public static string FileExists(this string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException(path);

            return path;
        }

        /// <summary>
        /// 得到目录存在的路径。
        /// </summary>
        /// <exception cref="FileNotFoundException">
        /// path 目录不存在。
        /// </exception>
        /// <param name="path">给定的目录路径。</param>
        /// <returns>返回路径字符串或抛出异常。</returns>
        public static string DirectoryExists(this string path)
        {
            if (!Directory.Exists(path))
                throw new DirectoryNotFoundException(path);

            return path;
        }


        /// <summary>
        /// 可从目标类型分配，同时返回目标类型。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="baseType"/> 或 <paramref name="targetType"/> 为空。
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="baseType"/> 不能从 <paramref name="targetType"/> 分配。
        /// </exception>
        /// <param name="baseType">给定的基础类型。</param>
        /// <param name="targetType">给定的目标类型。</param>
        /// <returns>返回目标类型或抛出异常。</returns>
        public static Type AssignableFromTarget(this Type baseType, Type targetType)
        {
            if (!baseType.IsAssignableFromTarget(targetType))
            {
                var message = $"The \"{baseType.ToString()}\" is not assignable from \"{targetType.ToString()}\".";
                throw new ArgumentException(message);
            }

            return targetType;
        }

        /// <summary>
        /// 可以分配给基础类型，同时返回目标类型。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="targetType"/> 或 <paramref name="baseType"/> 为空。
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="targetType"/> 不能分配给 <paramref name="baseType"/>。
        /// </exception>
        /// <param name="targetType">给定的目标类型。</param>
        /// <param name="baseType">给定的基础类型。</param>
        /// <returns>返回目标类型或抛出异常。</returns>
        public static Type AssignableToBase(this Type targetType, Type baseType)
        {
            return baseType.AssignableFromTarget(targetType);
        }


        /// <summary>
        /// 来源类型实例是目标类型实例。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> 为空。
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="source"/> 不是目标类型。
        /// </exception>
        /// <typeparam name="TSource">指定的来源类型。</typeparam>
        /// <typeparam name="TTarget">指定的目标类型。</typeparam>
        /// <param name="source">给定的来源实例。</param>
        /// <param name="paramName">给定的参数名。</param>
        /// <returns>返回目标类型实例或抛出异常。</returns>
        public static TTarget IsValue<TSource, TTarget>(this TSource source, string paramName)
        {
            if (source == null) // 未限定源类型为 Class
                throw new ArgumentNullException(paramName);

            if (!(source is TTarget target))
            {
                var sourceType = source?.GetType() ?? typeof(TSource);
                var message = $"The \"{sourceType.FullName}\" is not require type \"{typeof(TTarget).FullName}\".";
                throw new ArgumentException(message);
            }

            return target;
        }

    }
}
