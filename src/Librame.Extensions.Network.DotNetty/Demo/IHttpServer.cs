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
using System;
using System.Threading.Tasks;

namespace Librame.Extensions.Network.DotNetty.Demo
{
    /// <summary>
    /// HTTP 服务端接口。
    /// </summary>
    public interface IHttpServer : IChannelService
    {
        /// <summary>
        /// 异步启动。
        /// </summary>
        /// <param name="configureProcess">给定的配置处理方法。</param>
        /// <param name="host">给定要启动的主机（可选；默认使用选项配置）。</param>
        /// <param name="port">给定要启动的端口（可选；默认使用选项配置）。</param>
        /// <returns>返回一个异步操作。</returns>
        Task StartAsync(Action<IChannel> configureProcess, string host = null, int? port = null);

        /// <summary>
        /// 异步启动。
        /// </summary>
        /// <typeparam name="TChannelHandler">指定的信道处理程序类型。</typeparam>
        /// <param name="channelHandler">给定的 <see cref="IChannelHandler"/>。</param>
        /// <param name="configureProcess">给定的配置处理方法。</param>
        /// <param name="host">给定要启动的主机（可选；默认使用选项配置）。</param>
        /// <param name="port">给定要启动的端口（可选；默认使用选项配置）。</param>
        /// <returns>返回一个异步操作。</returns>
        Task StartAsync<TChannelHandler>(TChannelHandler channelHandler,
            Action<IChannel> configureProcess, string host = null, int? port = null)
            where TChannelHandler : IChannelHandler;
    }
}
