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
    /// 抽象网络服务。
    /// </summary>
    /// <typeparam name="TService">指定的服务类型。</typeparam>
    public class AbstractNetworkService<TService> : AbstractService<TService>, INetworkService
        where TService : class, INetworkService
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractNetworkService{TService}"/> 实例。
        /// </summary>
        /// <param name="hash">给定的 <see cref="IHashService"/>。</param>
        /// <param name="options">给定的 <see cref="IOptions{NetworkBuilderOptions}"/>。</param>
        /// <param name="logger">给定的 <see cref="ILogger{TService}"/>。</param>
        public AbstractNetworkService(IHashService hash,
            IOptions<NetworkBuilderOptions> options, ILogger<TService> logger)
            : base(logger)
        {
            Hash = hash.NotNull(nameof(hash));
            Options = options.NotNull(nameof(options)).Value;
        }


        /// <summary>
        /// 散列算法。
        /// </summary>
        public IHashService Hash { get; }

        /// <summary>
        /// 构建器选项。
        /// </summary>
        public NetworkBuilderOptions Options { get; }

        /// <summary>
        /// 字符编码（默认使用 <see cref="Encoding.UTF8"/>）。
        /// </summary>
        public Encoding Encoding { get; set; } = Encoding.UTF8;


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
