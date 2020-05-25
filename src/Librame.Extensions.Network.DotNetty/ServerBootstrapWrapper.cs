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
using System;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography.X509Certificates;

namespace Librame.Extensions.Network.DotNetty
{
    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal class ServerBootstrapWrapper : BootstrapWrapper<ServerBootstrap, IServerChannel>, IServerBootstrapWrapper
    {
        public ServerBootstrapWrapper(ServerBootstrap bootstrap, ILoggerFactory loggerFactory)
            : base(bootstrap, loggerFactory)
        {
        }


        public IServerBootstrapWrapper AddChannelHandler<TInitializeChannel>(X509Certificate2 tlsCertificate = null,
            Action<IChannelPipeline> pipelineAction = null, bool addTlsPipelineName = false)
            where TInitializeChannel : IChannel
        {
            Bootstrap.ChildHandler(new ActionChannelInitializer<TInitializeChannel>(channel =>
            {
                var pipeline = channel.Pipeline;
                if (tlsCertificate.IsNotNull())
                {
                    var tlsHandler = TlsHandler.Server(tlsCertificate);

                    if (addTlsPipelineName)
                        pipeline.AddLast("tls", tlsHandler);
                    else
                        pipeline.AddLast(tlsHandler);

                    var targetHost = tlsCertificate.GetNameInfo(X509NameType.DnsName, false);
                    Logger.LogInformation($"Add TLS handler: {targetHost}");
                }

                pipelineAction?.Invoke(pipeline);
            }));

            return this;
        }


        public IServerBootstrapWrapper Configure(Action<ServerBootstrap> configureAction)
        {
            configureAction.Invoke(Bootstrap);
            return this;
        }

    }
}
