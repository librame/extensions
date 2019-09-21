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
                dependency.OptionsAction = builderAction;
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
            => builder.AddDotNetty<DotNettyDependencyOptions>(dependencyAction);

        /// <summary>
        /// 添加 DotNetty 扩展。
        /// </summary>
        /// <typeparam name="TDependencyOptions">指定的依赖类型。</typeparam>
        /// <param name="builder">给定的 <see cref="INetworkBuilder"/>。</param>
        /// <param name="dependencyAction">给定的依赖选项配置动作（可选）。</param>
        /// <returns>返回 <see cref="INetworkBuilder"/>。</returns>
        public static INetworkBuilder AddDotNetty<TDependencyOptions>(this INetworkBuilder builder,
            Action<TDependencyOptions> dependencyAction = null)
            where TDependencyOptions : DotNettyDependencyOptions, new()
        {
            // Add Dependencies
            var dependency = dependencyAction.ConfigureDependencyOptions();

            builder.Services.OnlyConfigure(dependency.OptionsAction,
                dependency.OptionsName);

            // 如果未添加加密扩展，则自动添加并默认配置
            if (!builder.HasParentBuilder<IEncryptionBuilder>())
                builder.AddEncryption().AddDeveloperGlobalSigningCredentials();

            return builder
                .AddWrappers()
                .AddDemo();
        }

        private static INetworkBuilder AddWrappers(this INetworkBuilder builder)
        {
            builder.Services.AddScoped<IBootstrapWrapperFactory, BootstrapWrapperFactory>();
            builder.Services.AddScoped<IBootstrapWrapper, BootstrapWrapper>();
            builder.Services.AddScoped<IServerBootstrapWrapper, ServerBootstrapWrapper>();
            builder.Services.AddScoped(typeof(IBootstrapWrapper<,>), typeof(BootstrapWrapper<,>));

            return builder;
        }

    }
}
