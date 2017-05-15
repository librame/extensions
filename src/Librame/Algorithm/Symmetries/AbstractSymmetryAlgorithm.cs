#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;

namespace Librame.Algorithm.Symmetries
{
    using Adaptation;
    using Utility;

    /// <summary>
    /// 抽象对称算法。
    /// </summary>
    public abstract class AbstractSymmetryAlgorithm : AbstractByteCodec, ISymmetryAlgorithm
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractSymmetryAlgorithm"/> 实例。
        /// </summary>
        /// <param name="algoSettings">给定的 <see cref="AlgorithmSettings"/>。</param>
        public AbstractSymmetryAlgorithm(AlgorithmSettings algoSettings)
            : base(algoSettings)
        {
        }


        /// <summary>
        /// 加密字符串。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <param name="guid">给定的全局唯一标识符（可选；默认使用 <see cref="AdapterSettings.AuthId"/>）。</param>
        /// <returns>返回加密字符串。</returns>
        public virtual string Encrypt(string str, string guid = null)
        {
            var key = GenerateKey(guid);

            return Encrypt(str, key);
        }
        /// <summary>
        /// 加密字符串。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <param name="key">给定的密钥。</param>
        /// <returns>返回加密字符串。</returns>
        public abstract string Encrypt(string str, byte[] key);


        /// <summary>
        /// 解密字符串。
        /// </summary>
        /// <param name="encrypt">给定的加密字符串。</param>
        /// <param name="guid">给定的全局唯一标识符（可选；默认使用 <see cref="AdapterSettings.AuthId"/>）。</param>
        /// <returns>返回原始字符串。</returns>
        public virtual string Decrypt(string encrypt, string guid = null)
        {
            var key = GenerateKey(guid);

            return Decrypt(encrypt, key);
        }
        /// <summary>
        /// 解密字符串。
        /// </summary>
        /// <param name="encrypt">给定的加密字符串。</param>
        /// <param name="key">给定的密钥。</param>
        /// <returns>返回原始字符串。</returns>
        public abstract string Decrypt(string encrypt, byte[] key);


        /// <summary>
        /// 生成密钥。
        /// </summary>
        /// <param name="guid">给定的全局唯一标识符（可选；默认使用 <see cref="AdapterSettings.AuthId"/>）。</param>
        /// <returns>返回用于当前算法的密钥字节数组。</returns>
        public abstract byte[] GenerateKey(string guid = null);

        /// <summary>
        /// 生成向量。
        /// </summary>
        /// <param name="key">给定的密钥。</param>
        /// <returns>返回用于当前算法的向量字节数组。</returns>
        public abstract byte[] GenerateIV(byte[] key);


        /// <summary>
        /// 将全局唯一标识符转换为 128 位的字节数组。
        /// </summary>
        /// <param name="guid">给定的全局唯一标识符（可选；默认使用 <see cref="AdapterSettings.AuthId"/>）。</param>
        /// <returns>返回 128 位的字节数组。</returns>
        protected virtual byte[] GuidToKey128Bit(string guid = null)
        {
            try
            {
                var bit = AlgoSettings.AdapterSettings.AuthId;
                if (!string.IsNullOrEmpty(guid))
                    bit = GuidUtility.AsHex(guid);

                return DecodeBit(bit);
            }
            catch (Exception ex)
            {
                Log.Error(ex.InnerMessage(), ex);

                return null;
            }
        }

    }
}
