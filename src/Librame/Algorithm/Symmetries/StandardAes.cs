#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Linq;
using System.Security.Cryptography;

namespace Librame.Algorithm.Symmetries
{
    /// <summary>
    /// 标准 AES 对称算法。
    /// </summary>
    public class StandardAes : AbstractStandardSymmetryAlgorithm
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractByteCodec"/> 实例。
        /// </summary>
        /// <param name="algoSettings">给定的 <see cref="AlgorithmSettings"/>。</param>
        public StandardAes(AlgorithmSettings algoSettings)
            : base(algoSettings)
        {
        }


        /// <summary>
        /// 生成密钥。
        /// </summary>
        /// <param name="guid">给定的全局唯一标识符（可选；默认使用 <see cref="Adaptation.AdapterSettings.AuthId"/>）。</param>
        /// <returns>返回用于当前算法的 256 位密钥字节数组。</returns>
        public override byte[] GenerateKey(string guid = null)
        {
            var key128 = GuidToKey128Bit(guid);

            // 256bit
            return key128.Concat(key128.Reverse()).ToArray();
        }

        /// <summary>
        /// 生成向量。
        /// </summary>
        /// <param name="key">给定的密钥。</param>
        /// <returns>返回用于当前算法的 128 位向量字节数组。</returns>
        public override byte[] GenerateIV(byte[] key)
        {
            // 128bit
            return key.Skip(8).Take(16).Reverse().ToArray();
        }


        /// <summary>
        /// 创建 AES 引擎。
        /// </summary>
        /// <returns>返回 <see cref="SymmetricAlgorithm"/>。</returns>
        protected override SymmetricAlgorithm CreateEngine()
        {
            return new RijndaelManaged();
        }

    }
}
