#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Reflection;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 资源映射描述符。
    /// </summary>
    public class ResourceMappingDescriptor
    {
        /// <summary>
        /// 构造一个 <see cref="ResourceMappingDescriptor"/>。
        /// </summary>
        /// <param name="typeInfo">给定的类型信息。</param>
        /// <param name="baseNamespace">给定的基础命名空间。</param>
        /// <param name="relativePath">给定的相对路径。</param>
        public ResourceMappingDescriptor(TypeInfo typeInfo, string baseNamespace, string relativePath)
        {
            TypeInfo = typeInfo.NotNull(nameof(typeInfo));
            BaseNamespace = baseNamespace.NotEmpty(nameof(baseNamespace));
            RelativePath = relativePath;
        }


        /// <summary>
        /// 基础命名空间。
        /// </summary>
        public string BaseNamespace { get; }

        /// <summary>
        /// 相对路径。
        /// </summary>
        public string RelativePath { get; }

        /// <summary>
        /// 类型信息。
        /// </summary>
        public TypeInfo TypeInfo { get; }
    }
}
