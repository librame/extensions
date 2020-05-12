#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Core.Builders;
using Librame.Extensions.Core.Options;
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
        /// <param name="builder">给定的 <see cref="INetworkBuilder"/>。</param>
        /// <param name="configureDependency">给定的配置依赖动作方法（可选）。</param>
        /// <returns>返回 <see cref="INetworkBuilder"/>。</returns>
        public static INetworkBuilder AddDotNetty(this INetworkBuilder builder,
            Action<DotNettyDependency> configureDependency = null)
            => builder.AddDotNetty<DotNettyDependency>(configureDependency);

        /// <summary>
        /// 添加 DotNetty 扩展。
        /// </summary>
        /// <typeparam name="TDependency">指定的依赖类型。</typeparam>
        /// <param name="builder">给定的 <see cref="INetworkBuilder"/>。</param>
        /// <param name="configureDependency">给定的配置依赖动作方法（可选）。</param>
        /// <returns>返回 <see cref="INetworkBuilder"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static INetworkBuilder AddDotNetty<TDependency>(this INetworkBuilder builder,
            Action<TDependency> configureDependency = null)
            where TDependency : DotNettyDependency
        {
            // Clear Options Cache
            ConsistencyOptionsCache.TryRemove<DotNettyOptions>();

            // Add Builder Dependency
            var dependency = builder.AddBuilderDependency(out var dependencyType, configureDependency);
            builder.Services.TryAddReferenceBuilderDependency<DotNettyDependency>(dependency, dependencyType);

            // Add Dependencies
            if (!builder.ContainsParentBuilder<IEncryptionBuilder>())
                builder.AddEncryption().AddDeveloperGlobalSigningCredentials();

            // Configure Builder
            return builder
                .AddDotNettyDependency(dependency)
                .AddWrappers()
                .AddDemo();
        }

        private static INetworkBuilder AddWrappers(this INetworkBuilder builder)
        {
            builder.Services.TryAddSingleton<IBootstrapWrapperFactory, BootstrapWrapperFactory>();

            builder.Services.TryAddTransient<IBootstrapWrapper, BootstrapWrapper>();
            builder.Services.TryAddTransient<IServerBootstrapWrapper, ServerBootstrapWrapper>();
            builder.Services.TryAddTransient(typeof(IBootstrapWrapper<,>), typeof(BootstrapWrapper<,>));

            return builder;
        }

    }
}
