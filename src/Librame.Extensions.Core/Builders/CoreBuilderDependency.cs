#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;

namespace Librame.Extensions.Core.Builders
{
    using Dependencies;

    /// <summary>
    /// 核心构建器依赖。
    /// </summary>
    public class CoreBuilderDependency : AbstractExtensionBuilderDependency<CoreBuilderOptions>
    {
        /// <summary>
        /// 构造一个 <see cref="CoreBuilderDependency"/>。
        /// </summary>
        public CoreBuilderDependency()
            : this(nameof(CoreBuilderDependency))
        {
        }

        /// <summary>
        /// 构造一个 <see cref="CoreBuilderDependency"/>。
        /// </summary>
        /// <param name="name">给定的名称。</param>
        protected CoreBuilderDependency(string name)
            : base(name)
        {
            TokensDirectory = ConfigDirectory.CombinePath(CoreSettings.Preference.TokensFolder);
        }


        /// <summary>
        /// 本地化选项依赖。
        /// </summary>
        public OptionsDependency<LocalizationOptions> Localization { get; }
            = new OptionsDependency<LocalizationOptions>();

        /// <summary>
        /// 内存缓存选项依赖。
        /// </summary>
        public OptionsDependency<MemoryCacheOptions> MemoryCache { get; }
            = new OptionsDependency<MemoryCacheOptions>();

        /// <summary>
        /// 内存分布式缓存选项依赖。
        /// </summary>
        public OptionsDependency<MemoryDistributedCacheOptions> MemoryDistributedCache { get; }
            = new OptionsDependency<MemoryDistributedCacheOptions>();


        /// <summary>
        /// 配置 <see cref="ILoggingBuilder"/>。
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public Action<ILoggingBuilder> ConfigureLoggingBuilder { get; set; }
            = _ => { };


        /// <summary>
        /// 令牌目录。
        /// </summary>
        public string TokensDirectory { get; set; }
    }
}
