#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Network.DotNetty
{
    using Builders;
    using Core.Builders;
    using Core.Services;
    using Encryption.Services;

    /// <summary>
    /// 信道服务接口。
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
        /// DotNetty 构建器选项。
        /// </summary>
        /// <value>返回 <see cref="DotNettyOptions"/>。</value>
        DotNettyOptions Options { get; }

        /// <summary>
        /// 核心构建器选项。
        /// </summary>
        /// <value>返回 <see cref="CoreBuilderOptions"/>。</value>
        public CoreBuilderOptions CoreOptions { get; }
    }
}
