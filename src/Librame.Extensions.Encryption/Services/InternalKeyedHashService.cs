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
    using Core;

    /// <summary>
    /// 内部键控散列服务。
    /// </summary>
    internal class InternalKeyedHashService : ExtensionBuilderServiceBase<EncryptionBuilderOptions>, IKeyedHashService
    {
        /// <summary>
        /// 构造一个 <see cref="InternalKeyedHashService"/> 实例。
        /// </summary>
        /// <param name="keyGenerator">给定的 <see cref="IKeyGenerator"/>。</param>
        public InternalKeyedHashService(IKeyGenerator keyGenerator)
            : base(keyGenerator.CastTo<IKeyGenerator, ExtensionBuilderServiceBase<EncryptionBuilderOptions>>(nameof(keyGenerator)))
        {
            KeyGenerator = keyGenerator;
        }


        /// <summary>
        /// 密钥生成器。
        /// </summary>
        /// <value>
        /// 返回 <see cref="IKeyGenerator"/>。
        /// </value>
        public IKeyGenerator KeyGenerator { get; }


        /// <summary>
        /// 计算哈希。
        /// </summary>
        /// <param name="buffer">给定的 <see cref="IBuffer{T}"/>。</param>
        /// <param name="hmac">给定的 <see cref="HMAC"/>。</param>
        /// <returns>返回 <see cref="IBuffer{T}"/>。</returns>
        private IByteBuffer ComputeHash(IByteBuffer buffer, HMAC hmac)
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
        public IByteBuffer HmacMd5(IByteBuffer buffer, string identifier = null)
        {
            var key = KeyGenerator.GetHmacMd5Key(identifier);
            var hash = new HMACMD5(key);

            return ComputeHash(buffer, hash);
        }


        /// <summary>
        /// 计算 HMACSHA1。
        /// </summary>
        /// <param name="buffer">给定的 <see cref="IBuffer{T}"/>。</param>
        /// <param name="identifier">给定的标识符（可选；默认使用选项配置。详情可参考 <see cref="AlgorithmIdentifier"/>）。</param>
        /// <returns>返回 <see cref="IBuffer{T}"/>。</returns>
        public IByteBuffer HmacSha1(IByteBuffer buffer, string identifier = null)
        {
            var key = KeyGenerator.GetHmacSha1Key(identifier);
            var hash = new HMACSHA1(key);

            return ComputeHash(buffer, hash);
        }


        /// <summary>
        /// 计算 HMACSHA256。
        /// </summary>
        /// <param name="buffer">给定的 <see cref="IBuffer{T}"/>。</param>
        /// <param name="identifier">给定的标识符（可选；默认使用选项配置。详情可参考 <see cref="AlgorithmIdentifier"/>）。</param>
        /// <returns>返回 <see cref="IBuffer{T}"/>。</returns>
        public IByteBuffer HmacSha256(IByteBuffer buffer, string identifier = null)
        {
            var key = KeyGenerator.GetHmacSha256Key(identifier);
            var hash = new HMACSHA256(key);

            return ComputeHash(buffer, hash);
        }


        /// <summary>
        /// 计算 HMACSHA384。
        /// </summary>
        /// <param name="buffer">给定的 <see cref="IBuffer{T}"/>。</param>
        /// <param name="identifier">给定的标识符（可选；默认使用选项配置。详情可参考 <see cref="AlgorithmIdentifier"/>）。</param>
        /// <returns>返回 <see cref="IBuffer{T}"/>。</returns>
        public IByteBuffer HmacSha384(IByteBuffer buffer, string identifier = null)
        {
            var key = KeyGenerator.GetHmacSha384Key(identifier);
            var hash = new HMACSHA384(key);

            return ComputeHash(buffer, hash);
        }


        /// <summary>
        /// 计算 HMACSHA512。
        /// </summary>
        /// <param name="buffer">给定的 <see cref="IBuffer{T}"/>。</param>
        /// <param name="identifier">给定的标识符（可选；默认使用选项配置。详情可参考 <see cref="AlgorithmIdentifier"/>）。</param>
        /// <returns>返回 <see cref="IBuffer{T}"/>。</returns>
        public IByteBuffer HmacSha512(IByteBuffer buffer, string identifier = null)
        {
            var key = KeyGenerator.GetHmacSha512Key(identifier);
            var hash = new HMACSHA512(key);

            return ComputeHash(buffer, hash);
        }

    }
}
