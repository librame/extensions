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
using System.Text;

namespace Librame.Extensions.Network
{
    using Encryption;
    using Services;

    /// <summary>
    /// 抽象网络服务。
    /// </summary>
    /// <typeparam name="TService">指定的服务类型。</typeparam>
    public class AbstractNetworkService<TService> : AbstractService<TService>, INetworkService
        where TService : class, IService
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractNetworkService{TService}"/> 实例。
        /// </summary>
        /// <param name="hash">给定的 <see cref="IHashAlgorithmService"/>。</param>
        /// <param name="options">给定的 <see cref="IOptions{DefaultNetworkBuilderOptions}"/>。</param>
        /// <param name="logger">给定的 <see cref="ILogger{TService}"/>。</param>
        public AbstractNetworkService(IHashAlgorithmService hash, IOptions<NetworkBuilderOptions> options, ILogger<TService> logger)
            : base(logger)
        {
            Hash = hash;
            Options = options.Value;
        }


        /// <summary>
        /// 散列算法。
        /// </summary>
        /// <value>
        /// 返回 <see cref="IHashAlgorithmService"/>。
        /// </value>
        public IHashAlgorithmService Hash { get; }

        /// <summary>
        /// 构建器选项。
        /// </summary>
        /// <value>
        /// 返回 <see cref="NetworkBuilderOptions"/>。
        /// </value>
        public NetworkBuilderOptions Options { get; }

        /// <summary>
        /// 字符编码（默认使用 <see cref="Encoding.UTF8"/>）。
        /// </summary>
        public Encoding Encoding { get; set; } = Encoding.UTF8;
    }
}
