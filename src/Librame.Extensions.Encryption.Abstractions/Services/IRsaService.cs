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
    using Core;

    /// <summary>
    /// RSA 非对称服务接口。
    /// </summary>
    public interface IRsaService : IService
    {
        /// <summary>
        /// 签名证书。
        /// </summary>
        /// <value>
        /// 返回 <see cref="ISigningCredentialsService"/>。
        /// </value>
        ISigningCredentialsService SigningCredentials { get; }


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
        /// <param name="buffer">给定要签名的 <see cref="IByteMemoryBuffer"/>。</param>
        /// <returns>返回签名后的 <see cref="IByteMemoryBuffer"/>。</returns>
        IByteMemoryBuffer SignData(IByteMemoryBuffer buffer);

        /// <summary>
        /// 签名散列。
        /// </summary>
        /// <param name="buffer">给定要签名的 <see cref="IByteMemoryBuffer"/>。</param>
        /// <returns>返回签名后的 <see cref="IByteMemoryBuffer"/>。</returns>
        IByteMemoryBuffer SignHash(IByteMemoryBuffer buffer);


        /// <summary>
        /// 验证数据。
        /// </summary>
        /// <param name="buffer">给定要签名的 <see cref="IByteMemoryBuffer"/>。</param>
        /// <param name="signedBuffer">给定已签名的 <see cref="IReadOnlyMemoryBuffer{Byte}"/>。</param>
        /// <returns>返回签名后的 <see cref="IByteMemoryBuffer"/>。</returns>
        bool VerifyData(IByteMemoryBuffer buffer, IReadOnlyMemoryBuffer<byte> signedBuffer);

        /// <summary>
        /// 验证散列。
        /// </summary>
        /// <param name="buffer">给定要签名的 <see cref="IByteMemoryBuffer"/>。</param>
        /// <param name="signedBuffer">给定已签名的 <see cref="IReadOnlyMemoryBuffer{Byte}"/>。</param>
        /// <returns>返回签名后的 <see cref="IByteMemoryBuffer"/>。</returns>
        bool VerifyHash(IByteMemoryBuffer buffer, IReadOnlyMemoryBuffer<byte> signedBuffer);


        /// <summary>
        /// 加密 <see cref="IByteMemoryBuffer"/>。
        /// </summary>
        /// <param name="buffer">给定待加密的 <see cref="IByteMemoryBuffer"/>。</param>
        /// <returns>返回 <see cref="IByteMemoryBuffer"/>。</returns>
        IByteMemoryBuffer Encrypt(IByteMemoryBuffer buffer);


        /// <summary>
        /// 解密 <see cref="IByteMemoryBuffer"/>。
        /// </summary>
        /// <param name="buffer">给定的加密 <see cref="IByteMemoryBuffer"/>。</param>
        /// <returns>返回 <see cref="IByteMemoryBuffer"/>。</returns>
        IByteMemoryBuffer Decrypt(IByteMemoryBuffer buffer);
    }
}
