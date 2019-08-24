﻿#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

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

    class FactorialClient : ChannelServiceBase, IFactorialClient
    {
        private readonly FactorialClientOptions _clientOptions;


        public FactorialClient(IBootstrapWrapperFactory wrapperFactory,
            ISigningCredentialsService signingCredentials,
            IOptions<DotNettyOptions> options, ILoggerFactory loggerFactory)
            : base(wrapperFactory, signingCredentials, options, loggerFactory)
        {
            _clientOptions = Options.FactorialClient;
        }


        public Task StartAsync(Action<IChannel> configureProcess, string host = null, int? port = null)
        {
            return StartAsync(new FactorialClientHandler(this),
                configureProcess, host, port);
        }

        public async Task StartAsync<TChannelHandler>(TChannelHandler channelHandler,
            Action<IChannel> configureProcess, string host = null, int? port = null)
            where TChannelHandler : IChannelHandler
        {
            var address = IPAddress.Parse(host.EnsureString(_clientOptions.Host));
            var endPoint = new IPEndPoint(address, port.EnsureValue(_clientOptions.Port));

            IEventLoopGroup group = null;

            try
            {
                X509Certificate2 tlsCertificate = null;
                if (_clientOptions.IsSsl)
                {
                    var credentials = SigningCredentials.GetSigningCredentials(_clientOptions.SigningCredentialsKey);
                    tlsCertificate = credentials.ResolveCertificate();
                }

                var channel = await WrapperFactory
                    .CreateTcp(false, out group)
                    .AddFactorialHandler(tlsCertificate, channelHandler)
                    .ConnectAsync(endPoint, _clientOptions.RetryCount);

                Logger.LogInformation($"Connect ip end point: {endPoint}");

                configureProcess.Invoke(channel);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.AsInnerMessage());
            }
            finally
            {
                group.ShutdownGracefullyAsync().Wait(_clientOptions.TimeOut);
            }
        }

    }
}
