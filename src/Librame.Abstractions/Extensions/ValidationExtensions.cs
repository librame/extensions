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
using System.Linq;

namespace Librame.Extensions
{
    using Infrastructures;

    /// <summary>
    /// 验证静态扩展。
    /// </summary>
    public static class ValidationExtensions
    {

        /// <summary>
        /// 是否为 DEFAULT（值类型）或 NULL（引用类型）。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="instance">给定的实例。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsDefault<T>(this T instance)
        {
            if (instance is IDefaultable defaultable)
                return defaultable.IsDefaulting();
            
            if (typeof(T).IsValueType)
                return default(T).Equals(instance);

            return null == instance;
        }

        /// <summary>
        /// 是否不为 DEFAULT（值类型）或 NULL（引用类型）。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="instance">给定的实例。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsNotDefault<T>(this T instance)
        {
            return !instance.IsDefault();
        }


        /// <summary>
        /// 是否为 DEFAULT、空字符串或仅由空字符组成的字符串。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsWhiteSpace(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        /// <summary>
        /// 是否不为 DEFAULT、空字符串或仅由空字符组成的字符串。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsNotWhiteSpace(this string str)
        {
            return !str.IsWhiteSpace();
        }


        /// <summary>
        /// 是否为 NULL 或 EMPTY。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        /// <summary>
        /// 是否不为 NULL 或 EMPTY。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsNotEmpty(this string str)
        {
            return !str.IsEmpty();
        }


        /// <summary>
        /// 是否为 NULL 或 EMPTY。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="enumerable">给定的 <see cref="IEnumerable{T}"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsEmpty<T>(this IEnumerable<T> enumerable)
        {
            return enumerable.IsDefault() || !enumerable.Any();
        }

        /// <summary>
        /// 是否不为 NULL 或 EMPTY。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="enumerable">给定的 <see cref="IEnumerable{T}"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsNotEmpty<T>(this IEnumerable<T> enumerable)
        {
            return enumerable.IsNotDefault() && enumerable.Any();
        }


        /// <summary>
        /// 是否大于（等于）对比值。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="value">给定的值。</param>
        /// <param name="compare">给定的比较值。</param>
        /// <param name="equals">是否比较等于（可选；默认不比较）。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsGreater<T>(this T value, T compare, bool equals = false)
            where T : IComparable<T>
        {
            return equals ? value.CompareTo(compare) >= 0 : value.CompareTo(compare) > 0;
        }

        /// <summary>
        /// 是否小于（等于）对比值。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="value">给定的值。</param>
        /// <param name="compare">给定的比较值。</param>
        /// <param name="equals">是否比较等于（可选；默认不比较）。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsLesser<T>(this T value, T compare, bool equals = false)
            where T : IComparable<T>
        {
            return equals ? value.CompareTo(compare) <= 0 : value.CompareTo(compare) < 0;
        }

        /// <summary>
        /// 是否超出（等于）范围对比值。
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
            return value.IsLesser(compareMinimum, equalMinimum) || value.IsGreater(compareMaximum, equalMaximum);
        }

        /// <summary>
        /// 是否为倍数。
        /// </summary>
        /// <param name="value">给定的数字。</param>
        /// <param name="multiples">给定的倍数。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsMultiples(this int value, int multiples)
        {
            return value % multiples == 0;
        }


        /// <summary>
        /// 是否为 <see cref="Nullable{T}"/> 类型。
        /// </summary>
        /// <param name="type">给定的类型。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsNullableType(this Type type)
        {
            return type.IsNotDefault() && type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(Nullable<>));
        }


        /// <summary>
        /// 是否为字符串类型。
        /// </summary>
        /// <param name="type">给定的类型。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsStringType(this Type type)
        {
            return type == typeof(string);
        }

    }
}
