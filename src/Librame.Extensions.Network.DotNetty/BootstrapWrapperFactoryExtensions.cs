#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using DotNetty.Transport.Libuv;
using Microsoft.Extensions.Logging;
using System;
using System.Runtime.InteropServices;

namespace Librame.Extensions.Network.DotNetty
{
    /// <summary>
    /// 引导程序封装器工厂静态扩展。
    /// </summary>
    public static class BootstrapWrapperFactoryExtensions
    {
        /// <summary>
        /// 创建 UDP 协议封装器。
        /// </summary>
        /// <param name="factory">给定的 <see cref="IBootstrapWrapperFactory"/>。</param>
        /// <param name="group">输出 <see cref="IEventLoopGroup"/>。</param>
        /// <returns>返回 <see cref="IBootstrapWrapper"/>。</returns>
        public static IBootstrapWrapper CreateUdp(this IBootstrapWrapperFactory factory, out IEventLoopGroup group)
            => factory.NotNull(nameof(factory))
                .Create(false, out group)
                .Configure(bootstrap => bootstrap.Channel<SocketDatagramChannel>());


        /// <summary>
        /// 创建 TCP 协议封装器。
        /// </summary>
        /// <param name="factory">给定的 <see cref="IBootstrapWrapperFactory"/>。</param>
        /// <param name="useLibuv">使用 LIBUV。</param>
        /// <param name="group">输出 <see cref="IEventLoopGroup"/>。</param>
        /// <returns>返回 <see cref="IBootstrapWrapper"/>。</returns>
        public static IBootstrapWrapper CreateTcp(this IBootstrapWrapperFactory factory, bool useLibuv, out IEventLoopGroup group)
            => factory.NotNull(nameof(factory))
                .Create(useLibuv, out group)
                .Configure(bootstrap =>
                {
                    bootstrap.Option(ChannelOption.TcpNodelay, true);

                    if (useLibuv)
                        bootstrap.Channel<TcpChannel>();
                    else
                        bootstrap.Channel<TcpSocketChannel>();
                });

        /// <summary>
        /// 创建 TCP 协议服务端封装器。
        /// </summary>
        /// <param name="factory">给定的 <see cref="IBootstrapWrapperFactory"/>。</param>
        /// <param name="useLibuv">使用 LIBUV。</param>
        /// <param name="bossGroup">输出引领 <see cref="IEventLoopGroup"/>。</param>
        /// <param name="workerGroup">输出工作 <see cref="IEventLoopGroup"/>。</param>
        /// <returns>返回 <see cref="IServerBootstrapWrapper"/>。</returns>
        public static IServerBootstrapWrapper CreateTcpServer(this IBootstrapWrapperFactory factory, bool useLibuv,
            out IEventLoopGroup bossGroup, out IEventLoopGroup workerGroup)
            => factory.NotNull(nameof(factory))
                .CreateServer(useLibuv, out bossGroup, out workerGroup)
                .Configure(bootstrap =>
                {
                    if (useLibuv)
                    {
                        bootstrap.Channel<TcpServerChannel>();

                        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)
                            || RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                        {
                            bootstrap
                                .Option(ChannelOption.SoReuseport, true)
                                .ChildOption(ChannelOption.SoReuseaddr, true);

                            factory.Logger.LogInformation($"Run in {Environment.OSVersion.Platform}");
                        }
                    }
                    else
                    {
                        bootstrap.Channel<TcpServerSocketChannel>();
                    }
                });

    }
}
