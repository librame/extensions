#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using System.Linq;

namespace Librame.Algorithm.Symmetries
{
    using Adaptation;

    /// <summary>
    /// BouncyCastle AES 对称算法。
    /// </summary>
    public class BouncyCastleAes : AbstractBouncyCastleSymmetryAlgorithm
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractByteCodec"/> 实例。
        /// </summary>
        /// <param name="algoSettings">给定的 <see cref="AlgorithmSettings"/>。</param>
        public BouncyCastleAes(AlgorithmSettings algoSettings)
            : base(algoSettings)
        {
        }


        /// <summary>
        /// 生成密钥。
        /// </summary>
        /// <param name="guid">给定的全局唯一标识符（可选；默认使用 <see cref="AdapterSettings.AuthId"/>）。</param>
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
        /// <returns>返回 <see cref="IBlockCipher"/>。</returns>
        protected override IBlockCipher CreateEngine()
        {
            return new AesFastEngine();
        }

    }
}
