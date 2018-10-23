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
        /// 得到此成员信息包含的自定义特性。
        /// </summary>
        /// <typeparam name="TAttribute">指定的特性类型。</typeparam>
        /// <param name="member">指定的成员信息。</param>
        /// <param name="inherit">指定是否搜索该成员的继承链以查找这些特性。</param>
        /// <returns>返回自定义特性对象。</returns>
        public static TAttribute Attribute<TAttribute>(this MemberInfo member, bool inherit = false)
            where TAttribute : Attribute
        {
            return member.GetCustomAttribute<TAttribute>(inherit);
        }


        /// <summary>
        /// 包含指定特性。
        /// </summary>
        /// <typeparam name="TAttribute">指定的特性类型。</typeparam>
        /// <param name="member">指定的成员信息。</param>
        /// <param name="inherit">指定是否搜索该成员的继承链以查找这些特性。</param>
        /// <returns>返回是否包含的布尔值。</returns>
        public static bool HasAttribute<TAttribute>(this MemberInfo member, bool inherit = false)
            where TAttribute : Attribute
        {
            return member.HasAttribute(out TAttribute attribute, inherit);
        }
        /// <summary>
        /// 包含指定特性。
        /// </summary>
        /// <typeparam name="TAttribute">指定的特性类型。</typeparam>
        /// <param name="member">指定的成员信息。</param>
        /// <param name="attribute">输出包含的特性。</param>
        /// <param name="inherit">指定是否搜索该成员的继承链以查找这些特性。</param>
        /// <returns>返回是否包含的布尔值。</returns>
        public static bool HasAttribute<TAttribute>(this MemberInfo member, out TAttribute attribute, bool inherit = false)
            where TAttribute : Attribute
        {
            attribute = member.Attribute<TAttribute>(inherit);

            return attribute.IsNotDefault();
        }

    }
}
