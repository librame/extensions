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

namespace Librame.Extensions.Network.DotNetty
{
    using Core;
    using Encryption;

    /// <summary>
    /// DotNetty 网络构建器静态扩展。
    /// </summary>
    public static class DotNettyNetworkBuilderExtensions
    {
        /// <summary>
        /// 添加 DotNetty 扩展。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IExtensionBuilder"/>。</param>
        /// <param name="builderAction">给定的选项配置动作。</param>
        /// <returns>返回 <see cref="INetworkBuilder"/>。</returns>
        public static INetworkBuilder AddDotNetty(this INetworkBuilder builder,
            Action<DotNettyOptions> builderAction)
        {
            builderAction.NotNull(nameof(builderAction));

            return builder.AddDotNetty(dependency =>
            {
                dependency.BuilderOptionsAction = builderAction;
            });
        }

        /// <summary>
        /// 添加 DotNetty 扩展。
        /// </summary>
        /// <param name="builder">给定的 <see cref="INetworkBuilder"/>。</param>
        /// <param name="dependencyAction">给定的依赖选项配置动作（可选）。</param>
        /// <returns>返回 <see cref="INetworkBuilder"/>。</returns>
        public static INetworkBuilder AddDotNetty(this INetworkBuilder builder,
            Action<DotNettyDependencyOptions> dependencyAction = null)
        {
            // Add Dependencies
            var dependency = dependencyAction.ConfigureDependencyOptions();

            builder.Services.OnlyConfigure(dependency.BuilderOptionsAction,
                dependency.BuilderOptionsName);

            // 如果未添加加密扩展，则自动添加并默认配置
            if (!builder.HasParentBuilder<IEncryptionBuilder>())
                builder.AddEncryption().AddDeveloperGlobalSigningCredentials();

            return builder
                .AddWrappers()
                .AddDemo();
        }

        private static INetworkBuilder AddWrappers(this INetworkBuilder builder)
        {
            builder.Services.AddSingleton<IBootstrapWrapperFactory, BootstrapWrapperFactory>();
            builder.Services.AddSingleton<IBootstrapWrapper, BootstrapWrapper>();
            builder.Services.AddSingleton<IServerBootstrapWrapper, ServerBootstrapWrapper>();
            builder.Services.AddSingleton(typeof(IBootstrapWrapper<,>), typeof(BootstrapWrapper<,>));

            return builder;
        }

    }
}
