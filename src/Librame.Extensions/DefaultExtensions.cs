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
    /// 默认静态扩展。
    /// </summary>
    public static class DefaultExtensions
    {

        #region HasOrDefault

        /// <summary>
        /// 具有（不为 NULL 或空字符串）或默认。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <param name="defaultValue">给定的默认值。</param>
        /// <returns>返回字符串。</returns>
        public static string HasOrDefault(this string str, string defaultValue)
        {
            return string.IsNullOrEmpty(str) ? defaultValue : str;
        }
        /// <summary>
        /// 具有（不为 NULL 或空字符串）或默认。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <param name="defaultValueFactory">给定的默认值工厂方法。</param>
        /// <returns>返回字符串。</returns>
        public static string HasOrDefault(this string str, Func<string> defaultValueFactory)
        {
            return string.IsNullOrEmpty(str) ? defaultValueFactory?.Invoke() : str;
        }

        ///// <summary>
        ///// 具有（不为 NULL）或默认。
        ///// </summary>
        ///// <typeparam name="TValue">指定的值类型。</typeparam>
        ///// <param name="value">给定的值。</param>
        ///// <param name="defaultValue">给定的默认值。</param>
        ///// <returns>返回值或默认值。</returns>
        //public static TValue HasOrDefault<TValue>(this TValue value, TValue defaultValue)
        //    where TValue : class
        //{
        //    return value.HasOrDefault(() => defaultValue);
        //}
        ///// <summary>
        ///// 具有（不为 NULL）或默认。
        ///// </summary>
        ///// <typeparam name="TValue">指定的值类型。</typeparam>
        ///// <param name="value">给定的值。</param>
        ///// <param name="defaultValueFactory">给定的默认值工厂方法。</param>
        ///// <returns>返回值或默认值。</returns>
        //public static TValue HasOrDefault<TValue>(this TValue value, Func<TValue> defaultValueFactory)
        //    where TValue : class
        //{
        //    return null == value ? defaultValueFactory?.Invoke() : value;
        //}

        /// <summary>
        /// 具有（可空有值）或默认值。
        /// </summary>
        /// <typeparam name="TValue">指定的值类型。</typeparam>
        /// <param name="nullable">给定的可空值。</param>
        /// <param name="defaultValue">给定的默认值（如果可空值为空，则返回此值）。</param>
        /// <returns>返回值或默认值。</returns>
        public static TValue HasOrDefault<TValue>(this TValue? nullable, TValue defaultValue)
            where TValue : struct
        {
            return nullable.HasValue ? nullable.Value : defaultValue;
        }
        /// <summary>
        /// 具有（可空有值）或默认值。
        /// </summary>
        /// <typeparam name="TValue">指定的值类型。</typeparam>
        /// <param name="nullable">给定的可空值。</param>
        /// <param name="defaultValueFactory">给定的默认值工厂方法（如果可空值为空，则调用此方法）。</param>
        /// <returns>返回值或默认值。</returns>
        public static TValue HasOrDefault<TValue>(this TValue? nullable, Func<TValue> defaultValueFactory)
            where TValue : struct
        {
            return nullable.HasValue ? nullable.Value : defaultValueFactory.Invoke();
        }

        #endregion

    }
}
