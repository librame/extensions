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
    using Core;

    /// <summary>
    /// 键控散列服务接口。
    /// </summary>
    public interface IKeyedHashService : IService
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
        /// <param name="buffer">给定的 <see cref="IByteMemoryBuffer"/>。</param>
        /// <param name="identifier">给定的 <see cref="UniqueAlgorithmIdentifier"/>（可选；默认使用选项配置）。</param>
        /// <returns>返回 <see cref="IByteMemoryBuffer"/>。</returns>
        IByteMemoryBuffer HmacMd5(IByteMemoryBuffer buffer, UniqueAlgorithmIdentifier identifier = null);


        /// <summary>
        /// 计算 HMACSHA1。
        /// </summary>
        /// <param name="buffer">给定的 <see cref="IByteMemoryBuffer"/>。</param>
        /// <param name="identifier">给定的 <see cref="UniqueAlgorithmIdentifier"/>（可选；默认使用选项配置）。</param>
        /// <returns>返回 <see cref="IByteMemoryBuffer"/>。</returns>
        IByteMemoryBuffer HmacSha1(IByteMemoryBuffer buffer, UniqueAlgorithmIdentifier identifier = null);


        /// <summary>
        /// 计算 HMACSHA256。
        /// </summary>
        /// <param name="buffer">给定的 <see cref="IByteMemoryBuffer"/>。</param>
        /// <param name="identifier">给定的 <see cref="UniqueAlgorithmIdentifier"/>（可选；默认使用选项配置）。</param>
        /// <returns>返回 <see cref="IByteMemoryBuffer"/>。</returns>
        IByteMemoryBuffer HmacSha256(IByteMemoryBuffer buffer, UniqueAlgorithmIdentifier identifier = null);


        /// <summary>
        /// 计算 HMACSHA384。
        /// </summary>
        /// <param name="buffer">给定的 <see cref="IByteMemoryBuffer"/>。</param>
        /// <param name="identifier">给定的 <see cref="UniqueAlgorithmIdentifier"/>（可选；默认使用选项配置）。</param>
        /// <returns>返回 <see cref="IByteMemoryBuffer"/>。</returns>
        IByteMemoryBuffer HmacSha384(IByteMemoryBuffer buffer, UniqueAlgorithmIdentifier identifier = null);


        /// <summary>
        /// 计算 HMACSHA512。
        /// </summary>
        /// <param name="buffer">给定的 <see cref="IByteMemoryBuffer"/>。</param>
        /// <param name="identifier">给定的 <see cref="UniqueAlgorithmIdentifier"/>（可选；默认使用选项配置）。</param>
        /// <returns>返回 <see cref="IByteMemoryBuffer"/>。</returns>
        IByteMemoryBuffer HmacSha512(IByteMemoryBuffer buffer, UniqueAlgorithmIdentifier identifier = null);
    }
}
