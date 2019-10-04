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
    public class CoreBuilderDependencyOptions : ExtensionBuilderDependencyOptions<CoreBuilderDependencyOptions, CoreBuilderOptions>
    {
        /// <summary>
        /// 本地化选项配置器（服务注册时手动注入配置动作）。
        /// </summary>
        public OptionsActionConfigurator<LocalizationOptions> Localization { get; }
            = new OptionsActionConfigurator<LocalizationOptions>(options => options.ResourcesPath = "Resources", autoConfigureAction: false);

        /// <summary>
        /// 内存缓存选项配置器（服务注册时手动注入配置动作）。
        /// </summary>
        public OptionsActionConfigurator<MemoryCacheOptions> MemoryCache { get; }
            = new OptionsActionConfigurator<MemoryCacheOptions>(autoConfigureAction: false);

        /// <summary>
        /// 内存分布式缓存选项配置器（服务注册时手动注入配置动作）。
        /// </summary>
        public OptionsActionConfigurator<MemoryDistributedCacheOptions> MemoryDistributedCache { get; }
            = new OptionsActionConfigurator<MemoryDistributedCacheOptions>(autoConfigureAction: false);


        /// <summary>
        /// <see cref="ILoggingBuilder"/> 配置动作。
        /// </summary>
        public Action<ILoggingBuilder> LoggingAction { get; set; }
            = _ => { };
    }
}
