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
    using Buffers;
    using Encryption;
    using Services;

    /// <summary>
    /// 抽象网络服务。
    /// </summary>
    /// <typeparam name="TService">指定的服务类型。</typeparam>
    public class AbstractNetworkService<TService> : AbstractService<TService, NetworkBuilderOptions>, INetworkService
        where TService : class, INetworkService
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractNetworkService{TService}"/> 实例。
        /// </summary>
        /// <param name="builderOptions">给定的 <see cref="IOptions{NetworkBuilderOptions}"/>。</param>
        /// <param name="logger">给定的 <see cref="ILogger{TService}"/>。</param>
        public AbstractNetworkService(IOptions<NetworkBuilderOptions> builderOptions, ILogger<TService> logger)
            : base(builderOptions, logger)
        {
        }


        /// <summary>
        /// 散列算法（默认不使用）。
        /// </summary>
        public IHashAlgorithmService Hash { get; set; }

        /// <summary>
        /// 字符编码（默认使用 <see cref="Encoding.UTF8"/>）。
        /// </summary>
        public Encoding Encoding { get; set; } = Encoding.UTF8;

        
        /// <summary>
        /// 尝试加密缓冲区。
        /// </summary>
        /// <param name="buffer">给定的 <see cref="IByteBuffer"/>。</param>
        /// <returns>返回是否加密的布尔值。</returns>
        protected virtual bool TryEncryptBuffer(IByteBuffer buffer)
        {
            if (Hash.IsDefault())
            {
                Logger.LogInformation($"{Hash} is null, encryption canceled.");
                return false;
            }

            Logger.LogInformation("encrypt buffer.");
            Hash.Rsa.Encrypt(buffer);
            return true;
        }

    }
}
