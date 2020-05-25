#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Encryption.Services
{
    using Core.Identifiers;
    using Core.Services;
    using Encryption.Generators;

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

        /// <summary>
        /// 向量生成器。
        /// </summary>
        /// <value>
        /// 返回 <see cref="IVectorGenerator"/>。
        /// </value>
        IVectorGenerator VectorGenerator { get; }


        #region AES

        /// <summary>
        /// 加密 AES。
        /// </summary>
        /// <param name="buffer">给定待加密的字节数组。</param>
        /// <param name="key">输出密钥。</param>
        /// <param name="vector">输出向量。</param>
        /// <param name="identifier">给定的 <see cref="SecurityIdentifier"/>（可选；默认使用选项配置）。</param>
        /// <returns>返回字节数组。</returns>
        byte[] EncryptAes(byte[] buffer, out byte[] key, out byte[] vector,
            SecurityIdentifier identifier = null);

        /// <summary>
        /// 解密 AES。
        /// </summary>
        /// <param name="buffer">给定的字节数组。</param>
        /// <param name="key">输出密钥。</param>
        /// <param name="vector">输出向量。</param>
        /// <param name="identifier">给定的 <see cref="SecurityIdentifier"/>（可选；默认使用选项配置）。</param>
        /// <returns>返回字节数组。</returns>
        byte[] DecryptAes(byte[] buffer, out byte[] key, out byte[] vector,
            SecurityIdentifier identifier = null);

        #endregion


        #region DES

        /// <summary>
        /// 加密 DES。
        /// </summary>
        /// <param name="buffer">给定待加密的字节数组。</param>
        /// <param name="key">输出密钥。</param>
        /// <param name="vector">输出向量。</param>
        /// <param name="identifier">给定的 <see cref="SecurityIdentifier"/>（可选；默认使用选项配置）。</param>
        /// <returns>返回字节数组。</returns>
        byte[] EncryptDes(byte[] buffer, out byte[] key, out byte[] vector,
            SecurityIdentifier identifier = null);

        /// <summary>
        /// 解密 DES。
        /// </summary>
        /// <param name="buffer">给定的字节数组。</param>
        /// <param name="key">输出密钥。</param>
        /// <param name="vector">输出向量。</param>
        /// <param name="identifier">给定的 <see cref="SecurityIdentifier"/>（可选；默认使用选项配置）。</param>
        /// <returns>返回字节数组。</returns>
        byte[] DecryptDes(byte[] buffer, out byte[] key, out byte[] vector,
            SecurityIdentifier identifier = null);

        #endregion


        #region TripleDES

        /// <summary>
        /// 加密 TripleDES。
        /// </summary>
        /// <param name="buffer">给定待加密的字节数组。</param>
        /// <param name="key">输出密钥。</param>
        /// <param name="vector">输出向量。</param>
        /// <param name="identifier">给定的 <see cref="SecurityIdentifier"/>（可选；默认使用选项配置）。</param>
        /// <returns>返回字节数组。</returns>
        byte[] EncryptTripleDes(byte[] buffer, out byte[] key, out byte[] vector,
            SecurityIdentifier identifier = null);

        /// <summary>
        /// 解密 TripleDES。
        /// </summary>
        /// <param name="buffer">给定的字节数组。</param>
        /// <param name="key">输出密钥。</param>
        /// <param name="vector">输出向量。</param>
        /// <param name="identifier">给定的 <see cref="SecurityIdentifier"/>（可选；默认使用选项配置）。</param>
        /// <returns>返回字节数组。</returns>
        byte[] DecryptTripleDes(byte[] buffer, out byte[] key, out byte[] vector,
            SecurityIdentifier identifier = null);

        #endregion

    }
}
