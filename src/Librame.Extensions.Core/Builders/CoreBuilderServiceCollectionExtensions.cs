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
        /// <param name="dependencyAction">给定的依赖选项配置动作（可选）。</param>
        /// <param name="createFactory">给定创建核心构建器的工厂方法（可选）。</param>
        /// <returns>返回 <see cref="ICoreBuilder"/>。</returns>
        public static ICoreBuilder AddLibrame(this IServiceCollection services,
            Action<CoreBuilderDependencyOptions> dependencyAction = null,
            Func<IServiceCollection, ICoreBuilder> createFactory = null)
        {
            // Add Dependencies
            var dependencyOptions = dependencyAction.ConfigureDependencyOptions();

            services
                .AddOptions()
                .AddLocalization(dependencyOptions.LocalizationAction)
                .AddLogging(dependencyOptions.LoggingAction)
                .AddMemoryCache(dependencyOptions.MemoryCacheAction);

            // Add Builder
            services.OnlyConfigure(dependencyOptions.SetupAction);

            var coreBuilder = (createFactory ??
                (s => new CoreBuilder(s))).Invoke(services);

            return coreBuilder
                .AddLocalizations()
                .AddMediators()
                .AddServices();
        }


        /// <summary>
        /// 添加分布式缓存。
        /// </summary>
        /// <param name="builder">给定的 <see cref="ICoreBuilder"/>。</param>
        /// <param name="setupAction">给定的配置动作。</param>
        /// <returns>返回 <see cref="ICoreBuilder"/>。</returns>
        public static ICoreBuilder AddDistributedCache(this ICoreBuilder builder, Action<IServiceCollection> setupAction)
        {
            setupAction.NotNull(nameof(setupAction)).Invoke(builder.Services);
            return builder;
        }

    }
}
