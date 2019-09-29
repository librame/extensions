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
using DotNetty.Transport.Channels;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Librame.Extensions.Network.DotNetty
{
    using Encryption;

    class WebSocketClient : ChannelServiceBase, IWebSocketClient
    {
        private readonly WebSocketClientOptions _clientOptions = null;


        public WebSocketClient(IBootstrapWrapperFactory wrapperFactory,
            ISigningCredentialsService signingCredentials,
            IOptions<DotNettyOptions> options, ILoggerFactory loggerFactory)
            : base(wrapperFactory, signingCredentials, options, loggerFactory)
        {
            _clientOptions = Options.WebSocketClient;
        }


        public async Task StartAsync(Action<IChannel> configureProcess, string host = null, int? port = null)
        {
            Action<IChannel> _configureProcess = async channel =>
            {
                configureProcess.Invoke(channel);

                var handler = channel.Pipeline.Last();
                if (handler.IsNotNull())
                {
                    var webSocketHandler = handler.CastTo<IChannelHandler,
                        WebSocketClientHandler>(nameof(handler));

                    await webSocketHandler.HandshakeCompletion.ConfigureAndWaitAsync();

                    Logger.LogInformation("WebSocket handshake completed.\n");
                    Logger.LogInformation($"\t[{_clientOptions.ExitCommand}]:Quit \n\t [ping]:Send ping frame\n\t Enter any text and Enter: Send text frame");
                }
            };

            await StartAsync(webSocketUrl =>
            {
                // Connect with V13 (RFC 6455 aka HyBi-17). You can change it to V08 or V00.
                // If you change it to V00, ping is not supported and remember to change
                // HttpResponseDecoder to WebSocketHttpResponseDecoder in the pipeline.
                var handshaker = WebSocketClientHandshakerFactory.NewHandshaker(webSocketUrl,
                    WebSocketVersion.V13, null, true, new DefaultHttpHeaders());

                return new WebSocketClientHandler(this, handshaker);
            },
            _configureProcess, host, port).ConfigureAndWaitAsync();
        }

        public async Task StartAsync<TChannelHandler>(Func<Uri, TChannelHandler> channelHandlerFactory,
            Action<IChannel> configureProcess, string host = null, int? port = null)
            where TChannelHandler : IChannelHandler
        {
            host = host.NotEmptyOrDefault(_clientOptions.Host);
            port = port.NotNullOrDefault(_clientOptions.Port);

            var builder = new UriBuilder
            {
                Scheme = _clientOptions.IsSsl ? "wss" : "ws",
                Host = host,
                Port = port.Value
            };

            if (!_clientOptions.Path.IsEmpty())
                builder.Path = _clientOptions.Path;

            IEventLoopGroup group = null;

            try
            {
                X509Certificate2 tlsCertificate = null;
                if (_clientOptions.IsSsl)
                {
                    var credentials = SigningCredentials.GetSigningCredentials(_clientOptions.SigningCredentialsKey);
                    tlsCertificate = credentials.ResolveCertificate();
                }

                var address = IPAddress.Parse(builder.Uri.Host);
                var endPoint = new IPEndPoint(address, builder.Uri.Port);
                var webSocketHandler = channelHandlerFactory.Invoke(builder.Uri);

                var channel = await WrapperFactory
                    .CreateTcp(_clientOptions.UseLibuv, out group)
                    .AddWebSocketHandler(tlsCertificate, webSocketHandler)
                    .ConnectAsync(endPoint, _clientOptions.RetryCount).ConfigureAndResultAsync();

                Logger.LogInformation($"Connect ip end point: {endPoint}");

                configureProcess.Invoke(channel);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.AsInnerMessage());
            }
            finally
            {
                await group.ShutdownGracefullyAsync(_clientOptions.QuietPeriod,
                    _clientOptions.TimeOut).ConfigureAndWaitAsync();
            }
        }

    }
}
