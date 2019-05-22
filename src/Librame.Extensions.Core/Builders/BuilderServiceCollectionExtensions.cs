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
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 构建器服务集合静态扩展。
    /// </summary>
    public static class BuilderServiceCollectionExtensions
    {
        /// <summary>
        /// 添加 Librame。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="configureLocalization">给定的配置本地化动作（可选）。</param>
        /// <param name="configureLogging">给定的配置日志动作（可选）。</param>
        /// <returns>返回 <see cref="IBuilder"/>。</returns>
        public static IBuilder AddLibrame(this IServiceCollection services,
            Action<LocalizationOptions> configureLocalization = null,
            Action<ILoggingBuilder> configureLogging = null)
        {
            if (null == configureLocalization)
            {
                configureLocalization = options =>
                {
                    options.ResourcesPath = "Resources";
                };
            }

            // Add Dependencies
            services.AddOptions();
            services.AddLocalization(configureLocalization);
            services.AddLogging(configureLogging ?? (_ => { }));

            var builder = new InternalBuilder(services);

            return builder
                .AddConverters()
                .AddLocalizations()
                .AddServices();
        }

    }
}
