#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.DependencyInjection;
using System;

namespace Librame.Builders
{
    using Extensions;

    /// <summary>
    /// 构建器服务集合静态扩展。
    /// </summary>
    public static class BuilderServiceCollectionExtensions
    {

        /// <summary>
        /// 预配置构建器依赖集合。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="configureOptions">给定的配置依赖选项（可选）。</param>
        /// <returns>返回 <see cref="IServiceCollection"/>。</returns>
        public static IServiceCollection PreConfigureBuilderDependencies(this IServiceCollection services,
            Action<DependenciesOptions> configureOptions = null)
        {
            services.AddOptions();

            var options = configureOptions.Newly();

            // Add Logging
            if (options.ConfigureLogging.IsDefault())
                services.AddLogging();
            else
                services.AddLogging(options.ConfigureLogging);

            // Add Localization
            if (options.ConfigureLocalization.IsDefault())
                services.AddLocalization();
            else
                services.AddLocalization(options.ConfigureLocalization);

            return services;
        }


        /// <summary>
        /// 添加 Librame 服务集合。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="configureOptions">给定的配置依赖选项（可选）。</param>
        /// <returns>返回 <see cref="IBuilder"/>。</returns>
        public static IBuilder AddLibrame(this IServiceCollection services,
            Action<DependenciesOptions> configureOptions = null)
        {
            services.PreConfigureBuilderDependencies(configureOptions);

            var builder = services.AsDefaultBuilder();

            return builder.AddBuffers()
                .AddConverters()
                .AddResources();
        }


        /// <summary>
        /// 转换为默认构建器。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <returns>返回 <see cref="IBuilder"/>。</returns>
        public static IBuilder AsDefaultBuilder(this IServiceCollection services)
        {
            return new DefaultBuilder(services);
        }

    }
}
