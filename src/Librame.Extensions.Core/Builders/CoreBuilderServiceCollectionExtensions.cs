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
using Librame.Extensions.Core.Builders;
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
        /// <param name="configureLoggingBuilder">给定的配置日志构建器动作方法。</param>
        /// <param name="builderFactory">给定创建核心构建器的工厂方法（可选）。</param>
        /// <returns>返回 <see cref="ICoreBuilder"/>。</returns>
        public static ICoreBuilder AddLibrame(this IServiceCollection services,
            Action<ILoggingBuilder> configureLoggingBuilder,
            Func<IServiceCollection, CoreBuilderDependency, ICoreBuilder> builderFactory = null)
        {
            configureLoggingBuilder.NotNull(nameof(configureLoggingBuilder));

            return services.AddLibrame(dependency =>
            {
                dependency.ConfigureLoggingBuilder = configureLoggingBuilder;
            },
            builderFactory);
        }

        /// <summary>
        /// 添加 Librame。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="configureDependency">给定的配置依赖动作方法（可选）。</param>
        /// <param name="builderFactory">给定创建核心构建器的工厂方法（可选）。</param>
        /// <returns>返回 <see cref="ICoreBuilder"/>。</returns>
        public static ICoreBuilder AddLibrame(this IServiceCollection services,
            Action<CoreBuilderDependency> configureDependency = null,
            Func<IServiceCollection, CoreBuilderDependency, ICoreBuilder> builderFactory = null)
            => services.AddLibrame<CoreBuilderDependency>(configureDependency, builderFactory);

        /// <summary>
        /// 添加 Librame。
        /// </summary>
        /// <typeparam name="TDependency">指定的依赖类型。</typeparam>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="configureDependency">给定的配置依赖动作方法（可选）。</param>
        /// <param name="builderFactory">给定创建核心构建器的工厂方法（可选）。</param>
        /// <returns>返回 <see cref="ICoreBuilder"/>。</returns>
        public static ICoreBuilder AddLibrame<TDependency>(this IServiceCollection services,
            Action<TDependency> configureDependency = null,
            Func<IServiceCollection, TDependency, ICoreBuilder> builderFactory = null)
            where TDependency : CoreBuilderDependency, new()
        {
            // Use BootstrapperStarter
            services.UseBootstrapperStarter();

            // Configure Dependency
            var dependency = configureDependency.ConfigureDependencyRoot(services);
            
            // Add Dependencies
            services
                .AddOptions()
                .AddLogging(dependency.ConfigureLoggingBuilder)
                .AddLocalization()
                .AddMemoryCache()
                .AddDistributedMemoryCache();

            // Create Builder
            var coreBuilder = builderFactory.NotNullOrDefault(()
                => (s, d) => new CoreBuilder(s, d)).Invoke(services, dependency);

            // Configure Builder
            return coreBuilder
                .AddDecorators()
                .AddLocalizers()
                .AddMediators()
                .AddOptions()
                .AddServices()
                .AddThreads();
        }

    }
}
