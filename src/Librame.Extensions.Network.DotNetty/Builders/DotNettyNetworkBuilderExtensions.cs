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
using Librame.Extensions.Encryption;
using Librame.Extensions.Network;
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
        /// <param name="builder">给定的 <see cref="IExtensionBuilder"/>。</param>
        /// <param name="builderAction">给定的选项配置动作。</param>
        /// <returns>返回 <see cref="INetworkBuilder"/>。</returns>
        public static INetworkBuilder AddDotNetty(this INetworkBuilder builder,
            Action<DotNettyOptions> builderAction)
        {
            builderAction.NotNull(nameof(builderAction));

            return builder.AddDotNetty(dependency =>
            {
                dependency.Builder.Action = builderAction;
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
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "builder")]
        public static INetworkBuilder AddDotNetty<TDependencyOptions>(this INetworkBuilder builder,
            Action<TDependencyOptions> dependencyAction = null)
            where TDependencyOptions : DotNettyDependencyOptions, new()
        {
            builder.NotNull(nameof(builder));

            // Configure DependencyOptions
            var dependency = dependencyAction.ConfigureDependency();
            builder.Services.AddAllOptionsConfigurators(dependency);

            // Add EncryptionBuilder
            if (!builder.HasParentBuilder<IEncryptionBuilder>())
                builder.AddEncryption().AddDeveloperGlobalSigningCredentials();

            // Configure Builder
            return builder
                .AddDotNettyDependencyOptions(dependency)
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
