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
using System.Reflection;

namespace Librame.Extensions
{
    /// <summary>
    /// 特性静态扩展。
    /// </summary>
    public static class AttributeExtensions
    {
        /// <summary>
        /// 是否定义特性。
        /// </summary>
        /// <typeparam name="TAttribute">指定的特性类型。</typeparam>
        /// <param name="provider">指定的 <see cref="ICustomAttributeProvider"/>。</param>
        /// <param name="inherit">指定是否搜索该成员的继承链以查找这些特性。</param>
        /// <returns>返回是否已定义此特性的布尔值。</returns>
        public static bool IsDefined<TAttribute>(this ICustomAttributeProvider provider, bool inherit = false)
            where TAttribute : Attribute
        {
            return provider.IsDefined(typeof(TAttribute), inherit);
        }


        #region GetCustomAttributes

        /// <summary>
        /// 得到特性。
        /// </summary>
        /// <typeparam name="TAttribute">指定的特性类型。</typeparam>
        /// <param name="provider">指定的 <see cref="ICustomAttributeProvider"/>。</param>
        /// <param name="inherit">指定是否搜索该成员的继承链以查找这些特性。</param>
        /// <returns>返回自定义特性。</returns>
        public static TAttribute GetCustomAttribute<TAttribute>(this ICustomAttributeProvider provider, bool inherit = false)
            where TAttribute : Attribute
        {
            var attributes = provider.GetCustomAttributes(typeof(TAttribute), inherit);

            return !attributes.IsNullOrEmpty() ? (TAttribute)attributes[0] : null;
        }
        /// <summary>
        /// 得到特性集合。
        /// </summary>
        /// <typeparam name="TAttribute">指定的特性类型。</typeparam>
        /// <param name="provider">指定的 <see cref="ICustomAttributeProvider"/>。</param>
        /// <param name="inherit">指定是否搜索该成员的继承链以查找这些特性。</param>
        /// <returns>返回自定义特性集合。</returns>
        public static IEnumerable<TAttribute> GetCustomAttributes<TAttribute>(this ICustomAttributeProvider provider, bool inherit = false)
            where TAttribute : Attribute
        {
            var attributes = provider.GetCustomAttributes(typeof(TAttribute), inherit);

            return !attributes.IsNullOrEmpty() ? attributes.Select(attrib => (TAttribute)attrib) : null;
        }

        /// <summary>
        /// 尝试得到特性。
        /// </summary>
        /// <typeparam name="TAttribute">指定的特性类型。</typeparam>
        /// <param name="provider">指定的 <see cref="ICustomAttributeProvider"/>。</param>
        /// <param name="attribute">输出得到的自定义特性。</param>
        /// <param name="inherit">指定是否搜索该成员的继承链以查找这些特性。</param>
        /// <returns>返回是否得到自定义特性的布尔值。</returns>
        public static bool TryGetCustomAttribute<TAttribute>(this ICustomAttributeProvider provider,
            out TAttribute attribute, bool inherit = false)
            where TAttribute : Attribute
        {
            attribute = provider.GetCustomAttribute<TAttribute>(inherit);

            return null != attribute;
        }
        /// <summary>
        /// 尝试得到特性集合。
        /// </summary>
        /// <typeparam name="TAttribute">指定的特性类型。</typeparam>
        /// <param name="provider">指定的 <see cref="ICustomAttributeProvider"/>。</param>
        /// <param name="attributes">输出得到的自定义特性。</param>
        /// <param name="inherit">指定是否搜索该成员的继承链以查找这些特性。</param>
        /// <returns>返回是否得到自定义特性集合的布尔值。</returns>
        public static bool TryGetCustomAttributes<TAttribute>(this ICustomAttributeProvider provider,
            out IEnumerable<TAttribute> attributes, bool inherit = false)
            where TAttribute : Attribute
        {
            attributes = provider.GetCustomAttributes<TAttribute>(inherit);

            return null != attributes;
        }

        #endregion

    }
}
