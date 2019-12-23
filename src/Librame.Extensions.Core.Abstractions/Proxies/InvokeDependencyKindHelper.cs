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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;

namespace Librame.Extensions.Core.Proxies
{
    /// <summary>
    /// 调用依赖种类助手。
    /// </summary>
    public static class InvokeDependencyKindHelper
    {
        private static readonly ConcurrentDictionary<string, IEnumerable<Attribute>> _attributes
            = typeof(InvokeDependencyKind).AsEnumAttributesDictionary();


        /// <summary>
        /// 尝试获取指定种类名称的默认值特性。
        /// </summary>
        /// <param name="name">给定的种类名称。</param>
        /// <param name="defaultValue">输出默认值。</param>
        /// <returns>返回布尔值。</returns>
        public static bool TryGetDefaultValue(string name, out string defaultValue)
        {
            if (TryGetAttribute(name, out DefaultValueAttribute attribute))
            {
                defaultValue = (string)attribute.Value;
                return true;
            }

            defaultValue = null;
            return false;
        }

        /// <summary>
        /// 尝试获取指定种类名称的特性。
        /// </summary>
        /// <typeparam name="TAttribute">指定的特性类型。</typeparam>
        /// <param name="name">给定的种类名称。</param>
        /// <param name="attribute">输出 <typeparamref name="TAttribute"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool TryGetAttribute<TAttribute>(string name, out TAttribute attribute)
            where TAttribute : Attribute
        {
            foreach (var attrib in _attributes[name])
            {
                if (attrib is TAttribute _attribute)
                {
                    attribute = _attribute;
                    return true;
                }
            }

            attribute = null;
            return false;
        }

    }
}
