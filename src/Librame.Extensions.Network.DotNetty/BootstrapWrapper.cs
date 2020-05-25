#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using DotNetty.Handlers.Tls;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using Microsoft.Extensions.Logging;
using Polly;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Librame.Extensions.Network.DotNetty
{
    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal class BootstrapWrapper : BootstrapWrapper<Bootstrap, IChannel>, IBootstrapWrapper
    {
        public BootstrapWrapper(Bootstrap bootstrap, ILoggerFactory loggerFactory)
            : base(bootstrap, loggerFactory)
        {
        }


        [SuppressMessage("Microsoft.Cryptography", "CA5359")]
        public IBootstrapWrapper AddChannelHandlerAsync<TInitializeChannel>(X509Certificate2 tlsCertificate = null,
            Action<IChannelPipeline> pipelineAction = null, bool addTlsPipelineName = false)
            where TInitializeChannel : IChannel
        {
            Bootstrap.Handler(new ActionChannelInitializer<TInitializeChannel>(channel =>
            {
                var pipeline = channel.Pipeline;
                if (tlsCertificate.IsNotNull())
                {
                    var targetHost = tlsCertificate.GetNameInfo(X509NameType.DnsName, false);
                    var tlsHandler = new TlsHandler(stream =>
                    {
                        return new SslStream(stream, true, (sender, cert, chain, errors) => true);
                    },
                    new ClientTlsSettings(targetHost));

                    if (addTlsPipelineName)
                        pipeline.AddLast("tls", tlsHandler);
                    else
                        pipeline.AddLast(tlsHandler);

                    Logger.LogInformation($"Add TLS handler: {targetHost}");
                }

                pipelineAction?.Invoke(pipeline);
            }));

            return this;
        }


        public IBootstrapWrapper Configure(Action<Bootstrap> configureAction)
        {
            configureAction.Invoke(Bootstrap);
            return this;
        }


        public Task<IChannel> ConnectAsync(string host, int port, int retryCount = 3)
        {
            var address = IPAddress.Parse(host);
            return ConnectAsync(new IPEndPoint(address, port), retryCount);
        }

        public Task<IChannel> ConnectAsync(IPEndPoint endPoint, int retryCount = 3)
        {
            return Policy
                .Handle<ConnectTimeoutException>()
                .OrResult<IChannel>(r => r.IsNull())
                .RetryAsync(retryCount, (exception, _retryCount, context) =>
                {
                    Logger.LogDebug($"Start the {retryCount} retry");
                })
                .ExecuteAsync(() => Bootstrap.ConnectAsync(endPoint));
        }

    }


    class BootstrapWrapper<TBootstrap, TChannel> : IBootstrapWrapper<TBootstrap, TChannel>
        where TBootstrap : AbstractBootstrap<TBootstrap, TChannel>
        where TChannel : IChannel
    {
        private readonly ILoggerFactory _loggerFactory;


        public BootstrapWrapper(TBootstrap bootstrap, ILoggerFactory loggerFactory)
        {
            Bootstrap = bootstrap.NotNull(nameof(bootstrap));
            _loggerFactory = loggerFactory.NotNull(nameof(loggerFactory));
        }


        protected TBootstrap Bootstrap { get; }

        public ILogger Logger
            => _loggerFactory.CreateLogger(GetType());


        public Task<IChannel> BindAsync(int port)
            => Bootstrap.BindAsync(port);

        public Task<IChannel> BindAsync(string host, int port)
            => Bootstrap.BindAsync(host, port);

        public Task<IChannel> BindAsync(IPEndPoint endPoint)
            => Bootstrap.BindAsync(endPoint);

    }

}
