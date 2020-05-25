#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using Microsoft.Extensions.Logging;

namespace Librame.Extensions.Network.DotNetty
{
    /// <summary>
    /// 引导程序封装器工厂接口。
    /// </summary>
    public interface IBootstrapWrapperFactory
    {
        /// <summary>
        /// 日志。
        /// </summary>
        ILogger Logger { get; }


        /// <summary>
        /// 创建封装器。
        /// </summary>
        /// <param name="useLibuv">使用 LIBUV。</param>
        /// <param name="group">输出 <see cref="IEventLoopGroup"/>。</param>
        /// <returns>返回 <see cref="IBootstrapWrapper"/>。</returns>
        IBootstrapWrapper Create(bool useLibuv, out IEventLoopGroup group);

        /// <summary>
        /// 创建服务端封装器。
        /// </summary>
        /// <param name="useLibuv">使用 LIBUV。</param>
        /// <param name="bossGroup">输出引领 <see cref="IEventLoopGroup"/>。</param>
        /// <param name="workerGroup">输出工作 <see cref="IEventLoopGroup"/>。</param>
        /// <returns>返回 <see cref="IServerBootstrapWrapper"/>。</returns>
        IServerBootstrapWrapper CreateServer(bool useLibuv,
            out IEventLoopGroup bossGroup, out IEventLoopGroup workerGroup);


        /// <summary>
        /// 创建泛型封装器。
        /// </summary>
        /// <param name="useLibuv">使用 LIBUV。</param>
        /// <param name="group">输出 <see cref="IEventLoopGroup"/>。</param>
        /// <returns>返回 <see cref="IBootstrapWrapper{TBootstrap, TChannel}"/>。</returns>
        IBootstrapWrapper<Bootstrap, IChannel> CreateGeneric(bool useLibuv,
            out IEventLoopGroup group);

        /// <summary>
        /// 创建服务端泛型封装器。
        /// </summary>
        /// <param name="useLibuv">使用 LIBUV。</param>
        /// <param name="bossGroup">输出引领 <see cref="IEventLoopGroup"/>。</param>
        /// <param name="workerGroup">输出工作 <see cref="IEventLoopGroup"/>。</param>
        /// <returns>返回 <see cref="IBootstrapWrapper{ServerBootstrap, IServerChannel}"/>。</returns>
        IBootstrapWrapper<ServerBootstrap, IServerChannel> CreateServerGeneric(bool useLibuv,
            out IEventLoopGroup bossGroup, out IEventLoopGroup workerGroup);
    }
}
