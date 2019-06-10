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
    using Core;
    using Encryption;

    /// <summary>
    /// 通道服务接口。
    /// </summary>
    public interface IChannelService : IService
    {
        /// <summary>
        /// 签名证书。
        /// </summary>
        /// <value>
        /// 返回 <see cref="ISigningCredentialsService"/>。
        /// </value>
        ISigningCredentialsService SigningCredentials { get; }

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
        /// 返回 <see cref="DotNettyOptions"/>。
        /// </value>
        DotNettyOptions Options { get; }
    }
}
