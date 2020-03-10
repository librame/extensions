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
    using KeyGenerators;

    /// <summary>
    /// 对称服务接口。
    /// </summary>
    public interface ISymmetricService : IService
    {
        /// <summary>
        /// 密钥生成器。
        /// </summary>
        /// <value>
        /// 返回 <see cref="IKeyGenerator"/>。
        /// </value>
        IKeyGenerator KeyGenerator { get; }


        #region AES

        /// <summary>
        /// 加密 AES。
        /// </summary>
        /// <param name="buffer">给定待加密的字节数组。</param>
        /// <param name="descriptor">给定的 <see cref="KeyDescriptor"/>（可选；默认使用选项配置）。</param>
        /// <returns>返回字节数组。</returns>
        byte[] EncryptAes(byte[] buffer, KeyDescriptor descriptor = null);

        /// <summary>
        /// 解密 AES。
        /// </summary>
        /// <param name="buffer">给定的字节数组。</param>
        /// <param name="descriptor">给定的 <see cref="KeyDescriptor"/>（可选；默认使用选项配置）。</param>
        /// <returns>返回字节数组。</returns>
        byte[] DecryptAes(byte[] buffer, KeyDescriptor descriptor = null);

        #endregion


        #region DES

        /// <summary>
        /// 加密 DES。
        /// </summary>
        /// <param name="buffer">给定待加密的字节数组。</param>
        /// <param name="descriptor">给定的 <see cref="KeyDescriptor"/>（可选；默认使用选项配置）。</param>
        /// <returns>返回字节数组。</returns>
        byte[] EncryptDes(byte[] buffer, KeyDescriptor descriptor = null);

        /// <summary>
        /// 解密 DES。
        /// </summary>
        /// <param name="buffer">给定的字节数组。</param>
        /// <param name="descriptor">给定的 <see cref="KeyDescriptor"/>（可选；默认使用选项配置）。</param>
        /// <returns>返回字节数组。</returns>
        byte[] DecryptDes(byte[] buffer, KeyDescriptor descriptor = null);

        #endregion


        #region TripleDES

        /// <summary>
        /// 加密 TripleDES。
        /// </summary>
        /// <param name="buffer">给定待加密的字节数组。</param>
        /// <param name="descriptor">给定的 <see cref="KeyDescriptor"/>（可选；默认使用选项配置）。</param>
        /// <returns>返回字节数组。</returns>
        byte[] EncryptTripleDes(byte[] buffer, KeyDescriptor descriptor = null);

        /// <summary>
        /// 解密 TripleDES。
        /// </summary>
        /// <param name="buffer">给定的字节数组。</param>
        /// <param name="descriptor">给定的 <see cref="KeyDescriptor"/>（可选；默认使用选项配置）。</param>
        /// <returns>返回字节数组。</returns>
        byte[] DecryptTripleDes(byte[] buffer, KeyDescriptor descriptor = null);

        #endregion

    }
}
