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
    using Internal;

    /// <summary>
    /// DotNetty 网络构建器静态扩展。
    /// </summary>
    public static class DotNettyNetworkBuilderExtensions
    {
        private static readonly AlgorithmIdentifier _defaultIdentifier
            = AlgorithmIdentifier.New();


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
            {
                builder.AddEncryption(options =>
                {
                    options.Identifier = _defaultIdentifier;
                })
                .AddDeveloperGlobalSigningCredentials();
            }

            return builder.AddDemo();
        }

        private static INetworkBuilder AddDemo(this INetworkBuilder builder)
        {
            builder.Services.AddScoped<IDiscardClient, InternalDiscardClient>();
            builder.Services.AddSingleton<IDiscardServer, InternalDiscardServer>();

            builder.Services.AddScoped<IEchoClient, InternalEchoClient>();
            builder.Services.AddSingleton<IEchoServer, InternalEchoServer>();

            builder.Services.AddScoped<IFactorialClient, InternalFactorialClient>();
            builder.Services.AddSingleton<IFactorialServer, InternalFactorialServer>();

            builder.Services.AddSingleton<IHttpServer, InternalHttpServer>();

            builder.Services.AddScoped<ISecureChatClient, InternalSecureChatClient>();
            builder.Services.AddSingleton<ISecureChatServer, InternalSecureChatServer>();

            builder.Services.AddScoped<ITelnetClient, InternalTelnetClient>();
            builder.Services.AddSingleton<ITelnetServer, InternalTelnetServer>();

            builder.Services.AddScoped<IWebSocketClient, InternalWebSocketClient>();
            builder.Services.AddSingleton<IWebSocketServer, InternalWebSocketServer>();

            return builder;
        }

    }
}
