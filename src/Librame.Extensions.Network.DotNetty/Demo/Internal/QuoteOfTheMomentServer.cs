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
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Librame.Extensions.Network.DotNetty.Demo
{
    using Builders;
    using Core.Builders;
    using Encryption.Services;

    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal class QuoteOfTheMomentServer : ChannelServiceBase, IQuoteOfTheMomentServer
    {
        private readonly ServerOptions _serverOptions;


        public QuoteOfTheMomentServer(IBootstrapWrapperFactory wrapperFactory,
            ISigningCredentialsService signingCredentials, IOptions<CoreBuilderOptions> coreOptions,
            IOptions<DotNettyOptions> options, ILoggerFactory loggerFactory)
            : base(wrapperFactory, signingCredentials, coreOptions, options, loggerFactory)
        {
            _serverOptions = Options.FactorialServer;
        }


        public Task StartAsync(Action<IChannel> configureProcess)
            => StartAsync(new QuoteOfTheMomentServerHandler(this), configureProcess);

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
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
                        bootstrap
                            .Option(ChannelOption.SoBroadcast, true)
                            .Handler(new LoggingHandler("SRV-LSTN"));
                    })
                    .AddQuoteOfTheMomentHandler(channelHandler)
                    .BindAsync(_serverOptions.Port).ConfigureAndResultAsync();

                configureProcess.Invoke(channel);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.AsInnerMessage());
            }
            finally
            {
                await group.ShutdownGracefullyAsync(_serverOptions.QuietPeriod, _serverOptions.TimeOut).ConfigureAndWaitAsync();
            }
        }

    }
}
