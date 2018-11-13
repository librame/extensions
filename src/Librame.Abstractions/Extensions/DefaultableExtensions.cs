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

namespace Librame.Extensions
{
    /// <summary>
    /// 可默认静态扩展。
    /// </summary>
    public static class DefaultableExtensions
    {

        /// <summary>
        /// 当作值或默认值（如果值为空、空字符串或空白字符）。
        /// </summary>
        /// <param name="num">给定的字符串。</param>
        /// <param name="defaultValue">给定的默认值。</param>
        /// <param name="useDefaultValueBy">给定的使用默认值条件（可选；默认当条件等于零时成立）。</param>
        /// <returns>返回字符串。</returns>
        public static long AsValueOrDefault(this long num, long defaultValue, long useDefaultValueBy = 0L)
        {
            return num.AsValueOrDefault(defaultValue, n => n == useDefaultValueBy);
        }
        /// <summary>
        /// 当作值或默认值（如果值为空、空字符串或空白字符）。
        /// </summary>
        /// <param name="num">给定的字符串。</param>
        /// <param name="defaultValue">给定的默认值。</param>
        /// <param name="useDefaultValueBy">给定的使用默认值条件（可选；默认当条件等于零时成立）。</param>
        /// <returns>返回字符串。</returns>
        public static int AsValueOrDefault(this int num, int defaultValue, int useDefaultValueBy = 0)
        {
            return num.AsValueOrDefault(defaultValue, n => n == useDefaultValueBy);
        }

        /// <summary>
        /// 当作值或默认值（如果值为空、空字符串或空白字符）。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <param name="defaultValue">给定的默认值。</param>
        /// <returns>返回字符串。</returns>
        public static string AsValueOrDefault(this string str, string defaultValue)
        {
            return str.AsValueOrDefault(defaultValue, s => s.IsWhiteSpace());
        }
        /// <summary>
        /// 当作值或默认值（如果值为空、空字符串或空白字符）。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <param name="defaultValueFactory">给定的默认值工厂方法。</param>
        /// <returns>返回字符串。</returns>
        public static string AsValueOrDefault(this string str, Func<string> defaultValueFactory)
        {
            return str.AsValueOrDefault(defaultValueFactory, s => s.IsWhiteSpace());
        }
        
        /// <summary>
        /// 当作值或默认值。
        /// </summary>
        /// <typeparam name="TValue">指定的值类型。</typeparam>
        /// <param name="value">给定的值。</param>
        /// <param name="defaultValue">给定的默认值。</param>
        /// <param name="useDefaultValueFactory">断定使用默认值的工厂方法（可选；默认使用 <see cref="ValidationExtensions.IsDefault{T}(T)"/>）验证。</param>
        /// <returns>返回值或默认值。</returns>
        public static TValue AsValueOrDefault<TValue>(this TValue value, TValue defaultValue,
            Func<TValue, bool> useDefaultValueFactory = null)
        {
            return value.AsValueOrDefault(() => defaultValue, useDefaultValueFactory);
        }
        /// <summary>
        /// 当作值或默认值。
        /// </summary>
        /// <typeparam name="TValue">指定的值类型。</typeparam>
        /// <param name="value">给定的值。</param>
        /// <param name="defaultValueFactory">给定的默认值工厂方法。</param>
        /// <param name="useDefaultValueFactory">断定使用默认值的工厂方法（可选；默认使用 <see cref="ValidationExtensions.IsDefault{T}(T)"/>）验证。</param>
        /// <returns>返回值或默认值。</returns>
        public static TValue AsValueOrDefault<TValue>(this TValue value, Func<TValue> defaultValueFactory,
            Func<TValue, bool> useDefaultValueFactory = null)
        {
            if (useDefaultValueFactory.IsDefault())
                useDefaultValueFactory = v => v.IsDefault();

            if (useDefaultValueFactory.Invoke(value))
                return defaultValueFactory.Invoke();

            return value;
        }


        /// <summary>
        /// 当作值或默认值（如果值为空）。
        /// </summary>
        /// <typeparam name="TValue">指定的值类型。</typeparam>
        /// <param name="nullable">给定的可空实例。</param>
        /// <param name="defaultValue">给定的默认值。</param>
        /// <returns>返回实例。</returns>
        public static TValue AsValueOrDefault<TValue>(this TValue? nullable, TValue defaultValue)
            where TValue : struct
        {
            return nullable.HasValue ? nullable.Value : defaultValue;
        }
        /// <summary>
        /// 当作值或默认值（如果值为空）。
        /// </summary>
        /// <typeparam name="TValue">指定的值类型。</typeparam>
        /// <param name="nullable">给定的可空实例。</param>
        /// <param name="defaultValueFactory">给定的默认值工厂方法。</param>
        /// <returns>返回实例。</returns>
        public static TValue AsValueOrDefault<TValue>(this TValue? nullable, Func<TValue> defaultValueFactory)
            where TValue : struct
        {
            return nullable.HasValue ? nullable.Value : defaultValueFactory.Invoke();
        }

    }
}
