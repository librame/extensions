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
        /// 签名证书提供程序。
        /// </summary>
        /// <value>
        /// 返回 <see cref="ISigningCredentialsProvider"/>。
        /// </value>
        ISigningCredentialsProvider Provider { get; }

        /// <summary>
        /// 加密构建器选项。
        /// </summary>
        /// <value>
        /// 返回 <see cref="EncryptionBuilderOptions"/>。
        /// </value>
        EncryptionBuilderOptions Options { get; }


        /// <summary>
        /// 签名散列算法。
        /// </summary>
        HashAlgorithmName SignHashAlgorithm { get; set; }

        /// <summary>
        /// 签名填充。
        /// </summary>
        RSASignaturePadding SignaturePadding { get; set; }

        /// <summary>
        /// 加密填充。
        /// </summary>
        RSAEncryptionPadding EncryptionPadding { get; set; }


        /// <summary>
        /// 签名数据。
        /// </summary>
        /// <param name="buffer">给定要签名的 <see cref="IBuffer{Byte}"/>。</param>
        /// <returns>返回签名后的 <see cref="IBuffer{Byte}"/>。</returns>
        IBuffer<byte> SignData(IBuffer<byte> buffer);

        /// <summary>
        /// 签名散列。
        /// </summary>
        /// <param name="buffer">给定要签名的 <see cref="IBuffer{Byte}"/>。</param>
        /// <returns>返回签名后的 <see cref="IBuffer{Byte}"/>。</returns>
        IBuffer<byte> SignHash(IBuffer<byte> buffer);


        /// <summary>
        /// 验证数据。
        /// </summary>
        /// <param name="buffer">给定要签名的 <see cref="IBuffer{Byte}"/>。</param>
        /// <param name="signedBuffer">给定已签名的 <see cref="IReadOnlyBuffer{Byte}"/>。</param>
        /// <returns>返回签名后的 <see cref="IBuffer{Byte}"/>。</returns>
        bool VerifyData(IBuffer<byte> buffer, IReadOnlyBuffer<byte> signedBuffer);

        /// <summary>
        /// 验证散列。
        /// </summary>
        /// <param name="buffer">给定要签名的 <see cref="IBuffer{Byte}"/>。</param>
        /// <param name="signedBuffer">给定已签名的 <see cref="IReadOnlyBuffer{Byte}"/>。</param>
        /// <returns>返回签名后的 <see cref="IBuffer{Byte}"/>。</returns>
        bool VerifyHash(IBuffer<byte> buffer, IReadOnlyBuffer<byte> signedBuffer);


        /// <summary>
        /// 加密 <see cref="IBuffer{Byte}"/>。
        /// </summary>
        /// <param name="buffer">给定待加密的 <see cref="IBuffer{Byte}"/>。</param>
        /// <returns>返回 <see cref="IBuffer{Byte}"/>。</returns>
        IBuffer<byte> Encrypt(IBuffer<byte> buffer);


        /// <summary>
        /// 解密 <see cref="IBuffer{Byte}"/>。
        /// </summary>
        /// <param name="buffer">给定的加密 <see cref="IBuffer{Byte}"/>。</param>
        /// <returns>返回 <see cref="IBuffer{Byte}"/>。</returns>
        IBuffer<byte> Decrypt(IBuffer<byte> buffer);
    }
}
