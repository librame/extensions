#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using DotNetty.Codecs.Http;
using DotNetty.Codecs.Http.WebSockets;
using DotNetty.Codecs.Http.WebSockets.Extensions.Compression;
using DotNetty.Handlers.Tls;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using DotNetty.Transport.Libuv;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Librame.Extensions.Network.DotNetty.Internal
{
    using Encryption;
    using Extensions;

    /// <summary>
    /// 内部 WebSocket 客户端通道服务。
    /// </summary>
    internal class InternalWebSocketClient : AbstractChannelService, IWebSocketClient
    {
        private readonly WebSocketClientOptions _clientOptions;


        /// <summary>
        /// 构造一个 <see cref="InternalWebSocketClient"/> 实例。
        /// </summary>
        /// <param name="signingCredentials">给定的 <see cref="ISigningCredentialsService"/>。</param>
        /// <param name="options">给定的 <see cref="IOptions{DotNettyOptions}"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        public InternalWebSocketClient(ISigningCredentialsService signingCredentials,
            IOptions<DotNettyOptions> options, ILoggerFactory loggerFactory)
            : base(signingCredentials, options, loggerFactory)
        {
            _clientOptions = Options.WebSocketClient;
        }


        /// <summary>
        /// 异步启动。
        /// </summary>
        /// <param name="configureProcess">给定的配置处理方法。</param>
        /// <param name="handlerFactory">给定的通道处理程序工厂方法（可选）。</param>
        /// <param name="host">给定要启动的主机（可选；默认使用选项配置）。</param>
        /// <param name="port">给定要启动的端口（可选；默认使用选项配置）。</param>
        /// <returns>返回一个异步操作。</returns>
        public async Task StartAsync(Action<IChannel> configureProcess,
            Func<IChannelHandler> handlerFactory = null, string host = null, int? port = null)
        {
            host = host.EnsureValue(_clientOptions.Host);
            port = port.EnsureValue(_clientOptions.Port);

            var builder = new UriBuilder
            {
                Scheme = _clientOptions.UseSSL ? "wss" : "ws",
                Host = host,
                Port = port.Value
            };
            
            if (!_clientOptions.Path.IsNullOrEmpty())
                builder.Path = _clientOptions.Path;

            Uri uri = builder.Uri;

            Logger.LogInformation("Transport type: " + (_clientOptions.UseLibuv ? "Libuv" : "Socket"));

            IEventLoopGroup group;
            if (_clientOptions.UseLibuv)
            {
                group = new EventLoopGroup();
            }
            else
            {
                group = new MultithreadEventLoopGroup();
            }

            X509Certificate2 cert = null;
            string targetHost = null;

            if (_clientOptions.UseSSL)
            {
                var credentials = SigningCredentials.GetSigningCredentials(_clientOptions.SigningCredentialsKey);
                cert = credentials.ResolveCertificate();
                targetHost = cert.GetNameInfo(X509NameType.DnsName, false);
            }

            try
            {
                var bootstrap = new Bootstrap();

                bootstrap
                    .Group(group)
                    .Option(ChannelOption.TcpNodelay, true);

                if (_clientOptions.UseLibuv)
                {
                    bootstrap.Channel<TcpChannel>();
                }
                else
                {
                    bootstrap.Channel<TcpSocketChannel>();
                }

                // Connect with V13 (RFC 6455 aka HyBi-17). You can change it to V08 or V00.
                // If you change it to V00, ping is not supported and remember to change
                // HttpResponseDecoder to WebSocketHttpResponseDecoder in the pipeline.
                var handshaker = WebSocketClientHandshakerFactory.NewHandshaker(uri,
                    WebSocketVersion.V13, null, true, new DefaultHttpHeaders());

                if (handlerFactory.IsNull())
                    handlerFactory = () => new InternalWebSocketClientHandler(this, handshaker);

                var handler = handlerFactory.Invoke() as InternalWebSocketClientHandler;
                
                bootstrap.Handler(new ActionChannelInitializer<IChannel>(channel =>
                {
                    var pipeline = channel.Pipeline;
                    if (cert.IsNotNull())
                    {
                        pipeline.AddLast("tls",
                            new TlsHandler(stream => new SslStream(stream, true, (sender, certificate, chain, errors) => true),
                            new ClientTlsSettings(targetHost)));
                    }
                    
                    pipeline.AddLast(
                        new HttpClientCodec(),
                        new HttpObjectAggregator(8192),
                        WebSocketClientCompressionHandler.Instance,
                        handler);
                }));
                
                var address = IPAddress.Parse(host);
                var bootstrapChannel = await bootstrap.ConnectAsync(new IPEndPoint(address, port.Value));
                await handler.HandshakeCompletion;

                Logger.LogInformation("WebSocket handshake completed.\n");
                Logger.LogInformation($"\t[{_clientOptions.ExitCommand}]:Quit \n\t [ping]:Send ping frame\n\t Enter any text and Enter: Send text frame");

                configureProcess.Invoke(bootstrapChannel);

                await bootstrapChannel.CloseAsync();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.AsInnerMessage());
            }
            finally
            {
                await group.ShutdownGracefullyAsync(_clientOptions.QuietPeriod, _clientOptions.TimeOut);
            }
        }

    }
}
