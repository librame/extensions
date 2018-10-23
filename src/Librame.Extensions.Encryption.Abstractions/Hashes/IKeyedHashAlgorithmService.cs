#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Encryption
{
    using Buffers;

    /// <summary>
    /// 键控散列算法服务接口。
    /// </summary>
    public interface IKeyedHashAlgorithmService : IEncryptionService
    {
        /// <summary>
        /// 密钥生成器。
        /// </summary>
        /// <value>
        /// 返回 <see cref="IKeyGenerator"/>。
        /// </value>
        IKeyGenerator KeyGenerator { get; }


        /// <summary>
        /// 计算 HMACMD5。
        /// </summary>
        /// <param name="buffer">给定的 <see cref="IBuffer{T}"/>。</param>
        /// <param name="identifier">给定的标识符（可选；默认使用选项配置。详情可参考 <see cref="AlgorithmIdentifier"/>）。</param>
        /// <returns>返回 <see cref="IBuffer{T}"/>。</returns>
        IBuffer<byte> HmacMd5(IBuffer<byte> buffer, string identifier = null);


        /// <summary>
        /// 计算 HMACSHA1。
        /// </summary>
        /// <param name="buffer">给定的 <see cref="IBuffer{T}"/>。</param>
        /// <param name="identifier">给定的标识符（可选；默认使用选项配置。详情可参考 <see cref="AlgorithmIdentifier"/>）。</param>
        /// <returns>返回 <see cref="IBuffer{T}"/>。</returns>
        IBuffer<byte> HmacSha1(IBuffer<byte> buffer, string identifier = null);


        /// <summary>
        /// 计算 HMACSHA256。
        /// </summary>
        /// <param name="buffer">给定的 <see cref="IBuffer{T}"/>。</param>
        /// <param name="identifier">给定的标识符（可选；默认使用选项配置。详情可参考 <see cref="AlgorithmIdentifier"/>）。</param>
        /// <returns>返回 <see cref="IBuffer{T}"/>。</returns>
        IBuffer<byte> HmacSha256(IBuffer<byte> buffer, string identifier = null);


        /// <summary>
        /// 计算 HMACSHA384。
        /// </summary>
        /// <param name="buffer">给定的 <see cref="IBuffer{T}"/>。</param>
        /// <param name="identifier">给定的标识符（可选；默认使用选项配置。详情可参考 <see cref="AlgorithmIdentifier"/>）。</param>
        /// <returns>返回 <see cref="IBuffer{T}"/>。</returns>
        IBuffer<byte> HmacSha384(IBuffer<byte> buffer, string identifier = null);


        /// <summary>
        /// 计算 HMACSHA512。
        /// </summary>
        /// <param name="buffer">给定的 <see cref="IBuffer{T}"/>。</param>
        /// <param name="identifier">给定的标识符（可选；默认使用选项配置。详情可参考 <see cref="AlgorithmIdentifier"/>）。</param>
        /// <returns>返回 <see cref="IBuffer{T}"/>。</returns>
        IBuffer<byte> HmacSha512(IBuffer<byte> buffer, string identifier = null);
    }
}
