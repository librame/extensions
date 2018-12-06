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
        /// <returns>返回是否包含的布尔值。</returns>
        public static bool IsAttribute<TAttribute>(this ICustomAttributeProvider provider, bool inherit = false)
            where TAttribute : Attribute
        {
            return provider.IsDefined(typeof(TAttribute), inherit);
        }


        /// <summary>
        /// 得到特性。
        /// </summary>
        /// <typeparam name="TAttribute">指定的特性类型。</typeparam>
        /// <param name="provider">指定的 <see cref="ICustomAttributeProvider"/>。</param>
        /// <param name="inherit">指定是否搜索该成员的继承链以查找这些特性。</param>
        /// <returns>返回自定义特性对象。</returns>
        public static TAttribute AsAttribute<TAttribute>(this ICustomAttributeProvider provider, bool inherit = false)
            where TAttribute : Attribute
        {
            var attributes = provider.GetCustomAttributes(typeof(TAttribute), inherit);

            return attributes.IsNotEmpty() ? (TAttribute)attributes[0] : default;
        }
        

        /// <summary>
        /// 尝试得到特性。
        /// </summary>
        /// <typeparam name="TAttribute">指定的特性类型。</typeparam>
        /// <param name="provider">指定的 <see cref="ICustomAttributeProvider"/>。</param>
        /// <param name="attribute">输出包含的特性。</param>
        /// <param name="inherit">指定是否搜索该成员的继承链以查找这些特性。</param>
        /// <returns>返回是否包含的布尔值。</returns>
        public static bool TryAsAttribute<TAttribute>(this ICustomAttributeProvider provider, out TAttribute attribute, bool inherit = false)
            where TAttribute : Attribute
        {
            attribute = provider.AsAttribute<TAttribute>(inherit);

            return attribute.IsNotDefault();
        }

    }
}
