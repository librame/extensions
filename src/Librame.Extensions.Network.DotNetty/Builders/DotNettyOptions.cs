#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using DotNetty.Common;
using System;
using System.Text;

namespace Librame.Extensions.Network.DotNetty
{
    using Core;
    using Encryption;

    /// <summary>
    /// DotNetty 选项。
    /// </summary>
    public class DotNettyOptions : AbstractExtensionBuilderOptions
    {
        /// <summary>
        /// 字符编码（默认为 <see cref="Encoding.UTF8"/>）。
        /// </summary>
        public Encoding Encoding { get; set; }
            = Encoding.UTF8;


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


    /// <summary>
    /// 析因客户端选项。
    /// </summary>
    public class FactorialClientOptions : ClientOptions
    {
        /// <summary>
        /// 数量。
        /// </summary>
        public int Count { get; set; }
            = 100;
    }


    /// <summary>
    /// WebSocket 客户端。
    /// </summary>
    public class WebSocketClientOptions : ClientOptions
    {
        /// <summary>
        /// 路径。
        /// </summary>
        public string Path { get; set; }
            = "/websocket";
    }


    /// <summary>
    /// HTTP 服务端选项。
    /// </summary>
    public class HttpServerOptions : ServerOptions
    {
        /// <summary>
        /// 资源探测器检测级别。
        /// </summary>
        public ResourceLeakDetector.DetectionLevel LeakDetector { get; set; }
            = ResourceLeakDetector.DetectionLevel.Disabled;
    }


    /// <summary>
    /// 客户端选项。
    /// </summary>
    public class ClientOptions : ConnectionOptions
    {
        /// <summary>
        /// 缓冲区大小。
        /// </summary>
        public int BufferSize { get; set; }
            = 256;

        /// <summary>
        /// 重试次数。
        /// </summary>
        public int RetryCount { get; set; }
            = 3;
    }


    /// <summary>
    /// 服务端选项。
    /// </summary>
    public class ServerOptions : ConnectionOptions
    {
    }


    /// <summary>
    /// 连接选项。
    /// </summary>
    public class ConnectionOptions
    {
        /// <summary>
        /// 使用 Libuv（默认使用）。
        /// </summary>
        public bool UseLibuv { get; set; }
            = false;

        /// <summary>
        /// 退出命名。
        /// </summary>
        public string ExitCommand { get; set; }
            = "exit";

        /// <summary>
        /// 签名证书键名（默认使用全局键名）。
        /// </summary>
        public string SigningCredentialsKey { get; set; }
            = EncryptionBuilderOptions.GLOBAL_KEY;

        /// <summary>
        /// 是 SSL（默认不是）。
        /// </summary>
        public bool IsSsl { get; set; }
            = false;

        /// <summary>
        /// 主机。
        /// </summary>
        public string Host { get; set; }
            = "127.0.0.1";

        /// <summary>
        /// 端口。
        /// </summary>
        public int Port { get; set; }
            = 8070;

        /// <summary>
        /// 静默时间间隔（默认 100 毫秒，即 0.1 秒）。
        /// </summary>
        public TimeSpan QuietPeriod { get; set; }
            = TimeSpan.FromMilliseconds(100);

        /// <summary>
        /// 超时时间间隔（默认 1 秒）。
        /// </summary>
        public TimeSpan TimeOut { get; set; }
            = TimeSpan.FromSeconds(1);
    }

}
