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

namespace Librame.Extensions.Network.DotNetty
{
    /// <summary>
    /// DEMO 网络构建器静态扩展。
    /// </summary>
    static class DemoNetworkBuilderExtensions
    {
        /// <summary>
        /// 增加 DEMO。
        /// </summary>
        /// <param name="builder">给定的 <see cref="INetworkBuilder"/>。</param>
        /// <returns>返回 <see cref="INetworkBuilder"/>。</returns>
        public static INetworkBuilder AddDemo(this INetworkBuilder builder)
        {
            builder.Services.AddScoped<IDiscardClient, DiscardClient>();
            builder.Services.AddScoped<IDiscardServer, DiscardServer>();

            builder.Services.AddScoped<IEchoClient, EchoClient>();
            builder.Services.AddScoped<IEchoServer, EchoServer>();

            builder.Services.AddScoped<IFactorialClient, FactorialClient>();
            builder.Services.AddScoped<IFactorialServer, FactorialServer>();

            builder.Services.AddScoped<IHttpServer, HttpServer>();

            builder.Services.AddScoped<ISecureChatClient, SecureChatClient>();
            builder.Services.AddScoped<ISecureChatServer, SecureChatServer>();

            builder.Services.AddScoped<ITelnetClient, TelnetClient>();
            builder.Services.AddScoped<ITelnetServer, TelnetServer>();

            builder.Services.AddScoped<IWebSocketClient, WebSocketClient>();
            builder.Services.AddScoped<IWebSocketServer, WebSocketServer>();

            return builder;
        }

    }
}
