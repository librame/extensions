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
using System.Text;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 核心构建器选项。
    /// </summary>
    public class CoreBuilderOptions : AbstractExtensionBuilderOptions, IEncoding
    {
        private static readonly CultureInfo _zhCNCultureInfo
            = new CultureInfo("zh-CN");


        /// <summary>
        /// 文化信息（默认为 zh-CN）。
        /// </summary>
        public CultureInfo CultureInfo { get; set; }
            = _zhCNCultureInfo;

        /// <summary>
        /// UI 文化信息（默认为 zh-CN）。
        /// </summary>
        public CultureInfo CultureUIInfo { get; set; }
            = _zhCNCultureInfo;


        /// <summary>
        /// 字符编码（默认为 UTF8）。
        /// </summary>
        public Encoding Encoding { get; set; }
            = Encoding.UTF8;


        /// <summary>
        /// 是 UTC 时钟。
        /// </summary>
        public bool IsUtcClock { get; set; }


        /// <summary>
        /// 资源映射工厂方法。
        /// </summary>
        public Func<ResourceMappingDescriptor, string> ResourceMappingFactory { get; set; }
            = descr =>
            {
                if (descr.RelativePath.IsNullOrEmpty())
                    return $"{descr.BaseNamespace}.{descr.TypeInfo.Name}";

                // _resourcesRelativePath 已格式化为点分隔符（如：Resources.）
                return $"{descr.BaseNamespace}.{descr.RelativePath}{descr.TypeInfo.Name}";
            };
    }
}
