#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Libuv;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime;
using System.Runtime.InteropServices;

namespace Librame.Extensions.Network.DotNetty
{
    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal class BootstrapWrapperFactory : IBootstrapWrapperFactory
    {
        private readonly ILoggerFactory _loggerFactory;


        public BootstrapWrapperFactory(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory.NotNull(nameof(loggerFactory));

            Logger = _loggerFactory.CreateLogger<BootstrapWrapperFactory>();
        }


        public ILogger Logger { get; }


        public IBootstrapWrapper Create(bool useLibuv, out IEventLoopGroup group)
        {
            var bootstrap = CreateBootstrap(useLibuv, out group);
            return new BootstrapWrapper(bootstrap, _loggerFactory);
        }

        public IServerBootstrapWrapper CreateServer(bool useLibuv,
            out IEventLoopGroup bossGroup, out IEventLoopGroup workerGroup)
        {
            var bootstrap = CreateServerBootstrap(useLibuv, out bossGroup, out workerGroup);
            return new ServerBootstrapWrapper(bootstrap, _loggerFactory);
        }


        public IBootstrapWrapper<Bootstrap, IChannel> CreateGeneric(bool useLibuv,
            out IEventLoopGroup group)
        {
            var bootstrap = CreateBootstrap(useLibuv, out group);
            return new BootstrapWrapper<Bootstrap, IChannel>(bootstrap, _loggerFactory);
        }

        public IBootstrapWrapper<ServerBootstrap, IServerChannel> CreateServerGeneric(bool useLibuv,
            out IEventLoopGroup bossGroup, out IEventLoopGroup workerGroup)
        {
            var bootstrap = CreateServerBootstrap(useLibuv, out bossGroup, out workerGroup);
            return new BootstrapWrapper<ServerBootstrap, IServerChannel>(bootstrap, _loggerFactory);
        }


        #region CreateBootstrap

        [SuppressMessage("Microsoft.Design", "CA1303:DoNotPassLiteralsAsLocalizedParameters")]
        private Bootstrap CreateBootstrap(bool useLibuv, out IEventLoopGroup group)
        {
            Logger.LogInformation($"\n{RuntimeInformation.OSArchitecture} {RuntimeInformation.OSDescription}"
                + $"\n{RuntimeInformation.ProcessArchitecture} {RuntimeInformation.FrameworkDescription}"
                + $"\nProcessor Count : {Environment.ProcessorCount}\n");

            if (useLibuv)
            {
                group = new EventLoopGroup();
            }
            else
            {
                group = new MultithreadEventLoopGroup();
            }
            Logger.LogDebug("Transport type: " + (useLibuv ? "Libuv" : "Socket"));

            var bootstrap = new Bootstrap().Group(group);
            Logger.LogDebug($"Use group: {group.GetType().GetDisplayNameWithNamespace()}");

            return bootstrap;
        }

        [SuppressMessage("Microsoft.Design", "CA1303:DoNotPassLiteralsAsLocalizedParameters")]
        private ServerBootstrap CreateServerBootstrap(bool useLibuv,
            out IEventLoopGroup bossGroup, out IEventLoopGroup workerGroup)
        {
            Logger.LogInformation(
                      $"\n{RuntimeInformation.OSArchitecture} {RuntimeInformation.OSDescription}"
                      + $"\n{RuntimeInformation.ProcessArchitecture} {RuntimeInformation.FrameworkDescription}"
                      + $"\nProcessor Count : {Environment.ProcessorCount}\n");

            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                GCSettings.LatencyMode = GCLatencyMode.SustainedLowLatency;

            Logger.LogDebug($"Server garbage collection: {GCSettings.IsServerGC}");
            Logger.LogDebug($"Current latency mode for garbage collection: {GCSettings.LatencyMode}");
            Logger.LogDebug("\n");

            if (useLibuv)
            {
                var dispatcher = new DispatcherEventLoopGroup();
                bossGroup = dispatcher;
                workerGroup = new WorkerEventLoopGroup(dispatcher);
            }
            else
            {
                bossGroup = new MultithreadEventLoopGroup(1);
                workerGroup = new MultithreadEventLoopGroup();
            }
            Logger.LogDebug("Transport type: " + (useLibuv ? "Libuv" : "Socket"));

            var bootstrap = new ServerBootstrap().Group(bossGroup, workerGroup);
            Logger.LogDebug($"Use boss group: {bossGroup.GetType().GetDisplayNameWithNamespace()}");
            Logger.LogDebug($"Use worker group: {workerGroup.GetType().GetDisplayNameWithNamespace()}");

            return bootstrap;
        }

        #endregion

    }
}
