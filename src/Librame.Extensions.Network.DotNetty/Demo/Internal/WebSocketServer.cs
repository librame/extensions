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

    class WebSocketServer : ChannelServiceBase, IWebSocketServer
    {
        private readonly HttpServerOptions _serverOptions;


        public WebSocketServer(IBootstrapWrapperFactory wrapperFactory,
            ISigningCredentialsService signingCredentials,
            IOptions<DotNettyOptions> options, ILoggerFactory loggerFactory)
            : base(wrapperFactory, signingCredentials, options, loggerFactory)
        {
            _serverOptions = Options.WebSocketServer;
            ResourceLeakDetector.Level = _serverOptions.LeakDetector;
        }


        public Task StartAsync(Action<IChannel> configureProcess, string host = null, int? port = null)
            => StartAsync(new WebSocketServerHandler(this), configureProcess, host, port);

        public async Task StartAsync<TChannelHandler>(TChannelHandler channelHandler,
            Action<IChannel> configureProcess, string host = null, int? port = null)
            where TChannelHandler : IChannelHandler
        {
            var address = IPAddress.Parse(host.NotEmptyOrDefault(_serverOptions.Host));
            var endPoint = new IPEndPoint(address, port.NotNullOrDefault(_serverOptions.Port));

            IEventLoopGroup bossGroup = null;
            IEventLoopGroup workerGroup = null;

            try
            {
                X509Certificate2 tlsCertificate = null;
                if (_serverOptions.IsSsl)
                {
                    var credentials = SigningCredentials.GetSigningCredentials(_serverOptions.SigningCredentialsKey);
                    tlsCertificate = credentials.ResolveCertificate();
                }

                var channel = await WrapperFactory
                    .CreateTcpServer(_serverOptions.UseLibuv, out bossGroup, out workerGroup)
                    .AddWebSocketHandler(tlsCertificate, channelHandler)
                    .BindAsync(endPoint).ConfigureAndResultAsync();

                Logger.LogInformation("Open your web browser and navigate to "
                    + $"{(_serverOptions.IsSsl ? "https" : "http")}"
                    + $"://{channel.LocalAddress.ToString()}:{port}/");
                Logger.LogInformation("Listening on "
                    + $"{(_serverOptions.IsSsl ? "wss" : "ws")}"
                    + $"://{channel.LocalAddress.ToString()}:{port}/websocket");

                configureProcess.Invoke(channel);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.AsInnerMessage());
            }
            finally
            {
                workerGroup.ShutdownGracefullyAsync().Wait();
                bossGroup.ShutdownGracefullyAsync().Wait();
            }
        }

    }
}
