#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Librame.Extensions.Network.Builders
{
    using DotNetty.Demo;

    static class DemoNetworkBuilderExtensions
    {
        internal static INetworkBuilder AddDemo(this INetworkBuilder builder)
        {
            builder.Services.TryAddSingleton<IDiscardClient, DiscardClient>();
            builder.Services.TryAddSingleton<IDiscardServer, DiscardServer>();

            builder.Services.TryAddSingleton<IEchoClient, EchoClient>();
            builder.Services.TryAddSingleton<IEchoServer, EchoServer>();

            builder.Services.TryAddSingleton<IFactorialClient, FactorialClient>();
            builder.Services.TryAddSingleton<IFactorialServer, FactorialServer>();

            builder.Services.TryAddSingleton<IHttpServer, HttpServer>();

            builder.Services.TryAddSingleton<IQuoteOfTheMomentClient, QuoteOfTheMomentClient>();
            builder.Services.TryAddSingleton<IQuoteOfTheMomentServer, QuoteOfTheMomentServer>();

            builder.Services.TryAddSingleton<ISecureChatClient, SecureChatClient>();
            builder.Services.TryAddSingleton<ISecureChatServer, SecureChatServer>();

            builder.Services.TryAddSingleton<ITelnetClient, TelnetClient>();
            builder.Services.TryAddSingleton<ITelnetServer, TelnetServer>();

            builder.Services.TryAddSingleton<IWebSocketClient, WebSocketClient>();
            builder.Services.TryAddSingleton<IWebSocketServer, WebSocketServer>();

            return builder;
        }

    }
}
