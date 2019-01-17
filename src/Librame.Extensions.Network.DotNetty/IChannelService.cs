#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.Logging;

namespace Librame.Extensions.Network.DotNetty
{
    using Encryption;
    using Services;

    /// <summary>
    /// 通道服务接口。
    /// </summary>
    public interface IChannelService : IService
    {
        /// <summary>
        /// 签名证书提供程序。
        /// </summary>
        /// <value>
        /// 返回 <see cref="ISigningCredentialsProvider"/>。
        /// </value>
        ISigningCredentialsProvider Provider { get; }

        /// <summary>
        /// 日志工厂。
        /// </summary>
        /// <value>
        /// 返回 <see cref="ILoggerFactory"/>。
        /// </value>
        ILoggerFactory LoggerFactory { get; }

        /// <summary>
        /// 通道选项。
        /// </summary>
        /// <value>
        /// 返回 <see cref="ChannelOptions"/>。
        /// </value>
        ChannelOptions Options { get; }
    }
}
