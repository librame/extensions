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
    using Services;

    /// <summary>
    /// 内部键控散列算法服务。
    /// </summary>
    internal class InternalKeyedHashAlgorithmService : AbstractService<InternalKeyedHashAlgorithmService>, IKeyedHashAlgorithmService
    {
        /// <summary>
        /// 构造一个 <see cref="InternalKeyedHashAlgorithmService"/> 实例。
        /// </summary>
        /// <param name="keyGenerator">给定的 <see cref="IKeyGenerator"/>。</param>
        /// <param name="logger">给定的 <see cref="ILogger{InternalKeyedHashAlgorithmService}"/>。</param>
        public InternalKeyedHashAlgorithmService(IKeyGenerator keyGenerator, ILogger<InternalKeyedHashAlgorithmService> logger)
            : base(logger)
        {
            KeyGenerator = keyGenerator.NotDefault(nameof(keyGenerator));
        }


        /// <summary>
        /// 密钥生成器。
        /// </summary>
        /// <value>
        /// 返回 <see cref="IKeyGenerator"/>。
        /// </value>
        public IKeyGenerator KeyGenerator { get; }

        /// <summary>
        /// 构建器选项。
        /// </summary>
        /// <value>
        /// 返回 <see cref="EncryptionBuilderOptions"/>。
        /// </value>
        public EncryptionBuilderOptions BuilderOptions => KeyGenerator.BuilderOptions;


        /// <summary>
        /// 计算哈希。
        /// </summary>
        /// <param name="buffer">给定的 <see cref="IBuffer{T}"/>。</param>
        /// <param name="hmac">给定的 <see cref="HMAC"/>。</param>
        /// <returns>返回 <see cref="IBuffer{T}"/>。</returns>
        protected IByteBuffer ComputeHash(IByteBuffer buffer, HMAC hmac)
        {
            return buffer.Change(memory =>
            {
                var hash = hmac.ComputeHash(memory.ToArray());
                Logger.LogDebug($"Compute HMAC hash: {hmac.HashName}");

                return hash;
            });
        }


        /// <summary>
        /// 计算 HMACMD5。
        /// </summary>
        /// <param name="buffer">给定的 <see cref="IBuffer{T}"/>。</param>
        /// <param name="identifier">给定的标识符（可选；默认使用选项配置。详情可参考 <see cref="AlgorithmIdentifier"/>）。</param>
        /// <returns>返回 <see cref="IBuffer{T}"/>。</returns>
        public virtual IByteBuffer HmacMd5(IByteBuffer buffer, string identifier = null)
        {
            var keyBuffer = KeyGenerator.GetKey512(identifier);
            var hash = new HMACMD5(keyBuffer.Memory.ToArray());

            return ComputeHash(buffer, hash);
        }


        /// <summary>
        /// 计算 HMACSHA1。
        /// </summary>
        /// <param name="buffer">给定的 <see cref="IBuffer{T}"/>。</param>
        /// <param name="identifier">给定的标识符（可选；默认使用选项配置。详情可参考 <see cref="AlgorithmIdentifier"/>）。</param>
        /// <returns>返回 <see cref="IBuffer{T}"/>。</returns>
        public virtual IByteBuffer HmacSha1(IByteBuffer buffer, string identifier = null)
        {
            var keyBuffer = KeyGenerator.GetKey512(identifier);
            var hash = new HMACSHA1(keyBuffer.Memory.ToArray());

            return ComputeHash(buffer, hash);
        }


        /// <summary>
        /// 计算 HMACSHA256。
        /// </summary>
        /// <param name="buffer">给定的 <see cref="IBuffer{T}"/>。</param>
        /// <param name="identifier">给定的标识符（可选；默认使用选项配置。详情可参考 <see cref="AlgorithmIdentifier"/>）。</param>
        /// <returns>返回 <see cref="IBuffer{T}"/>。</returns>
        public virtual IByteBuffer HmacSha256(IByteBuffer buffer, string identifier = null)
        {
            var keyBuffer = KeyGenerator.GetKey512(identifier);
            var hash = new HMACSHA256(keyBuffer.Memory.ToArray());

            return ComputeHash(buffer, hash);
        }


        /// <summary>
        /// 计算 HMACSHA384。
        /// </summary>
        /// <param name="buffer">给定的 <see cref="IBuffer{T}"/>。</param>
        /// <param name="identifier">给定的标识符（可选；默认使用选项配置。详情可参考 <see cref="AlgorithmIdentifier"/>）。</param>
        /// <returns>返回 <see cref="IBuffer{T}"/>。</returns>
        public virtual IByteBuffer HmacSha384(IByteBuffer buffer, string identifier = null)
        {
            var keyBuffer = KeyGenerator.GetKey1024(identifier);
            var hash = new HMACSHA384(keyBuffer.Memory.ToArray());

            return ComputeHash(buffer, hash);
        }


        /// <summary>
        /// 计算 HMACSHA512。
        /// </summary>
        /// <param name="buffer">给定的 <see cref="IBuffer{T}"/>。</param>
        /// <param name="identifier">给定的标识符（可选；默认使用选项配置。详情可参考 <see cref="AlgorithmIdentifier"/>）。</param>
        /// <returns>返回 <see cref="IBuffer{T}"/>。</returns>
        public virtual IByteBuffer HmacSha512(IByteBuffer buffer, string identifier = null)
        {
            var keyBuffer = KeyGenerator.GetKey1024(identifier);
            var hash = new HMACSHA512(keyBuffer.Memory.ToArray());

            return ComputeHash(buffer, hash);
        }

    }
}
