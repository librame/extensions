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

namespace Librame.Extensions.Encryption.Services
{
    using Core.Services;

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
        /// <param name="buffer">给定要签名的字节数组。</param>
        /// <returns>返回签名后的字节数组。</returns>
        byte[] SignData(byte[] buffer);

        /// <summary>
        /// 签名散列。
        /// </summary>
        /// <param name="buffer">给定要签名的字节数组。</param>
        /// <returns>返回签名后的字节数组。</returns>
        byte[] SignHash(byte[] buffer);


        /// <summary>
        /// 验证数据。
        /// </summary>
        /// <param name="buffer">给定要签名的字节数组。</param>
        /// <param name="signedBuffer">给定已签名的字节数组。</param>
        /// <returns>返回签名后的字节数组。</returns>
        bool VerifyData(byte[] buffer, byte[] signedBuffer);

        /// <summary>
        /// 验证散列。
        /// </summary>
        /// <param name="buffer">给定要签名的字节数组。</param>
        /// <param name="signedBuffer">给定已签名的字节数组。</param>
        /// <returns>返回签名后的字节数组。</returns>
        bool VerifyHash(byte[] buffer, byte[] signedBuffer);


        /// <summary>
        /// 加密字节数组。
        /// </summary>
        /// <param name="buffer">给定待加密的字节数组。</param>
        /// <returns>返回字节数组。</returns>
        byte[] Encrypt(byte[] buffer);


        /// <summary>
        /// 解密字节数组。
        /// </summary>
        /// <param name="buffer">给定的加密字节数组。</param>
        /// <returns>返回字节数组。</returns>
        byte[] Decrypt(byte[] buffer);
    }
}
