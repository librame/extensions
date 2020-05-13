#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Encryption.Services
{
    using Core.Services;

    /// <summary>
    /// 散列服务接口。
    /// </summary>
    public interface IHashService : IService
    {
        /// <summary>
        /// RSA 非对称算法。
        /// </summary>
        /// <value>
        /// 返回 <see cref="IRsaService"/>。
        /// </value>
        IRsaService Rsa { get; }


        /// <summary>
        /// 计算 MD5。
        /// </summary>
        /// <param name="buffer">给定的字节数组。</param>
        /// <param name="isSigned">是否签名（默认不签名）。</param>
        /// <returns>返回字节数组。</returns>
        byte[] Md5(byte[] buffer, bool isSigned = false);

        /// <summary>
        /// 计算 SHA1。
        /// </summary>
        /// <param name="buffer">给定的字节数组。</param>
        /// <param name="isSigned">是否签名（默认不签名）。</param>
        /// <returns>返回字节数组。</returns>
        byte[] Sha1(byte[] buffer, bool isSigned = false);

        /// <summary>
        /// 计算 SHA256。
        /// </summary>
        /// <param name="buffer">给定的字节数组。</param>
        /// <param name="isSigned">是否签名（默认不签名）。</param>
        /// <returns>返回字节数组。</returns>
        byte[] Sha256(byte[] buffer, bool isSigned = false);

        /// <summary>
        /// 计算 SHA384。
        /// </summary>
        /// <param name="buffer">给定的字节数组。</param>
        /// <param name="isSigned">是否签名（默认不签名）。</param>
        /// <returns>返回字节数组。</returns>
        byte[] Sha384(byte[] buffer, bool isSigned = false);

        /// <summary>
        /// 计算 SHA512。
        /// </summary>
        /// <param name="buffer">给定的字节数组。</param>
        /// <param name="isSigned">是否签名（默认不签名）。</param>
        /// <returns>返回字节数组。</returns>
        byte[] Sha512(byte[] buffer, bool isSigned = false);
    }
}
