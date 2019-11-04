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
using Librame.Extensions.Network;
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
        /// <param name="builder">给定的 <see cref="IExtensionBuilder"/>。</param>
        /// <param name="builderAction">给定的选项配置动作。</param>
        /// <param name="builderFactory">给定创建网络构建器的工厂方法（可选）。</param>
        /// <returns>返回 <see cref="INetworkBuilder"/>。</returns>
        public static INetworkBuilder AddNetwork(this IExtensionBuilder builder,
            Action<NetworkBuilderOptions> builderAction,
            Func<IExtensionBuilder, NetworkBuilderDependencyOptions, INetworkBuilder> builderFactory = null)
        {
            builderAction.NotNull(nameof(builderAction));

            return builder.AddNetwork(dependency =>
            {
                dependency.Builder.Action = builderAction;
            },
            builderFactory);
        }

        /// <summary>
        /// 添加网络扩展。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IExtensionBuilder"/>。</param>
        /// <param name="dependencyAction">给定的依赖选项配置动作（可选）。</param>
        /// <param name="builderFactory">给定创建网络构建器的工厂方法（可选）。</param>
        /// <returns>返回 <see cref="INetworkBuilder"/>。</returns>
        public static INetworkBuilder AddNetwork(this IExtensionBuilder builder,
            Action<NetworkBuilderDependencyOptions> dependencyAction = null,
            Func<IExtensionBuilder, NetworkBuilderDependencyOptions, INetworkBuilder> builderFactory = null)
            => builder.AddNetwork<NetworkBuilderDependencyOptions>(dependencyAction, builderFactory);

        /// <summary>
        /// 添加网络扩展。
        /// </summary>
        /// <typeparam name="TDependencyOptions">指定的依赖类型。</typeparam>
        /// <param name="builder">给定的 <see cref="IExtensionBuilder"/>。</param>
        /// <param name="dependencyAction">给定的依赖选项配置动作（可选）。</param>
        /// <param name="builderFactory">给定创建网络构建器的工厂方法（可选）。</param>
        /// <returns>返回 <see cref="INetworkBuilder"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "builder")]
        public static INetworkBuilder AddNetwork<TDependencyOptions>(this IExtensionBuilder builder,
            Action<TDependencyOptions> dependencyAction = null,
            Func<IExtensionBuilder, TDependencyOptions, INetworkBuilder> builderFactory = null)
            where TDependencyOptions : NetworkBuilderDependencyOptions, new()
        {
            // Configure DependencyOptions
            var dependency = dependencyAction.ConfigureDependency();
            builder.Services.AddAllOptionsConfigurators(dependency);

            // Add Dependencies
            builder.Services
                .AddHttpClient();

            // Create Builder
            var networkBuilder = builderFactory.NotNullOrDefault(()
                => (b, d) => new NetworkBuilder(b, d)).Invoke(builder, dependency);

            // Configure Builder
            return networkBuilder
                .AddRequesters()
                .AddServices();
        }

    }
}
