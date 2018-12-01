﻿#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Builders;
using Librame.Extensions;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 构建器服务集合静态扩展。
    /// </summary>
    public static class BuilderServiceCollectionExtensions
    {

        /// <summary>
        /// 添加 Librame 服务集合。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="configureOptions">给定的配置依赖选项（可选）。</param>
        /// <returns>返回 <see cref="IBuilder"/>。</returns>
        public static IBuilder AddLibrame(this IServiceCollection services,
            Action<DependenciesOptions> configureOptions = null)
        {
            services.AddPreLibrameDependencies(configureOptions);

            var builder = services.AsBuilder();

            return builder
                .AddConverters()
                .AddResources();
        }


        /// <summary>
        /// 添加前置 Librame 依赖集合。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="configureOptions">给定的配置依赖选项（可选）。</param>
        /// <returns>返回 <see cref="IServiceCollection"/>。</returns>
        public static IServiceCollection AddPreLibrameDependencies(this IServiceCollection services,
            Action<DependenciesOptions> configureOptions = null)
        {
            // Add Options
            services.AddOptions();

            var options = new DependenciesOptions();
            configureOptions?.Invoke(options);

            // Add Localization
            if (options.ConfigureLocalization.IsDefault())
                services.AddLocalization();
            else
                services.AddLocalization(options.ConfigureLocalization);

            // Add Logging
            if (options.ConfigureLogging.IsDefault())
                services.AddLogging();
            else
                services.AddLogging(options.ConfigureLogging);

            return services;
        }
        
        /// <summary>
        /// 转换为构建器。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <returns>返回 <see cref="IBuilder"/>。</returns>
        public static IBuilder AsBuilder(this IServiceCollection services)
        {
            return new Builder(services);
        }

    }
}
