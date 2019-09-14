#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 核心构建器依赖选项。
    /// </summary>
    public class CoreBuilderDependencyOptions : ExtensionBuilderDependencyOptions<CoreBuilderOptions>
    {
        /// <summary>
        /// <see cref="LocalizationOptions"/> 配置动作。
        /// </summary>
        public Action<LocalizationOptions> LocalizationOptionsAction { get; set; }
            = options => options.ResourcesPath = "Resources";

        /// <summary>
        /// <see cref="ILoggingBuilder"/> 配置动作。
        /// </summary>
        public Action<ILoggingBuilder> LoggingBuilderAction { get; set; }
            = _ => { };

        /// <summary>
        /// <see cref="MemoryCacheOptions"/> 配置动作。
        /// </summary>
        public Action<MemoryCacheOptions> MemoryCacheOptionsAction { get; set; }
            = _ => { };
    }
}
