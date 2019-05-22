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
    /// <typeparam name="TService">指定的服务类型。</typeparam>
    public abstract class AbstractChannelService<TService> : AbstractService<TService>, IChannelService
        where TService : class, IChannelService
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractChannelService{TService}"/> 实例。
        /// </summary>
        /// <param name="signingCredentials">给定的 <see cref="ISigningCredentialsService"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        /// <param name="options">给定的 <see cref="IOptions{ChannelOptions}"/>。</param>
        public AbstractChannelService(ISigningCredentialsService signingCredentials,
            ILoggerFactory loggerFactory, IOptions<ChannelOptions> options)
            : base(loggerFactory.CreateLogger<TService>())
        {
            SigningCredentials = signingCredentials.NotNull(nameof(signingCredentials));
            LoggerFactory = loggerFactory.NotNull(nameof(loggerFactory));
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
        /// 日志工厂。
        /// </summary>
        /// <value>
        /// 返回 <see cref="ILoggerFactory"/>。
        /// </value>
        public ILoggerFactory LoggerFactory { get; }
        
        /// <summary>
        /// 通道选项。
        /// </summary>
        /// <value>
        /// 返回 <see cref="ChannelOptions"/>。
        /// </value>
        public ChannelOptions Options { get; }
    }
}
