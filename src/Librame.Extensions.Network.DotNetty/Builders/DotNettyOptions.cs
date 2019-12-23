#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Network.Builders
{
    using Core.Builders;
    using DotNetty.Demo;

    /// <summary>
    /// DotNetty 选项。
    /// </summary>
    public class DotNettyOptions : IExtensionBuilderOptions
    {
        /// <summary>
        /// HTTP 服务端。
        /// </summary>
        public HttpServerOptions HttpServer { get; set; }
            = new HttpServerOptions();

        /// <summary>
        /// Discard 客户端。
        /// </summary>
        public ClientOptions DiscardClient { get; set; }
            = new ClientOptions();
        /// <summary>
        /// Discard 服务端。
        /// </summary>
        public ServerOptions DiscardServer { get; set; }
            = new ServerOptions();

        /// <summary>
        /// Echo 客户端。
        /// </summary>
        public ClientOptions EchoClient { get; set; }
            = new ClientOptions();
        /// <summary>
        /// Echo 服务端。
        /// </summary>
        public ServerOptions EchoServer { get; set; }
            = new ServerOptions();

        /// <summary>
        /// Factorial 客户端。
        /// </summary>
        public FactorialClientOptions FactorialClient { get; set; }
            = new FactorialClientOptions();
        /// <summary>
        /// Factorial 服务端。
        /// </summary>
        public ServerOptions FactorialServer { get; set; }
            = new ServerOptions();

        /// <summary>
        /// QuoteOfTheMoment 客户端。
        /// </summary>
        public ClientOptions QuoteOfTheMomentClient { get; set; }
            = new ClientOptions();
        /// <summary>
        /// QuoteOfTheMoment 服务端。
        /// </summary>
        public ServerOptions QuoteOfTheMomentServer { get; set; }
            = new ServerOptions();

        /// <summary>
        /// SecureChat 客户端。
        /// </summary>
        public ClientOptions SecureChatClient { get; set; }
            = new ClientOptions();
        /// <summary>
        /// SecureChat 服务端。
        /// </summary>
        public ServerOptions SecureChatServer { get; set; }
            = new ServerOptions();

        /// <summary>
        /// Telnet 客户端。
        /// </summary>
        public ClientOptions TelnetClient { get; set; }
            = new ClientOptions();
        /// <summary>
        /// Telnet 服务端。
        /// </summary>
        public ServerOptions TelnetServer { get; set; }
            = new ServerOptions();

        /// <summary>
        /// WebSocket 客户端。
        /// </summary>
        public WebSocketClientOptions WebSocketClient { get; set; }
            = new WebSocketClientOptions();
        /// <summary>
        /// WebSocket 服务端。
        /// </summary>
        public HttpServerOptions WebSocketServer { get; set; }
            = new HttpServerOptions();
    }
}
