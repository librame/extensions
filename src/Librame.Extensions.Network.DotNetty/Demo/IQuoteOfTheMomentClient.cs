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
    /// QuoteOfTheMoment 客户端接口。
    /// </summary>
    public interface IQuoteOfTheMomentClient : IChannelService
    {
        /// <summary>
        /// 异步启动。
        /// </summary>
        /// <param name="configureProcess">给定的配置处理方法。</param>
        /// <returns>返回一个异步操作。</returns>
        Task StartAsync(Action<IChannel> configureProcess);

        /// <summary>
        /// 异步启动。
        /// </summary>
        /// <typeparam name="TChannelHandler">指定的信道处理程序类型。</typeparam>
        /// <param name="channelHandler">给定的 <see cref="IChannelHandler"/>。</param>
        /// <param name="configureProcess">给定的配置处理方法。</param>
        /// <returns>返回一个异步操作。</returns>
        Task StartAsync<TChannelHandler>(TChannelHandler channelHandler,
            Action<IChannel> configureProcess)
            where TChannelHandler : IChannelHandler;
    }
}
