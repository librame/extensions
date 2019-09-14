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
        /// <param name="loggingAction">给定的日志构建器配置动作。</param>
        /// <param name="builderFactory">给定创建核心构建器的工厂方法（可选）。</param>
        /// <returns>返回 <see cref="ICoreBuilder"/>。</returns>
        public static ICoreBuilder AddLibrame(this IServiceCollection services,
            Action<ILoggingBuilder> loggingAction,
            Func<IServiceCollection, ICoreBuilder> builderFactory = null)
        {
            loggingAction.NotNull(nameof(loggingAction));

            return services.AddLibrame(dependency =>
            {
                dependency.LoggingBuilderAction = loggingAction;
            },
            builderFactory);
        }

        /// <summary>
        /// 添加 Librame。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="builderAction">给定的选项配置动作。</param>
        /// <param name="builderFactory">给定创建核心构建器的工厂方法（可选）。</param>
        /// <returns>返回 <see cref="ICoreBuilder"/>。</returns>
        public static ICoreBuilder AddLibrame(this IServiceCollection services,
            Action<CoreBuilderOptions> builderAction,
            Func<IServiceCollection, ICoreBuilder> builderFactory = null)
        {
            builderAction.NotNull(nameof(builderAction));

            return services.AddLibrame(dependency =>
            {
                dependency.BuilderOptionsAction = builderAction;
            },
            builderFactory);
        }

        /// <summary>
        /// 添加 Librame。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="dependencyAction">给定的依赖选项配置动作（可选）。</param>
        /// <param name="builderFactory">给定创建核心构建器的工厂方法（可选）。</param>
        /// <returns>返回 <see cref="ICoreBuilder"/>。</returns>
        public static ICoreBuilder AddLibrame(this IServiceCollection services,
            Action<CoreBuilderDependencyOptions> dependencyAction = null,
            Func<IServiceCollection, ICoreBuilder> builderFactory = null)
        {
            // Add Dependencies
            var dependency = dependencyAction.ConfigureDependencyOptions();

            services
                .AddOptions()
                .AddLocalization(dependency.LocalizationOptionsAction)
                .AddLogging(dependency.LoggingBuilderAction)
                .AddMemoryCache(dependency.MemoryCacheOptionsAction);

            // Add Builder
            services.OnlyConfigure(dependency.BuilderOptionsAction,
                dependency.BuilderOptionsName);

            var coreBuilder = builderFactory.NotNullOrDefault(()
                => s => new CoreBuilder(s)).Invoke(services);
            
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
