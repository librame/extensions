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

namespace Microsoft.Extensions.DependencyInjection
{
    using Configuration;

    /// <summary>
    /// 核心构建器服务集合静态扩展。
    /// </summary>
    public static class CoreBuilderServiceCollectionExtensions
    {
        /// <summary>
        /// 添加 Librame。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="configureOptions">给定的 <see cref="Action{BuilderOptions}"/>（可选；高优先级）。</param>
        /// <param name="configuration">给定的 <see cref="IConfiguration"/>（可选；次优先级）。</param>
        /// <param name="configureBinderOptions">给定的配置绑定器选项动作（可选）。</param>
        /// <returns>返回 <see cref="ICoreBuilder"/>。</returns>
        public static ICoreBuilder AddLibrame(this IServiceCollection services,
            Action<CoreBuilderOptions> configureOptions = null,
            IConfiguration configuration = null,
            Action<BinderOptions> configureBinderOptions = null)
        {
            var options = services.ConfigureBuilder(configureOptions,
                configuration, configureBinderOptions);

            // AddLocalization
            services.AddLocalization(options.ConfigureLocalization);

            // AddLogging
            services.AddLogging(options.ConfigureLogging);

            var builder = new InternalCoreBuilder(services, options);

            return builder
                .AddConverters()
                .AddLocalizations()
                .AddMediators()
                .AddServices()
                .AddValidators();
        }

    }
}
