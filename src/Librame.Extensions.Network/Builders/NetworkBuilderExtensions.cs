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

namespace Librame.Extensions.Network
{
    using Core;

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
            Func<IExtensionBuilder, INetworkBuilder> builderFactory = null)
        {
            builderAction.NotNull(nameof(builderAction));

            return builder.AddNetwork(dependency =>
            {
                dependency.BuilderOptionsAction = builderAction;
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
            Func<IExtensionBuilder, INetworkBuilder> builderFactory = null)
        {
            // Add Dependencies
            var dependency = dependencyAction.ConfigureDependencyOptions();

            builder.Services.AddHttpClient();

            // Add Builder
            builder.Services.OnlyConfigure(dependency.BuilderOptionsAction,
                dependency.BuilderOptionsName);

            var networkBuilder = builderFactory.NotNullOrDefault(()
                => b => new NetworkBuilder(b)).Invoke(builder);

            return networkBuilder
                .AddRequesters()
                .AddServices();
        }

    }
}
