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
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Paddings;

namespace Librame.Algorithm.Symmetries
{
    /// <summary>
    /// 抽象对称算法基类。
    /// </summary>
    public abstract class AbstractBouncyCastleSymmetryAlgorithm : AbstractSymmetryAlgorithm
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractBouncyCastleSymmetryAlgorithm"/> 实例。
        /// </summary>
        /// <param name="algoSettings">给定的 <see cref="AlgorithmSettings"/>。</param>
        public AbstractBouncyCastleSymmetryAlgorithm(AlgorithmSettings algoSettings)
            : base(algoSettings)
        {
        }

        
        /// <summary>
        /// 加密字符串。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <param name="key">给定的密钥。</param>
        /// <returns>返回加密字符串。</returns>
        public override string Encrypt(string str, byte[] key)
        {
            // 向量
            var iv = GenerateIV(key);

            var cipher = new CbcBlockCipher(CreateEngine());
            var engine = new PaddedBufferedBlockCipher(cipher);

            // 密钥加密
            engine.Init(true, new ParametersWithIV(new KeyParameter(key), iv));

            var buffer = GetBytes(str);
            return EncodeBase64(engine.DoFinal(buffer));
        }

        
        /// <summary>
        /// 解密字符串。
        /// </summary>
        /// <param name="encrypt">给定的加密字符串。</param>
        /// <param name="key">给定的密钥。</param>
        /// <returns>返回原始字符串。</returns>
        public override string Decrypt(string encrypt, byte[] key)
        {
            // 向量
            var iv = GenerateIV(key);

            var cipher = new CbcBlockCipher(CreateEngine());
            var engine = new PaddedBufferedBlockCipher(cipher);
            // 密钥加密
            engine.Init(false, new ParametersWithIV(new KeyParameter(key), iv));

            var buffer = DecodeBase64(encrypt);

            return GetString(engine.DoFinal(buffer));
        }


        /// <summary>
        /// 创建对称算法引擎。
        /// </summary>
        /// <returns>返回 <see cref="IBlockCipher"/>。</returns>
        protected abstract IBlockCipher CreateEngine();

    }
}
