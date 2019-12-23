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
using Librame.Extensions.Encryption.Builders;
using Librame.Extensions.Network.Builders;
using Librame.Extensions.Network.DotNetty;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// DotNetty 网络构建器静态扩展。
    /// </summary>
    public static class DotNettyNetworkBuilderExtensions
    {
        /// <summary>
        /// 添加 DotNetty 扩展。
        /// </summary>
        /// <param name="baseBuilder">给定的基础 <see cref="IExtensionBuilder"/>。</param>
        /// <param name="configureOptions">给定的选项配置动作。</param>
        /// <returns>返回 <see cref="INetworkBuilder"/>。</returns>
        public static INetworkBuilder AddDotNetty(this INetworkBuilder baseBuilder,
            Action<DotNettyOptions> configureOptions)
        {
            configureOptions.NotNull(nameof(configureOptions));

            return baseBuilder.AddDotNetty(dependency =>
            {
                dependency.Builder.ConfigureOptions = configureOptions;
            });
        }

        /// <summary>
        /// 添加 DotNetty 扩展。
        /// </summary>
        /// <param name="baseBuilder">给定的基础 <see cref="INetworkBuilder"/>。</param>
        /// <param name="configureDependency">给定的配置依赖动作方法（可选）。</param>
        /// <returns>返回 <see cref="INetworkBuilder"/>。</returns>
        public static INetworkBuilder AddDotNetty(this INetworkBuilder baseBuilder,
            Action<DotNettyDependency> configureDependency = null)
            => baseBuilder.AddDotNetty<DotNettyDependency>(configureDependency);

        /// <summary>
        /// 添加 DotNetty 扩展。
        /// </summary>
        /// <typeparam name="TDependencyOptions">指定的依赖类型。</typeparam>
        /// <param name="baseBuilder">给定的基础 <see cref="INetworkBuilder"/>。</param>
        /// <param name="configureDependency">给定的配置依赖动作方法（可选）。</param>
        /// <returns>返回 <see cref="INetworkBuilder"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "baseBuilder")]
        public static INetworkBuilder AddDotNetty<TDependencyOptions>(this INetworkBuilder baseBuilder,
            Action<TDependencyOptions> configureDependency = null)
            where TDependencyOptions : DotNettyDependency, new()
        {
            baseBuilder.NotNull(nameof(baseBuilder));

            // Configure Dependency
            var dependency = configureDependency.ConfigureDependency(baseBuilder);

            // Add Dependencies
            if (!baseBuilder.ContainsParentBuilder<IEncryptionBuilder>())
                baseBuilder.AddEncryption().AddDeveloperGlobalSigningCredentials();

            // Configure Builder
            return baseBuilder
                .AddDotNettyDependency(dependency)
                .AddWrappers()
                .AddDemo();
        }

        private static INetworkBuilder AddWrappers(this INetworkBuilder builder)
        {
            builder.Services.TryAddScoped<IBootstrapWrapperFactory, BootstrapWrapperFactory>();
            builder.Services.TryAddScoped<IBootstrapWrapper, BootstrapWrapper>();
            builder.Services.TryAddScoped<IServerBootstrapWrapper, ServerBootstrapWrapper>();
            builder.Services.TryAddScoped(typeof(IBootstrapWrapper<,>), typeof(BootstrapWrapper<,>));

            return builder;
        }

    }
}
