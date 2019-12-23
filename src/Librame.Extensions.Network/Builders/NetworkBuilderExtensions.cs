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
using System;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 网络构建器静态扩展。
    /// </summary>
    public static class NetworkBuilderExtensions
    {
        /// <summary>
        /// 添加网络扩展。
        /// </summary>
        /// <param name="baseBuilder">给定的基础 <see cref="IExtensionBuilder"/>。</param>
        /// <param name="configureOptions">给定的选项配置动作。</param>
        /// <param name="builderFactory">给定创建网络构建器的工厂方法（可选）。</param>
        /// <returns>返回 <see cref="INetworkBuilder"/>。</returns>
        public static INetworkBuilder AddNetwork(this IExtensionBuilder baseBuilder,
            Action<NetworkBuilderOptions> configureOptions,
            Func<IExtensionBuilder, NetworkBuilderDependency, INetworkBuilder> builderFactory = null)
        {
            configureOptions.NotNull(nameof(configureOptions));

            return baseBuilder.AddNetwork(dependency =>
            {
                dependency.Builder.ConfigureOptions = configureOptions;
            },
            builderFactory);
        }

        /// <summary>
        /// 添加网络扩展。
        /// </summary>
        /// <param name="baseBuilder">给定的基础 <see cref="IExtensionBuilder"/>。</param>
        /// <param name="configureDependency">给定的配置依赖动作方法（可选）。</param>
        /// <param name="builderFactory">给定创建网络构建器的工厂方法（可选）。</param>
        /// <returns>返回 <see cref="INetworkBuilder"/>。</returns>
        public static INetworkBuilder AddNetwork(this IExtensionBuilder baseBuilder,
            Action<NetworkBuilderDependency> configureDependency = null,
            Func<IExtensionBuilder, NetworkBuilderDependency, INetworkBuilder> builderFactory = null)
            => baseBuilder.AddNetwork<NetworkBuilderDependency>(configureDependency, builderFactory);

        /// <summary>
        /// 添加网络扩展。
        /// </summary>
        /// <typeparam name="TDependencyOptions">指定的依赖类型。</typeparam>
        /// <param name="baseBuilder">给定的基础 <see cref="IExtensionBuilder"/>。</param>
        /// <param name="configureDependency">给定的配置依赖动作方法（可选）。</param>
        /// <param name="builderFactory">给定创建网络构建器的工厂方法（可选）。</param>
        /// <returns>返回 <see cref="INetworkBuilder"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "baseBuilder")]
        public static INetworkBuilder AddNetwork<TDependencyOptions>(this IExtensionBuilder baseBuilder,
            Action<TDependencyOptions> configureDependency = null,
            Func<IExtensionBuilder, TDependencyOptions, INetworkBuilder> builderFactory = null)
            where TDependencyOptions : NetworkBuilderDependency, new()
        {
            if (!baseBuilder.ContainsParentBuilder<IEncryptionBuilder>())
            {
                baseBuilder
                    .AddEncryption()
                    .AddDeveloperGlobalSigningCredentials();
            }

            // Configure Dependency
            var dependency = configureDependency.ConfigureDependency(baseBuilder);

            // Add Dependencies
            baseBuilder.Services
                .AddHttpClient();

            // Create Builder
            var networkBuilder = builderFactory.NotNullOrDefault(()
                => (b, d) => new NetworkBuilder(b, d)).Invoke(baseBuilder, dependency);

            // Configure Builder
            return networkBuilder
                .AddRequesters()
                .AddServices();
        }

    }
}
