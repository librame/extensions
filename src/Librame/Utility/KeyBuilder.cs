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
            type.NotNull(nameof(type));

            return TypeUtility.AssemblyQualifiedNameWithoutVCP(type);
        }


        /// <summary>
        /// 构建可格式化的键名。
        /// </summary>
        /// <param name="name">给定的名称。</param>
        /// <returns>返回可格式化键名。</returns>
        public static string BuildFormatKey(string name)
        {
            name.NotNullOrEmpty(nameof(name));

            // 如果已是可格式化的键名
            if (name.StartsWith("$(") && name.EndsWith(")"))
                return name;

            return string.Format("$({0})", name);
        }


        /// <summary>
        /// 是否包含指定可格式化键名。
        /// </summary>
        /// <param name="template">要格式化的模板内容。</param>
        /// <param name="formatKey">给定的可格式化键名。</param>
        /// <returns>返回布尔值。</returns>
        public static bool ContainsFormatKey(string template, string formatKey)
        {
            return template.Contains(formatKey.NotNullOrEmpty(nameof(formatKey)));
        }


        /// <summary>
        /// 格式化模板内容。
        /// </summary>
        /// <param name="template">要格式化的模板内容。</param>
        /// <param name="formatDictionary">给定要格式化的字典集合。</param>
        /// <returns>返回经过格式化的模板内容。</returns>
        public static string Formatting(string template, IDictionary<string, string> formatDictionary)
        {
            formatDictionary.NotNull(nameof(formatDictionary));
            
            formatDictionary.Invoke(pair =>
            {
                template = Formatting(template, pair.Key, pair.Value);
            });

            return template;
        }


        /// <summary>
        /// 格式化键值。
        /// </summary>
        /// <param name="template">要格式化的模板内容。</param>
        /// <param name="name">用于格式化的名称。</param>
        /// <param name="value">用于格式化的值。</param>
        /// <returns>返回字符串。</returns>
        public static string Formatting(string template, string name, string value)
        {
            if (string.IsNullOrEmpty(template))
                return string.Empty;

            if (string.IsNullOrEmpty(name))
                return template;
            
            value.NotNull(nameof(value));

            //// 转义可格式化键名
            //var escapedFormatKey = EscapeFormatKey(formatKey);

            // 尝试构建可格式化键名
            name = BuildFormatKey(name);

            // 开始替换
            return template.Replace(name, value);
        }

    }
}
