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

namespace Microsoft.Extensions.Localization
{
    /// <summary>
    /// 资源映射特性。
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly |
        AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Interface,
        AllowMultiple = false, Inherited = false)]
    public class ResourceMappingAttribute : Attribute
    {
        /// <summary>
        /// 构造一个 <see cref="ResourceMappingAttribute"/> 实例。
        /// </summary>
        /// <remarks>
        /// 如果启用，则使用 <see cref="PrefixFactory"/> 设定加载资源文件；反之，则使用原始规则加载资源文件。
        /// </remarks>
        /// <param name="enabled">是否启用此特性（可选；默认启用）。</param>
        public ResourceMappingAttribute(bool enabled = true)
            : base()
        {
            Enabled = enabled;
        }


        /// <summary>
        /// 启用特性。
        /// </summary>
        public bool Enabled { get; }

        /// <summary>
        /// 前缀工厂方法（可选；输入参数集合依次为 BaseNamespace、[ResourceRelativeLocation]、TypeInfo）。
        /// </summary>
        public Func<string, string, TypeInfo, string> PrefixFactory { get; set; }
    }
}
