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
    /// 通道服务基类。
    /// </summary>
    public class ChannelServiceBase : AbstractService, IChannelService
    {
        /// <summary>
        /// 构造一个 <see cref="ChannelServiceBase"/> 实例。
        /// </summary>
        /// <param name="signingCredentials">给定的 <see cref="ISigningCredentialsService"/>。</param>
        /// <param name="options">给定的 <see cref="IOptions{DotNettyOptions}"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        public ChannelServiceBase(ISigningCredentialsService signingCredentials,
            IOptions<DotNettyOptions> options, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            SigningCredentials = signingCredentials.NotNull(nameof(signingCredentials));
            Options = options.NotNull(nameof(options)).Value;
        }


        /// <summary>
        /// 签名证书。
        /// </summary>
        /// <value>
        /// 返回 <see cref="ISigningCredentialsService"/>。
        /// </value>
        public ISigningCredentialsService SigningCredentials { get; }

        /// <summary>
        /// DotNetty 构建器选项。
        /// </summary>
        /// <value>返回 <see cref="DotNettyOptions"/>。</value>
        public DotNettyOptions Options { get; }
    }
}
