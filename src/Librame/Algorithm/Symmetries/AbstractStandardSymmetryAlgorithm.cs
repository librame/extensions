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

namespace Librame.Algorithm.Symmetries
{
    /// <summary>
    /// 抽象标准对称算法。
    /// </summary>
    public abstract class AbstractStandardSymmetryAlgorithm : AbstractSymmetryAlgorithm
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractStandardSymmetryAlgorithm"/> 实例。
        /// </summary>
        /// <param name="algoSettings">给定的 <see cref="AlgorithmSettings"/>。</param>
        public AbstractStandardSymmetryAlgorithm(AlgorithmSettings algoSettings)
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
            var aes = CreateEngine();
            aes.Key = key;
            aes.IV = GenerateIV(key);
            aes.Mode = CipherMode.ECB;
            aes.Padding = PaddingMode.PKCS7;
            
            var ct = aes.CreateEncryptor();

            var buffer = GetBytes(str);
            return EncodeBit(ct.TransformFinalBlock(buffer, 0, buffer.Length));
        }


        /// <summary>
        /// 解密字符串。
        /// </summary>
        /// <param name="encrypt">给定的加密字符串。</param>
        /// <param name="key">给定的密钥。</param>
        /// <returns>返回原始字符串。</returns>
        public override string Decrypt(string encrypt, byte[] key)
        {
            var aes = CreateEngine();
            aes.Key = key;
            aes.IV = GenerateIV(key);
            aes.Mode = CipherMode.ECB;
            aes.Padding = PaddingMode.PKCS7;
            
            var ct = aes.CreateDecryptor();

            var buffer = DecodeBit(encrypt);
            return GetString(ct.TransformFinalBlock(buffer, 0, buffer.Length));
        }


        /// <summary>
        /// 创建对称算法引擎。
        /// </summary>
        /// <returns>返回 <see cref="SymmetricAlgorithm"/>。</returns>
        protected abstract SymmetricAlgorithm CreateEngine();

    }
}
