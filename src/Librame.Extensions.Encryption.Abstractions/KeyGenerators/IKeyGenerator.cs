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
    /// 密钥生成器接口。
    /// </summary>
    public interface IKeyGenerator : IEncryptionService
    {
        /// <summary>
        /// 获取 64 位密钥。
        /// </summary>
        /// <param name="identifier">给定的标识符（可选；默认使用选项配置。详情可参考 <see cref="AlgorithmIdentifier"/>）。</param>
        /// <returns>返回 <see cref="IBuffer{T}"/>。</returns>
        IBuffer<byte> GetKey64(string identifier = null);

        /// <summary>
        /// 获取 128 位密钥。
        /// </summary>
        /// <param name="identifier">给定的标识符（可选；默认使用选项配置。详情可参考 <see cref="AlgorithmIdentifier"/>）。</param>
        /// <returns>返回 <see cref="IBuffer{T}"/>。</returns>
        IBuffer<byte> GetKey128(string identifier = null);

        /// <summary>
        /// 获取 192 位密钥。
        /// </summary>
        /// <param name="identifier">给定的标识符（可选；默认使用选项配置。详情可参考 <see cref="AlgorithmIdentifier"/>）。</param>
        /// <returns>返回 <see cref="IBuffer{T}"/>。</returns>
        IBuffer<byte> GetKey192(string identifier = null);

        /// <summary>
        /// 获取 256 位密钥。
        /// </summary>
        /// <param name="identifier">给定的标识符（可选；默认使用选项配置。详情可参考 <see cref="AlgorithmIdentifier"/>）。</param>
        /// <returns>返回 <see cref="IBuffer{T}"/>。</returns>
        IBuffer<byte> GetKey256(string identifier = null);

        /// <summary>
        /// 获取 384 位密钥。
        /// </summary>
        /// <param name="identifier">给定的标识符（可选；默认使用选项配置。详情可参考 <see cref="AlgorithmIdentifier"/>）。</param>
        /// <returns>返回 <see cref="IBuffer{T}"/>。</returns>
        IBuffer<byte> GetKey384(string identifier = null);

        /// <summary>
        /// 获取 512 位密钥。
        /// </summary>
        /// <param name="identifier">给定的标识符（可选；默认使用选项配置。详情可参考 <see cref="AlgorithmIdentifier"/>）。</param>
        /// <returns>返回 <see cref="IBuffer{T}"/>。</returns>
        IBuffer<byte> GetKey512(string identifier = null);

        /// <summary>
        /// 获取 1024 位密钥。
        /// </summary>
        /// <param name="identifier">给定的标识符（可选；默认使用选项配置。详情可参考 <see cref="AlgorithmIdentifier"/>）。</param>
        /// <returns>返回 <see cref="IBuffer{T}"/>。</returns>
        IBuffer<byte> GetKey1024(string identifier = null);

        /// <summary>
        /// 获取 2048 位密钥。
        /// </summary>
        /// <param name="identifier">给定的标识符（可选；默认使用选项配置。详情可参考 <see cref="AlgorithmIdentifier"/>）。</param>
        /// <returns>返回 <see cref="IBuffer{T}"/>。</returns>
        IBuffer<byte> GetKey2048(string identifier = null);
    }
}
