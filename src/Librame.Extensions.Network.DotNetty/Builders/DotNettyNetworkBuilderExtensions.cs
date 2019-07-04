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
    using Internal;

    /// <summary>
    /// DotNetty 网络构建器静态扩展。
    /// </summary>
    public static class DotNettyNetworkBuilderExtensions
    {
        /// <summary>
        /// 添加 DotNetty。
        /// </summary>
        /// <param name="builder">给定的 <see cref="INetworkBuilder"/>。</param>
        /// <param name="configureOptions">给定的 <see cref="Action{DotNettyOptions}"/>（可选；高优先级）。</param>
        /// <param name="configuration">给定的 <see cref="IConfiguration"/>（可选；次优先级）。</param>
        /// <param name="configureBinderOptions">给定的配置绑定器选项动作（可选）。</param>
        /// <returns>返回 <see cref="INetworkBuilder"/>。</returns>
        public static INetworkBuilder AddDotNetty(this INetworkBuilder builder,
            Action<DotNettyOptions> configureOptions = null,
            IConfiguration configuration = null,
            Action<BinderOptions> configureBinderOptions = null)
        {
            builder.Configure(configureOptions,
                configuration, configureBinderOptions);

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
