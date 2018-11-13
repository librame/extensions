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
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;

namespace Librame.Builders
{
    /// <summary>
    /// 依赖集合选项。
    /// </summary>
    public class DependenciesOptions
    {
        /// <summary>
        /// 配置日志构建器（可选）。
        /// </summary>
        public Action<ILoggingBuilder> ConfigureLogging { get; set; }

        /// <summary>
        /// 配置本地化选项（可选）。
        /// </summary>
        public Action<LocalizationOptions> ConfigureLocalization { get; set; }


        /// <summary>
        /// 替换日志工厂。
        /// </summary>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        public void ReplaceLoggerFactory(ILoggerFactory loggerFactory)
        {
            ReplaceLoggerFactory(services => services.Replace(ServiceDescriptor.Singleton(loggerFactory)));
        }

        /// <summary>
        /// 替换日志工厂。
        /// </summary>
        /// <param name="implementationFactory">给定的 <see cref="ILoggerFactory"/> 解析工厂方法。</param>
        public void ReplaceLoggerFactory(Func<IServiceProvider, ILoggerFactory> implementationFactory)
        {
            ReplaceLoggerFactory(services => services.Replace(ServiceDescriptor.Singleton(implementationFactory)));
        }

        private void ReplaceLoggerFactory(Action<IServiceCollection> configureServices)
        {
            var copy = ConfigureLogging;

            ConfigureLogging = builder =>
            {
                configureServices.Invoke(builder.Services);

                copy?.Invoke(builder);
            };
        }

    }
}
