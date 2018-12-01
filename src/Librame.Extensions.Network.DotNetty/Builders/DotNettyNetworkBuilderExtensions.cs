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
    using Builders;

    /// <summary>
    /// DotNetty 网络构建器静态扩展。
    /// </summary>
    public static class DotNettyNetworkBuilderExtensions
    {

        /// <summary>
        /// 添加 DotNetty。
        /// </summary>
        /// <param name="builder">给定的 <see cref="INetworkBuilder"/>。</param>
        /// <param name="configureOptions">给定的 <see cref="Action{ChannelOptions}"/>（可选）。</param>
        /// <returns>返回 <see cref="INetworkBuilder"/>。</returns>
        public static INetworkBuilder AddDotNetty(this INetworkBuilder builder, Action<ChannelOptions> configureOptions = null)
        {
            if (configureOptions.IsNotDefault())
                builder.Services.Configure(configureOptions);

            builder.Services.AddSingleton<IDiscardClient, InternalDiscardClient>();
            builder.Services.AddSingleton<IDiscardServer, InternalDiscardServer>();

            builder.Services.AddSingleton<IEchoClient, InternalEchoClient>();
            builder.Services.AddSingleton<IEchoServer, InternalEchoServer>();

            builder.Services.AddSingleton<IFactorialClient, InternalFactorialClient>();
            builder.Services.AddSingleton<IFactorialServer, InternalFactorialServer>();

            builder.Services.AddSingleton<IHttpServer, InternalHttpServer>();

            builder.Services.AddSingleton<ISecureChatClient, InternalSecureChatClient>();
            builder.Services.AddSingleton<ISecureChatServer, InternalSecureChatServer>();

            builder.Services.AddSingleton<ITelnetClient, InternalTelnetClient>();
            builder.Services.AddSingleton<ITelnetServer, InternalTelnetServer>();

            builder.Services.AddSingleton<IWebSocketClient, InternalWebSocketClient>();
            builder.Services.AddSingleton<IWebSocketServer, InternalWebSocketServer>();

            return builder;
        }

    }
}
