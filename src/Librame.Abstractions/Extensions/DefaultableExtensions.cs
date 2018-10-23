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


        /// <summary>
        /// 当作值或默认值（如果值为空、空字符串或空白字符）。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <param name="defaultValue">给定的默认值。</param>
        /// <returns>返回字符串。</returns>
        public static string AsValueOrDefault(this string str, string defaultValue)
        {
            return !string.IsNullOrWhiteSpace(str) ? str : defaultValue;
        }
        /// <summary>
        /// 当作值或默认值（如果值为空、空字符串或空白字符）。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <param name="defaultValueFactory">给定的默认值工厂方法。</param>
        /// <returns>返回字符串。</returns>
        public static string AsValueOrDefault(this string str, Func<string> defaultValueFactory)
        {
            return !string.IsNullOrWhiteSpace(str) ? str : defaultValueFactory.Invoke();
        }

    }
}
