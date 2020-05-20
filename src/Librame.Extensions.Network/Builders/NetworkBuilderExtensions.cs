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
using Librame.Extensions.Core.Options;
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
        /// <param name="parentBuilder">给定的父级 <see cref="IExtensionBuilder"/>。</param>
        /// <param name="configureDependency">给定的配置依赖动作方法（可选）。</param>
        /// <param name="builderFactory">给定创建网络构建器的工厂方法（可选）。</param>
        /// <returns>返回 <see cref="INetworkBuilder"/>。</returns>
        public static INetworkBuilder AddNetwork(this IExtensionBuilder parentBuilder,
            Action<NetworkBuilderDependency> configureDependency = null,
            Func<IExtensionBuilder, NetworkBuilderDependency, INetworkBuilder> builderFactory = null)
            => parentBuilder.AddNetwork<NetworkBuilderDependency>(configureDependency, builderFactory);

        /// <summary>
        /// 添加网络扩展。
        /// </summary>
        /// <typeparam name="TDependency">指定的依赖类型。</typeparam>
        /// <param name="parentBuilder">给定的父级 <see cref="IExtensionBuilder"/>。</param>
        /// <param name="configureDependency">给定的配置依赖动作方法（可选）。</param>
        /// <param name="builderFactory">给定创建网络构建器的工厂方法（可选）。</param>
        /// <returns>返回 <see cref="INetworkBuilder"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static INetworkBuilder AddNetwork<TDependency>(this IExtensionBuilder parentBuilder,
            Action<TDependency> configureDependency = null,
            Func<IExtensionBuilder, TDependency, INetworkBuilder> builderFactory = null)
            where TDependency : NetworkBuilderDependency
        {
            if (!parentBuilder.ContainsParentBuilder<IEncryptionBuilder>())
            {
                parentBuilder
                    .AddEncryption()
                    .AddDeveloperGlobalSigningCredentials();
            }

            // Clear Options Cache
            ConsistencyOptionsCache.TryRemove<NetworkBuilderOptions>();

            // Add Builder Dependency
            var dependency = parentBuilder.AddBuilderDependency(out var dependencyType, configureDependency);
            parentBuilder.Services.TryAddReferenceBuilderDependency<NetworkBuilderDependency>(dependency, dependencyType);

            // Add Dependencies
            parentBuilder.Services
                .AddHttpClient();

            // Create Builder
            return builderFactory.NotNullOrDefault(()
                => (b, d) => new NetworkBuilder(b, d)).Invoke(parentBuilder, dependency);
        }

    }
}
