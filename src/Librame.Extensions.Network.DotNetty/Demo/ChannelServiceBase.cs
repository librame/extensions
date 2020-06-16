#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.Logging;

namespace Librame.Extensions.Network.DotNetty.Demo
{
    using Core.Builders;
    using Core.Services;
    using Encryption.Services;
    using Network.Builders;

    /// <summary>
    /// 信道服务基类。
    /// </summary>
    public class ChannelServiceBase : AbstractService, IChannelService
    {
        /// <summary>
        /// 构造一个 <see cref="ChannelServiceBase"/>。
        /// </summary>
        /// <param name="wrapperFactory">给定的 <see cref="IBootstrapWrapperFactory"/>。</param>
        /// <param name="signingCredentials">给定的 <see cref="ISigningCredentialsService"/>。</param>
        /// <param name="dependency">给定的 <see cref="DotNettyDependency"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        public ChannelServiceBase(IBootstrapWrapperFactory wrapperFactory,
            ISigningCredentialsService signingCredentials,
            DotNettyDependency dependency,
            ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            WrapperFactory = wrapperFactory.NotNull(nameof(wrapperFactory));
            SigningCredentials = signingCredentials.NotNull(nameof(signingCredentials));
            Dependency = dependency.NotNull(nameof(dependency));
        }


        /// <summary>
        /// 引导程序封装器工厂。
        /// </summary>
        /// <value>
        /// 返回 <see cref="IBootstrapWrapperFactory"/>。
        /// </value>
        public IBootstrapWrapperFactory WrapperFactory { get; }

        /// <summary>
        /// 签名证书。
        /// </summary>
        /// <value>
        /// 返回 <see cref="ISigningCredentialsService"/>。
        /// </value>
        public ISigningCredentialsService SigningCredentials { get; }

        /// <summary>
        /// DotNetty 依赖。
        /// </summary>
        public DotNettyDependency Dependency { get; }

        /// <summary>
        /// DotNetty 构建器选项。
        /// </summary>
        /// <value>返回 <see cref="DotNettyOptions"/>。</value>
        public DotNettyOptions Options
            => Dependency.Options;

        /// <summary>
        /// 核心构建器选项。
        /// </summary>
        /// <value>返回 <see cref="CoreBuilderOptions"/>。</value>
        public CoreBuilderOptions CoreOptions
            => Dependency.GetRequiredDependency<CoreBuilderDependency>().Options;
    }
}
