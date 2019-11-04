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
using System;
using System.Security.Cryptography.X509Certificates;

namespace Librame.Extensions.Network.DotNetty
{
    /// <summary>
    /// 服务器引导程序封装器接口。
    /// </summary>
    public interface IServerBootstrapWrapper : IBootstrapWrapper<ServerBootstrap, IServerChannel>
    {
        /// <summary>
        /// 增加信道处理程序。
        /// </summary>
        /// <typeparam name="TInitializeChannel">指定的初始化信道类型。</typeparam>
        /// <param name="tlsCertificate">给定的 <see cref="X509Certificate2"/>（可选）。</param>
        /// <param name="pipelineAction">给定的 <see cref="IChannelPipeline"/> 动作方法（可选）。</param>
        /// <param name="addTlsPipelineName">增加 TLS 管道名称（可选；默认不增加）。</param>
        /// <returns>返回 <see cref="IServerBootstrapWrapper"/>。</returns>
        IServerBootstrapWrapper AddChannelHandler<TInitializeChannel>(X509Certificate2 tlsCertificate = null,
            Action<IChannelPipeline> pipelineAction = null, bool addTlsPipelineName = false)
            where TInitializeChannel : IChannel;


        /// <summary>
        /// 配置服务器引导程序。
        /// </summary>
        /// <param name="configureAction">给定的配置动作。</param>
        /// <returns>返回 <see cref="IServerBootstrapWrapper"/>。</returns>
        IServerBootstrapWrapper Configure(Action<ServerBootstrap> configureAction);
    }
}
