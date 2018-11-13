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

namespace Librame.Resources
{
    /// <summary>
    /// 资源前缀特性。
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
    public class ResourcePrefixAttribute : Attribute
    {
        /// <summary>
        /// 构造一个 <see cref="ResourcePrefixAttribute"/> 实例。
        /// </summary>
        /// <param name="useEnhanced">使用增强模式。</param>
        public ResourcePrefixAttribute(bool useEnhanced)
            : base()
        {
            UseEnhanced = useEnhanced;
        }


        /// <summary>
        /// 使用增强模式。
        /// </summary>
        public bool UseEnhanced { get; }
    }
}
