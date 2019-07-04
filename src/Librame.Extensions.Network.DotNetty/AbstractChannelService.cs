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
using Microsoft.Extensions.Options;

namespace Librame.Extensions.Network.DotNetty
{
    using Core;
    using Encryption;

    /// <summary>
    /// 抽象通道服务。
    /// </summary>
    public abstract class AbstractChannelService : AbstractService<DotNettyOptions>, IChannelService
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractChannelService"/> 实例。
        /// </summary>
        /// <param name="signingCredentials">给定的 <see cref="ISigningCredentialsService"/>。</param>
        /// <param name="options">给定的 <see cref="IOptions{DotNettyOptions}"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        public AbstractChannelService(ISigningCredentialsService signingCredentials,
            IOptions<DotNettyOptions> options, ILoggerFactory loggerFactory)
            : base(options, loggerFactory)
        {
            SigningCredentials = signingCredentials.NotNull(nameof(signingCredentials));
        }


        /// <summary>
        /// 签名证书。
        /// </summary>
        /// <value>
        /// 返回 <see cref="ISigningCredentialsService"/>。
        /// </value>
        public ISigningCredentialsService SigningCredentials { get; }
    }
}
