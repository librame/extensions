#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions;
using Librame.Extensions.Core;
using Microsoft.Extensions.Logging;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 核心构建器服务集合静态扩展。
    /// </summary>
    public static class CoreBuilderServiceCollectionExtensions
    {
        /// <summary>
        /// 添加 Librame。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="setupAction">给定的选项配置动作（可选）。</param>
        /// <param name="setupLoggingAction">给定的日志配置动作（可选）。</param>
        /// <returns>返回 <see cref="ICoreBuilder"/>。</returns>
        public static ICoreBuilder AddLibrame(this IServiceCollection services,
            Action<CoreBuilderOptions> setupAction = null,
            Action<ILoggingBuilder> setupLoggingAction = null)
        {
            return services.AddLibrame(s => new InternalCoreBuilder(s), setupAction,
                setupLoggingAction);
        }

        /// <summary>
        /// 添加 Librame。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="createFactory">给定创建核心构建器的工厂方法。</param>
        /// <param name="setupAction">给定的选项配置动作（可选）。</param>
        /// <param name="setupLoggingAction">给定的日志配置动作（可选）。</param>
        /// <returns>返回 <see cref="ICoreBuilder"/>。</returns>
        public static ICoreBuilder AddLibrame(this IServiceCollection services,
            Func<IServiceCollection, ICoreBuilder> createFactory,
            Action<CoreBuilderOptions> setupAction = null,
            Action<ILoggingBuilder> setupLoggingAction = null)
        {
            createFactory.NotNull(nameof(createFactory));

            // Add Dependencies
            services
                .AddOptions()
                .AddLocalization(options =>
                {
                    options.ResourcesPath = "Resources";
                })
                .AddLogging(setupLoggingAction ?? (_ => { }));

            services.OnlyConfigure(setupAction);

            var coreBuilder = createFactory.Invoke(services);

            return coreBuilder
                .AddConverters()
                .AddLocalizations()
                .AddMediators()
                .AddServices()
                .AddValidators();
        }

    }
}
