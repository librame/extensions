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
using System.Security.Cryptography;

namespace Librame.Extensions.Encryption
{
    using Buffers;
    using Builders;
    using Services;

    /// <summary>
    /// 内部散列算法服务。
    /// </summary>
    internal class InternalHashAlgorithmService : AbstractService<InternalHashAlgorithmService>, IHashAlgorithmService
    {
        /// <summary>
        /// 构造一个 <see cref="InternalHashAlgorithmService"/> 实例。
        /// </summary>
        /// <param name="rsa">给定的 <see cref="IRsaAlgorithmService"/>。</param>
        /// <param name="logger">给定的 <see cref="ILogger{InternalHashAlgorithmService}"/>。</param>
        public InternalHashAlgorithmService(IRsaAlgorithmService rsa, ILogger<InternalHashAlgorithmService> logger)
            : base(logger)
        {
            Rsa = rsa.NotDefault(nameof(rsa));
        }


        /// <summary>
        /// RSA 非对称算法。
        /// </summary>
        /// <value>
        /// 返回 <see cref="IRsaAlgorithmService"/>。
        /// </value>
        public IRsaAlgorithmService Rsa { get; }

        /// <summary>
        /// 构建器选项。
        /// </summary>
        /// <value>
        /// 返回 <see cref="EncryptionBuilderOptions"/>。
        /// </value>
        public EncryptionBuilderOptions BuilderOptions => Rsa.BuilderOptions;


        /// <summary>
        /// 计算散列。
        /// </summary>
        /// <param name="algorithm">给定的 <see cref="HashAlgorithm"/>。</param>
        /// <param name="buffer">给定要签名的 <see cref="IByteBuffer"/>。</param>
        /// <param name="isSigned">是否签名（默认不签名）。</param>
        /// <returns>返回签名的 <see cref="IByteBuffer"/>。</returns>
        protected virtual IByteBuffer ComputeHash(HashAlgorithm algorithm, IByteBuffer buffer, bool isSigned = false)
        {
            buffer.Change(memory =>
            {
                var hash = algorithm.ComputeHash(memory.ToArray());
                Logger.LogDebug($"Compute hash: {algorithm.GetType().Name}");

                return hash;
            });

            if (isSigned)
            {
                Rsa.SignHash(buffer);
                Logger.LogDebug($"Use rsa sign hash: {Rsa.SignHashAlgorithm.Name}");
            }

            return buffer;
        }

        
        /// <summary>
        /// 计算 MD5。
        /// </summary>
        /// <param name="buffer">给定的 <see cref="IByteBuffer"/>。</param>
        /// <param name="isSigned">是否签名（默认不签名）。</param>
        /// <returns>返回 <see cref="IByteBuffer"/>。</returns>
        public virtual IByteBuffer Md5(IByteBuffer buffer, bool isSigned = false)
        {
            return ComputeHash(MD5.Create(), buffer, isSigned);
        }

        
        /// <summary>
        /// 计算 SHA1。
        /// </summary>
        /// <param name="buffer">给定的 <see cref="IByteBuffer"/>。</param>
        /// <param name="isSigned">是否签名（默认不签名）。</param>
        /// <returns>返回 <see cref="IByteBuffer"/>。</returns>
        public virtual IByteBuffer Sha1(IByteBuffer buffer, bool isSigned = false)
        {
            return ComputeHash(SHA1.Create(), buffer, isSigned);
        }

        
        /// <summary>
        /// 计算 SHA256。
        /// </summary>
        /// <param name="buffer">给定的 <see cref="IByteBuffer"/>。</param>
        /// <param name="isSigned">是否签名（默认不签名）。</param>
        /// <returns>返回 <see cref="IByteBuffer"/>。</returns>
        public virtual IByteBuffer Sha256(IByteBuffer buffer, bool isSigned = false)
        {
            return ComputeHash(SHA256.Create(), buffer, isSigned);
        }

        
        /// <summary>
        /// 计算 SHA384。
        /// </summary>
        /// <param name="buffer">给定的 <see cref="IByteBuffer"/>。</param>
        /// <param name="isSigned">是否签名（默认不签名）。</param>
        /// <returns>返回 <see cref="IByteBuffer"/>。</returns>
        public virtual IByteBuffer Sha384(IByteBuffer buffer, bool isSigned = false)
        {
            return ComputeHash(SHA384.Create(), buffer, isSigned);
        }

        
        /// <summary>
        /// 计算 SHA512。
        /// </summary>
        /// <param name="buffer">给定的 <see cref="IByteBuffer"/>。</param>
        /// <param name="isSigned">是否签名（默认不签名）。</param>
        /// <returns>返回 <see cref="IByteBuffer"/>。</returns>
        public virtual IByteBuffer Sha512(IByteBuffer buffer, bool isSigned = false)
        {
            return ComputeHash(SHA512.Create(), buffer, isSigned);
        }

    }
}
