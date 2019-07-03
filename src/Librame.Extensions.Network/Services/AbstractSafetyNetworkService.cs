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
    using Core;
    using Encryption;

    /// <summary>
    /// 抽象安全网络服务。
    /// </summary>
    public abstract class AbstractSafetyNetworkService : AbstractNetworkService, ISafetyNetworkService
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractSafetyNetworkService"/> 实例。
        /// </summary>
        /// <param name="hash">给定的 <see cref="IHashService"/>。</param>
        /// <param name="coreOptions">给定的 <see cref="IOptions{CoreBuilderOptions}"/>。</param>
        /// <param name="options">给定的 <see cref="IOptions{NetworkBuilderOptions}"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        public AbstractSafetyNetworkService(IHashService hash, IOptions<CoreBuilderOptions> coreOptions,
            IOptions<NetworkBuilderOptions> options, ILoggerFactory loggerFactory)
            : base(coreOptions, options, loggerFactory)
        {
            Hash = hash.NotNull(nameof(hash));
        }


        /// <summary>
        /// 散列算法。
        /// </summary>
        public IHashService Hash { get; }


        /// <summary>
        /// 准备要发送的数据。
        /// </summary>
        /// <param name="message">给定的消息。</param>
        /// <returns>返回字节数组。</returns>
        protected byte[] ReadySendData(string message)
        {
            return ReadySendData(Encoding.GetBytes(message));
        }
        /// <summary>
        /// 准备要发送的数据。
        /// </summary>
        /// <param name="bytes">给定的字节数组。</param>
        /// <returns>返回字节数组。</returns>
        protected byte[] ReadySendData(byte[] bytes)
        {
            if (!Options.Enciphered)
            {
                Logger.LogInformation("Encipher canceled.");
                return bytes;
            }

            var buffer = bytes.AsByteBuffer();
            Hash.Rsa.Encrypt(buffer);
            Logger.LogInformation("Encipher buffer.");

            return buffer.Memory.ToArray();
        }

    }
}
