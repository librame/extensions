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
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Librame.Extensions.Network.DotNetty
{
    /// <summary>
    /// 引导程序封装器接口。
    /// </summary>
    public interface IBootstrapWrapper : IBootstrapWrapper<Bootstrap, IChannel>
    {
        /// <summary>
        /// 异步增加信道处理程序。
        /// </summary>
        /// <typeparam name="TInitializeChannel">指定的初始化信道类型。</typeparam>
        /// <param name="tlsCertificate">给定的 <see cref="X509Certificate2"/>（可选）。</param>
        /// <param name="pipelineAction">给定的 <see cref="IChannelPipeline"/> 动作方法（可选）。</param>
        /// <param name="addTlsPipelineName">增加 TLS 管道名称（可选；默认不增加）。</param>
        /// <returns>返回 <see cref="IBootstrapWrapper"/>。</returns>
        IBootstrapWrapper AddChannelHandlerAsync<TInitializeChannel>(X509Certificate2 tlsCertificate = null,
            Action<IChannelPipeline> pipelineAction = null, bool addTlsPipelineName = false)
            where TInitializeChannel : IChannel;


        /// <summary>
        /// 配置引导程序。
        /// </summary>
        /// <param name="configureAction">给定的配置动作。</param>
        /// <returns>返回 <see cref="IBootstrapWrapper"/>。</returns>
        IBootstrapWrapper Configure(Action<Bootstrap> configureAction);


        /// <summary>
        /// 异步连接。
        /// </summary>
        /// <param name="host">给定的主机。</param>
        /// <param name="port">给定的端口。</param>
        /// <param name="retryCount">给定连接失败的重试次数（可选；默认重试 3 次）。</param>
        /// <returns>返回一个包含 <see cref="IChannel"/> 的异步操作。</returns>
        Task<IChannel> ConnectAsync(string host, int port, int retryCount = 3);

        /// <summary>
        /// 异步连接。
        /// </summary>
        /// <param name="endPoint">给定的 <see cref="IPEndPoint"/>。</param>
        /// <param name="retryCount">给定连接失败的重试次数（可选；默认重试 3 次）。</param>
        /// <returns>返回一个包含 <see cref="IChannel"/> 的异步操作。</returns>
        Task<IChannel> ConnectAsync(IPEndPoint endPoint, int retryCount = 3);
    }


    /// <summary>
    /// 引导程序封装器接口。
    /// </summary>
    /// <typeparam name="TBootstrap">指定的引导程序类型。</typeparam>
    /// <typeparam name="TChannel">指定的信道类型。</typeparam>
    public interface IBootstrapWrapper<out TBootstrap, in TChannel>
        where TBootstrap : AbstractBootstrap<TBootstrap, TChannel>
        where TChannel : IChannel
    {
        /// <summary>
        /// 日志。
        /// </summary>
        ILogger Logger { get; }


        /// <summary>
        /// 异步绑定。
        /// </summary>
        /// <param name="port">给定的端口。</param>
        /// <returns>返回一个包含 <see cref="IChannel"/> 的异步操作。</returns>
        Task<IChannel> BindAsync(int port);

        /// <summary>
        /// 异步绑定。
        /// </summary>
        /// <param name="host">给定的主机。</param>
        /// <param name="port">给定的端口。</param>
        /// <returns>返回一个包含 <see cref="IChannel"/> 的异步操作。</returns>
        Task<IChannel> BindAsync(string host, int port);

        /// <summary>
        /// 异步绑定。
        /// </summary>
        /// <param name="endPoint">给定的 <see cref="IPEndPoint"/>。</param>
        /// <returns>返回一个包含 <see cref="IChannel"/> 的异步操作。</returns>
        Task<IChannel> BindAsync(IPEndPoint endPoint);
    }
}
