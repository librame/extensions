#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using DotNetty.Handlers.Logging;
using DotNetty.Transport.Channels;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Librame.Extensions.Network.DotNetty.Demo
{
    using Encryption.Services;
    using Network.Builders;
    using Network.DotNetty.Options;

    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal class QuoteOfTheMomentServer : ChannelServiceBase, IQuoteOfTheMomentServer
    {
        private readonly ServerOptions _serverOptions;


        public QuoteOfTheMomentServer(IBootstrapWrapperFactory wrapperFactory,
            ISigningCredentialsService signingCredentials,
            DotNettyDependency dependency,
            ILoggerFactory loggerFactory)
            : base(wrapperFactory, signingCredentials, dependency, loggerFactory)
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
