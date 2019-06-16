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
        private static byte[] _locker = new byte[0];

        /// <summary>
        /// 确保单例。
        /// </summary>
        /// <typeparam name="TSingleton">指定的单例类型。</typeparam>
        /// <param name="singleton">给定的当前单例。</param>
        /// <param name="buildFactory">用于构建单例的工厂方法。</param>
        /// <returns>返回单例。</returns>
        public static TSingleton EnsureSingleton<TSingleton>(this TSingleton singleton, Func<TSingleton> buildFactory)
            where TSingleton : class
        {
            if (singleton.IsNull())
            {
                lock (_locker)
                {
                    if (singleton.IsNull())
                        singleton = buildFactory?.Invoke();
                }
            }

            return singleton.NotNull(nameof(singleton));
        }


        /// <summary>
        /// 确保字符串（如果当前字符串为 NULL 或空字符串，则返回默认值）。
        /// </summary>
        /// <param name="str">给定的当前字符串。</param>
        /// <param name="defaultValue">给定的默认值。</param>
        /// <returns>返回字符串。</returns>
        public static string EnsureValue(this string str, string defaultValue)
        {
            return string.IsNullOrEmpty(str) ? defaultValue : str;
        }
        /// <summary>
        /// 确保字符串（如果当前字符串为 NULL 或空字符串，则返回默认值）。
        /// </summary>
        /// <param name="str">给定的当前字符串。</param>
        /// <param name="defaultValueFactory">给定的默认值工厂方法。</param>
        /// <returns>返回字符串。</returns>
        public static string EnsureValue(this string str, Func<string> defaultValueFactory)
        {
            return string.IsNullOrEmpty(str) ? defaultValueFactory?.Invoke() : str;
        }


        /// <summary>
        /// 确保可空值（如果当前可空值为 NULL，则返回默认值）。
        /// </summary>
        /// <typeparam name="TValue">指定的值类型。</typeparam>
        /// <param name="nullable">给定的当前可空值。</param>
        /// <param name="defaultValue">给定的默认值（如果可空值为空，则返回此值）。</param>
        /// <returns>返回值或默认值。</returns>
        public static TValue EnsureValue<TValue>(this TValue? nullable, TValue defaultValue)
            where TValue : struct
        {
            return nullable.HasValue ? nullable.Value : defaultValue;
        }
        /// <summary>
        /// 确保可空值（如果当前可空值为 NULL，则返回默认值）。
        /// </summary>
        /// <typeparam name="TValue">指定的值类型。</typeparam>
        /// <param name="nullable">给定的当前可空值。</param>
        /// <param name="defaultValueFactory">给定的默认值工厂方法（如果可空值为空，则调用此方法）。</param>
        /// <returns>返回值或默认值。</returns>
        public static TValue EnsureValue<TValue>(this TValue? nullable, Func<TValue> defaultValueFactory)
            where TValue : struct
        {
            return nullable.HasValue ? nullable.Value : defaultValueFactory.Invoke();
        }

    }
}
