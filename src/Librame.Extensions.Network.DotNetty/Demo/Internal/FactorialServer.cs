#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using DotNetty.Handlers.Logging;
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

    class FactorialServer : ChannelServiceBase, IFactorialServer
    {
        private readonly ServerOptions _serverOptions;


        public FactorialServer(IBootstrapWrapperFactory wrapperFactory,
            ISigningCredentialsService signingCredentials,
            IOptions<DotNettyOptions> options, ILoggerFactory loggerFactory)
            : base(wrapperFactory, signingCredentials, options, loggerFactory)
        {
            _serverOptions = Options.FactorialServer;
        }


        public Task StartAsync(Action<IChannel> configureProcess, string host = null, int? port = null)
            => StartAsync(new FactorialServerHandler(this), configureProcess, host, port);

        public async Task StartAsync<TChannelHandler>(TChannelHandler channelHandler,
            Action<IChannel> configureProcess, string host = null, int? port = null)
            where TChannelHandler : IChannelHandler
        {
            var address = IPAddress.Parse(host.EnsureString(_serverOptions.Host));
            var endPoint = new IPEndPoint(address, port.EnsureValue(_serverOptions.Port));

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
                    .Configure(bootstrap =>
                    {
                        bootstrap
                            .Option(ChannelOption.SoBacklog, 100)
                            .Handler(new LoggingHandler("LSTN"));
                    })
                    .AddFactorialHandler(tlsCertificate, channelHandler)
                    .BindAsync(endPoint);

                configureProcess.Invoke(channel);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.AsInnerMessage());
            }
            finally
            {
                Task.WaitAll(bossGroup.ShutdownGracefullyAsync(), workerGroup.ShutdownGracefullyAsync());
            }
        }

    }
}
