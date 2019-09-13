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
        /// 添加 DotNetty。
        /// </summary>
        /// <param name="builder">给定的 <see cref="INetworkBuilder"/>。</param>
        /// <param name="setupAction">给定的选项配置动作（可选）。</param>
        /// <returns>返回 <see cref="INetworkBuilder"/>。</returns>
        public static INetworkBuilder AddDotNetty(this INetworkBuilder builder,
            Action<DotNettyOptions> setupAction = null)
        {
            builder.Services.OnlyConfigure(setupAction);

            // 如果未添加加密扩展，则自动添加
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
