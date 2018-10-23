#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Security.Cryptography;

namespace Librame.Extensions.Encryption
{
    using Buffers;

    /// <summary>
    /// RSA 非对称算法服务接口。
    /// </summary>
    public interface IRsaAlgorithmService : IEncryptionService
    {
        /// <summary>
        /// 密钥生成器。
        /// </summary>
        /// <value>
        /// 返回 <see cref="IRsaKeyGenerator"/>。
        /// </value>
        IRsaKeyGenerator KeyGenerator { get; }


        /// <summary>
        /// 签名散列算法名称（默认为 <see cref="HashAlgorithmName.SHA256"/>）。
        /// </summary>
        HashAlgorithmName SignHashAlgorithm { get; set; }

        /// <summary>
        /// 签名填充（默认为 <see cref="RSASignaturePadding.Pkcs1"/>）。
        /// </summary>
        RSASignaturePadding SignaturePadding { get; set; }

        /// <summary>
        /// 加密填充（默认为 <see cref="RSAEncryptionPadding.Pkcs1"/>）。
        /// </summary>
        RSAEncryptionPadding EncryptionPadding { get; set; }


        /// <summary>
        /// 签名数据。
        /// </summary>
        /// <param name="buffer">给定要签名的 <see cref="IBuffer{T}"/>。</param>
        /// <param name="hashAlgorithm">给定的 <see cref="HashAlgorithmName"/>（可选；默认为 <see cref="SignHashAlgorithm"/>）。</param>
        /// <param name="padding">给定的 <see cref="RSASignaturePadding"/>（可选；默认为 <see cref="SignaturePadding"/>）。</param>
        /// <param name="parameters">给定的 <see cref="RSAParameters"/>（可选；默认使用 <see cref="KeyGenerator"/>）。</param>
        /// <returns>返回签名后的 <see cref="IBuffer{T}"/>。</returns>
        IBuffer<byte> SignData(IBuffer<byte> buffer,
            HashAlgorithmName hashAlgorithm = default, RSASignaturePadding padding = null, RSAParameters? parameters = null);

        /// <summary>
        /// 签名散列。
        /// </summary>
        /// <param name="buffer">给定要签名的 <see cref="IBuffer{T}"/>。</param>
        /// <param name="hashAlgorithm">给定的 <see cref="HashAlgorithmName"/>（可选；默认为 <see cref="SignHashAlgorithm"/>）。</param>
        /// <param name="padding">给定的 <see cref="RSASignaturePadding"/>（可选；默认为 <see cref="SignaturePadding"/>）。</param>
        /// <param name="parameters">给定的 <see cref="RSAParameters"/>（可选；默认使用 <see cref="KeyGenerator"/>）。</param>
        /// <returns>返回签名后的 <see cref="IBuffer{T}"/>。</returns>
        IBuffer<byte> SignHash(IBuffer<byte> buffer,
            HashAlgorithmName hashAlgorithm = default, RSASignaturePadding padding = null, RSAParameters? parameters = null);


        /// <summary>
        /// 验证数据。
        /// </summary>
        /// <param name="buffer">给定要签名的 <see cref="IBuffer{T}"/>。</param>
        /// <param name="signedBuffer">给定已签名的 <see cref="IReadOnlyBuffer{T}"/>。</param>
        /// <param name="hashAlgorithm">给定的 <see cref="HashAlgorithmName"/>（可选；默认为 <see cref="SignHashAlgorithm"/>）。</param>
        /// <param name="padding">给定的 <see cref="RSASignaturePadding"/>（可选；默认为 <see cref="SignaturePadding"/>）。</param>
        /// <param name="parameters">给定的 <see cref="RSAParameters"/>（可选；默认使用 <see cref="KeyGenerator"/>）。</param>
        /// <returns>返回签名后的 <see cref="IBuffer{T}"/>。</returns>
        bool VerifyData(IBuffer<byte> buffer, IReadOnlyBuffer<byte> signedBuffer,
            HashAlgorithmName hashAlgorithm = default, RSASignaturePadding padding = null, RSAParameters? parameters = null);

        /// <summary>
        /// 验证散列。
        /// </summary>
        /// <param name="buffer">给定要签名的 <see cref="IBuffer{T}"/>。</param>
        /// <param name="signedBuffer">给定已签名的 <see cref="IReadOnlyBuffer{T}"/>。</param>
        /// <param name="hashAlgorithm">给定的 <see cref="HashAlgorithmName"/>（可选；默认为 <see cref="SignHashAlgorithm"/>）。</param>
        /// <param name="padding">给定的 <see cref="RSASignaturePadding"/>（可选；默认为 <see cref="SignaturePadding"/>）。</param>
        /// <param name="parameters">给定的 <see cref="RSAParameters"/>（可选；默认使用 <see cref="KeyGenerator"/>）。</param>
        /// <returns>返回签名后的 <see cref="IBuffer{T}"/>。</returns>
        bool VerifyHash(IBuffer<byte> buffer, IReadOnlyBuffer<byte> signedBuffer,
            HashAlgorithmName hashAlgorithm = default, RSASignaturePadding padding = null, RSAParameters? parameters = null);


        /// <summary>
        /// 加密 <see cref="IBuffer{T}"/>。
        /// </summary>
        /// <param name="buffer">给定待加密的 <see cref="IBuffer{T}"/>。</param>
        /// <param name="padding">给定的 <see cref="RSAEncryptionPadding"/>（可选；默认为 <see cref="EncryptionPadding"/>）。</param>
        /// <param name="parameters">给定的 <see cref="RSAParameters"/>（可选；默认使用 <see cref="KeyGenerator"/>）。</param>
        /// <returns>返回 <see cref="IBuffer{T}"/>。</returns>
        IBuffer<byte> Encrypt(IBuffer<byte> buffer,
            RSAEncryptionPadding padding = null, RSAParameters? parameters = null);


        /// <summary>
        /// 解密 <see cref="IBuffer{T}"/>。
        /// </summary>
        /// <param name="buffer">给定的加密 <see cref="IBuffer{T}"/>。</param>
        /// <param name="padding">给定的 <see cref="RSAEncryptionPadding"/>（可选；默认为 <see cref="EncryptionPadding"/>）。</param>
        /// <param name="parameters">给定的 <see cref="RSAParameters"/>（可选；默认使用 <see cref="KeyGenerator"/>）。</param>
        /// <returns>返回 <see cref="IBuffer{T}"/>。</returns>
        IBuffer<byte> Decrypt(IBuffer<byte> buffer,
            RSAEncryptionPadding padding = null, RSAParameters? parameters = null);
    }
}
