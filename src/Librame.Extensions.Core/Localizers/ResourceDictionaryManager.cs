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
using System.Globalization;
using System.Reflection;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 资源字典管理器。
    /// </summary>
    public class ResourceDictionaryManager
    {
        /// <summary>
        /// 构造一个 <see cref="ResourceDictionaryManager"/>。
        /// </summary>
        /// <param name="resourceBaseType">给定的资源基础类型。</param>
        /// <param name="resourceAssembly">给定资源程序集（可选；默认为资源基础类型所在的程序集）。</param>
        public ResourceDictionaryManager(Type resourceBaseType, Assembly resourceAssembly = null)
        {
            ResourceBaseType = resourceBaseType.NotNull(nameof(resourceBaseType));
            ResourceAssembly = resourceAssembly ?? resourceBaseType.Assembly;
        }


        /// <summary>
        /// 资源基础类型。
        /// </summary>
        protected Type ResourceBaseType { get; }

        /// <summary>
        /// 资源程序集。
        /// </summary>
        protected Assembly ResourceAssembly { get; }


        /// <summary>
        /// 创建资源字典。
        /// </summary>
        /// <returns>返回 <see cref="IResourceDictionary"/>。</returns>
        public virtual IResourceDictionary CreateDictionary()
        {
            var type = GetDictionaryType();
            return (IResourceDictionary)type.EnsureCreateObject();
        }

        /// <summary>
        /// 获取资源字典类型。
        /// </summary>
        /// <returns>返回类型。</returns>
        protected virtual Type GetDictionaryType()
        {
            var suffixName = CultureInfo.CurrentUICulture.Name.Replace('-', '_');
            return Type.GetType($"{ResourceBaseType.FullName}_{suffixName}, {ResourceAssembly}");
        }
    }
}
