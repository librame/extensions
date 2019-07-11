#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.Configuration;
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
        /// <param name="configureOptions">给定的 <see cref="Action{DotNettyOptions}"/>（可选；高优先级）。</param>
        /// <param name="configuration">给定的 <see cref="IConfiguration"/>（可选；次优先级）。</param>
        /// <param name="configureBinderOptions">给定的配置绑定器选项动作（可选）。</param>
        /// <param name="addEncryptionFactory">注册加密构建器扩展工厂方法（可选）。</param>
        /// <returns>返回 <see cref="INetworkBuilder"/>。</returns>
        public static INetworkBuilder AddDotNetty(this INetworkBuilder builder,
            Action<DotNettyOptions> configureOptions = null,
            IConfiguration configuration = null,
            Action<BinderOptions> configureBinderOptions = null,
            Func<INetworkBuilder, IEncryptionBuilder> addEncryptionFactory = null)
        {
            builder.Configure(configureOptions,
                configuration, configureBinderOptions);

            if (!(builder is IEncryptionBuilder))
            {
                if (addEncryptionFactory.IsNull())
                {
                    addEncryptionFactory = _builder =>
                    {
                        return _builder.AddEncryption(options =>
                        {
                            options.Identifier = _defaultIdentifier;
                        })
                        .AddDeveloperGlobalSigningCredentials();
                    };
                }

                addEncryptionFactory.Invoke(builder);
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
