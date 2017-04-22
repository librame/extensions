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

namespace Librame.Utility
{
    /// <summary>
    /// 键名构建器。
    /// </summary>
    public class KeyBuilder : LibrameBase<KeyBuilder>
    {
        /// <summary>
        /// 构建键名。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <returns>返回字符串。</returns>
        public static string BuildKey<T>()
        {
            return BuildKey(typeof(T));
        }

        /// <summary>
        /// 构建键名。
        /// </summary>
        /// <param name="type">给定的类型。</param>
        /// <returns>返回字符串。</returns>
        public static string BuildKey(Type type)
        {
            type.GuardNull(nameof(type));

            return TypeUtility.AssemblyQualifiedNameWithoutVCP(type);
        }


        /// <summary>
        /// 构建可格式化的键名。
        /// </summary>
        /// <param name="key">给定的键名。</param>
        /// <returns>返回可格式化键名。</returns>
        public static string BuildFormatKey(string key)
        {
            if (string.IsNullOrEmpty(key))
                return string.Empty;

            return string.Format("$({0})", key);
        }

        /// <summary>
        /// 格式化键值。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <param name="formatKey">给定用于格式化的键名。</param>
        /// <param name="formatValue">给定用于格式化的键值。</param>
        /// <returns>返回字符串。</returns>
        public static string FormatKeyValue(string str, string formatKey, string formatValue)
        {
            if (string.IsNullOrEmpty(str))
                return string.Empty;

            if (string.IsNullOrEmpty(formatKey))
                return str;

            return str.Replace(formatKey, formatValue);
        }

    }
}
