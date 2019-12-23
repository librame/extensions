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
            builder.Services.TryAddScoped<IDiscardClient, DiscardClient>();
            builder.Services.TryAddScoped<IDiscardServer, DiscardServer>();

            builder.Services.TryAddScoped<IEchoClient, EchoClient>();
            builder.Services.TryAddScoped<IEchoServer, EchoServer>();

            builder.Services.TryAddScoped<IFactorialClient, FactorialClient>();
            builder.Services.TryAddScoped<IFactorialServer, FactorialServer>();

            builder.Services.TryAddScoped<IHttpServer, HttpServer>();

            builder.Services.TryAddScoped<IQuoteOfTheMomentClient, QuoteOfTheMomentClient>();
            builder.Services.TryAddScoped<IQuoteOfTheMomentServer, QuoteOfTheMomentServer>();

            builder.Services.TryAddScoped<ISecureChatClient, SecureChatClient>();
            builder.Services.TryAddScoped<ISecureChatServer, SecureChatServer>();

            builder.Services.TryAddScoped<ITelnetClient, TelnetClient>();
            builder.Services.TryAddScoped<ITelnetServer, TelnetServer>();

            builder.Services.TryAddScoped<IWebSocketClient, WebSocketClient>();
            builder.Services.TryAddScoped<IWebSocketServer, WebSocketServer>();

            return builder;
        }

    }
}
