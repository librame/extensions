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
    /// 对称算法服务接口。
    /// </summary>
    public interface ISymmetricAlgorithmService : IEncryptionService
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
        /// 转换为 AES。
        /// </summary>
        /// <param name="buffer">给定待加密的字节数组。</param>
        /// <param name="identifier">给定的标识符（可选；默认使用选项配置。详情可参考 <see cref="AlgorithmIdentifier"/>）。</param>
        /// <returns>返回字节数组。</returns>
        IByteBuffer ToAes(IByteBuffer buffer, string identifier = null);

        /// <summary>
        /// 还原 AES。
        /// </summary>
        /// <param name="buffer">给定的字节数组。</param>
        /// <param name="identifier">给定的标识符（可选；默认使用选项配置。详情可参考 <see cref="AlgorithmIdentifier"/>）。</param>
        /// <returns>返回字节数组。</returns>
        IByteBuffer FromAes(IByteBuffer buffer, string identifier = null);

        #endregion


        #region DES

        /// <summary>
        /// 转换为 DES。
        /// </summary>
        /// <param name="buffer">给定待加密的字节数组。</param>
        /// <param name="identifier">给定的标识符（可选；默认使用选项配置。详情可参考 <see cref="AlgorithmIdentifier"/>）。</param>
        /// <returns>返回字节数组。</returns>
        IByteBuffer ToDes(IByteBuffer buffer, string identifier = null);

        /// <summary>
        /// 还原 DES。
        /// </summary>
        /// <param name="buffer">给定的字节数组。</param>
        /// <param name="identifier">给定的标识符（可选；默认使用选项配置。详情可参考 <see cref="AlgorithmIdentifier"/>）。</param>
        /// <returns>返回字节数组。</returns>
        IByteBuffer FromDes(IByteBuffer buffer, string identifier = null);

        #endregion


        #region TripleDES

        /// <summary>
        /// 转换为 TripleDES。
        /// </summary>
        /// <param name="buffer">给定待加密的字节数组。</param>
        /// <param name="identifier">给定的标识符（可选；默认使用选项配置。详情可参考 <see cref="AlgorithmIdentifier"/>）。</param>
        /// <returns>返回字节数组。</returns>
        IByteBuffer ToTripleDes(IByteBuffer buffer, string identifier = null);

        /// <summary>
        /// 还原 TripleDES。
        /// </summary>
        /// <param name="buffer">给定的字节数组。</param>
        /// <param name="identifier">给定的标识符（可选；默认使用选项配置。详情可参考 <see cref="AlgorithmIdentifier"/>）。</param>
        /// <returns>返回字节数组。</returns>
        IByteBuffer FromTripleDes(IByteBuffer buffer, string identifier = null);

        #endregion

    }
}
