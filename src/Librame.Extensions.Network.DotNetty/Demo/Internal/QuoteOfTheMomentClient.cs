#region License

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
using System;
using System.Net;
using System.Threading.Tasks;

namespace Librame.Extensions.Network.DotNetty
{
    using Encryption;

    class QuoteOfTheMomentClient : ChannelServiceBase, IQuoteOfTheMomentClient
    {
        private readonly ClientOptions _clientOptions;


        public QuoteOfTheMomentClient(IBootstrapWrapperFactory wrapperFactory,
            ISigningCredentialsService signingCredentials,
            IOptions<DotNettyOptions> options, ILoggerFactory loggerFactory)
            : base(wrapperFactory, signingCredentials, options, loggerFactory)
        {
            _clientOptions = Options.QuoteOfTheMomentClient;
        }


        public Task StartAsync(Action<IChannel> configureProcess)
            => StartAsync(new QuoteOfTheMomentClientHandler(this), configureProcess);

        public async Task StartAsync<TChannelHandler>(TChannelHandler channelHandler,
            Action<IChannel> configureProcess)
            where TChannelHandler : IChannelHandler
        {
            IEventLoopGroup group = null;

            try
            {
                var channel = await WrapperFactory
                    .CreateUdp(out group)
                    .Configure(bootstrap =>
                    {
                        bootstrap.Option(ChannelOption.SoBroadcast, true);
                    })
                    .AddQuoteOfTheMomentHandler(channelHandler)
                    .BindAsync(IPEndPoint.MinPort).ConfigureAndResultAsync();

                configureProcess.Invoke(channel);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.AsInnerMessage());
            }
            finally
            {
                await group.ShutdownGracefullyAsync(_clientOptions.QuietPeriod,
                    _clientOptions.TimeOut);
            }
        }

    }
}
