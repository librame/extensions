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
    /// 散列算法服务接口。
    /// </summary>
    public interface IHashAlgorithmService : IEncryptionService
    {
        /// <summary>
        /// RSA 非对称算法。
        /// </summary>
        /// <value>
        /// 返回 <see cref="IRsaAlgorithmService"/>。
        /// </value>
        IRsaAlgorithmService Rsa { get; }

        
        /// <summary>
        /// 计算 MD5。
        /// </summary>
        /// <param name="buffer">给定的 <see cref="IBuffer{T}"/>。</param>
        /// <param name="isSigned">是否签名（默认不签名）。</param>
        /// <returns>返回 <see cref="IBuffer{T}"/>。</returns>
        IBuffer<byte> Md5(IBuffer<byte> buffer, bool isSigned = false);

        
        /// <summary>
        /// 计算 SHA1。
        /// </summary>
        /// <param name="buffer">给定的 <see cref="IBuffer{T}"/>。</param>
        /// <param name="isSigned">是否签名（默认不签名）。</param>
        /// <returns>返回 <see cref="IBuffer{T}"/>。</returns>
        IBuffer<byte> Sha1(IBuffer<byte> buffer, bool isSigned = false);

        
        /// <summary>
        /// 计算 SHA256。
        /// </summary>
        /// <param name="buffer">给定的 <see cref="IBuffer{T}"/>。</param>
        /// <param name="isSigned">是否签名（默认不签名）。</param>
        /// <returns>返回 <see cref="IBuffer{T}"/>。</returns>
        IBuffer<byte> Sha256(IBuffer<byte> buffer, bool isSigned = false);

        
        /// <summary>
        /// 计算 SHA384。
        /// </summary>
        /// <param name="buffer">给定的 <see cref="IBuffer{T}"/>。</param>
        /// <param name="isSigned">是否签名（默认不签名）。</param>
        /// <returns>返回 <see cref="IBuffer{T}"/>。</returns>
        IBuffer<byte> Sha384(IBuffer<byte> buffer, bool isSigned = false);

        
        /// <summary>
        /// 计算 SHA512。
        /// </summary>
        /// <param name="buffer">给定的 <see cref="IBuffer{T}"/>。</param>
        /// <param name="isSigned">是否签名（默认不签名）。</param>
        /// <returns>返回 <see cref="IBuffer{T}"/>。</returns>
        IBuffer<byte> Sha512(IBuffer<byte> buffer, bool isSigned = false);
    }
}
