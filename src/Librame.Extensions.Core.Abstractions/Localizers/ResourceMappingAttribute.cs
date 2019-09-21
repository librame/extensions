#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Core;
using System;

namespace Microsoft.Extensions.Localization
{
    /// <summary>
    /// 资源映射特性。
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly,
        AllowMultiple = false, Inherited = false)]
    public class ResourceMappingAttribute : AvailableAttribute
    {
        /// <summary>
        /// 构造一个 <see cref="ResourceMappingAttribute"/>。
        /// </summary>
        /// <param name="enabled">是否启用此特性（可选；默认启用）。</param>
        public ResourceMappingAttribute(bool enabled = true)
            : base(enabled)
        {
        }


        /// <summary>
        /// 工厂方法。
        /// </summary>
        public Func<ResourceMappingDescriptor, string> Factory { get; set; }
    }
}
