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
            Func<IServiceCollection, CoreBuilderDependencyOptions, ICoreBuilder> builderFactory = null)
        {
            loggingAction.NotNull(nameof(loggingAction));

            return services.AddLibrame(dependency =>
            {
                dependency.LoggingAction = loggingAction;
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
            Func<IServiceCollection, CoreBuilderDependencyOptions, ICoreBuilder> builderFactory = null)
        {
            builderAction.NotNull(nameof(builderAction));

            return services.AddLibrame(dependency =>
            {
                dependency.OptionsAction = builderAction;
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
            Func<IServiceCollection, CoreBuilderDependencyOptions, ICoreBuilder> builderFactory = null)
            => services.AddLibrame<CoreBuilderDependencyOptions>(dependencyAction, builderFactory);

        /// <summary>
        /// 添加 Librame。
        /// </summary>
        /// <typeparam name="TDependencyOptions">指定的依赖类型。</typeparam>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="dependencyAction">给定的依赖选项配置动作（可选）。</param>
        /// <param name="builderFactory">给定创建核心构建器的工厂方法（可选）。</param>
        /// <returns>返回 <see cref="ICoreBuilder"/>。</returns>
        public static ICoreBuilder AddLibrame<TDependencyOptions>(this IServiceCollection services,
            Action<TDependencyOptions> dependencyAction = null,
            Func<IServiceCollection, TDependencyOptions, ICoreBuilder> builderFactory = null)
            where TDependencyOptions : CoreBuilderDependencyOptions, new()
        {
            // Add Dependencies
            var dependency = dependencyAction.ConfigureDependencyOptions();

            services
                .AddOptions()
                .AddLocalization(dependency.LocalizationAction)
                .AddLogging(dependency.LoggingAction)
                .AddMemoryCache(dependency.MemoryCacheAction)
                .AddDistributedMemoryCache(dependency.MemoryDistributedCacheAction);

            // Add Builder
            services.OnlyConfigure(dependency.OptionsAction, dependency.OptionsName);

            var coreBuilder = builderFactory.NotNullOrDefault(()
                => (s, d) => new CoreBuilder(s, d)).Invoke(services, dependency);

            return coreBuilder
                .AddLocalizers()
                .AddMediators()
                .AddServices()
                .AddThreads();
        }

    }
}
