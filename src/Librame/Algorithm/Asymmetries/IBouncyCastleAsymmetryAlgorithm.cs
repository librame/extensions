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

namespace Librame.Algorithm.Asymmetries
{
    /// <summary>
    /// 非对称算法。
    /// </summary>
    public interface IBouncyCastleAsymmetryAlgorithm : IByteCodec
    {

        #region Public Key

        /// <summary>
        /// 转换为公钥字符串。
        /// </summary>
        /// <param name="publicKey">给定的公钥。</param>
        /// <returns>返回字符串。</returns>
        string ExportPublicKeyString(AsymmetricKeyParameter publicKey);

        /// <summary>
        /// 还原公钥字符串。
        /// </summary>
        /// <param name="publicKeyString">给定的公钥字符串。</param>
        /// <returns>返回 <see cref="AsymmetricKeyParameter"/>。</returns>
        AsymmetricKeyParameter ImportPublicKeyString(string publicKeyString);

        #endregion


        #region Private Key

        /// <summary>
        /// 加密为私钥字符串。
        /// </summary>
        /// <param name="privateKey">给定的私钥。</param>
        /// <param name="password">给定的密码。</param>
        /// <returns>返回字符串。</returns>
        string EncryptPrivateKeyString(AsymmetricKeyParameter privateKey, string password);

        /// <summary>
        /// 解密私钥。
        /// </summary>
        /// <param name="privateKeyString">给定的私钥字符串。</param>
        /// <param name="password">给定的密码。</param>
        /// <returns>返回字符串。</returns>
        AsymmetricKeyParameter DecryptPrivateKeyString(string privateKeyString, string password);


        /// <summary>
        /// 转换为私钥字符串。
        /// </summary>
        /// <param name="privateKey">给定的私钥。</param>
        /// <returns>返回字符串。</returns>
        string ExportPrivateKeyString(AsymmetricKeyParameter privateKey);

        /// <summary>
        /// 还原私钥字符串。
        /// </summary>
        /// <param name="privateKeyString">给定的私钥字符串。</param>
        /// <returns>返回 <see cref="AsymmetricKeyParameter"/>。</returns>
        AsymmetricKeyParameter ImportPrivateKeyString(string privateKeyString);

        #endregion


        /// <summary>
        /// 加密字符串。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <param name="privateKeyPassword">给定的私钥加密密码。</param>
        /// <param name="keyPair">输出密钥对。</param>
        /// <returns>返回加密字符串。</returns>
        string Encrypt(string str, string privateKeyPassword, out AsymmetryKeyPair keyPair);
        /// <summary>
        /// 加密字符串。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <param name="publicKey">给定的公钥。</param>
        /// <returns>返回加密字符串。</returns>
        string Encrypt(string str, AsymmetricKeyParameter publicKey);


        /// <summary>
        /// 解密字符串。
        /// </summary>
        /// <param name="encrypt">给定的加密字符串。</param>
        /// <param name="privateKeyString">给定的私钥字符串。</param>
        /// <param name="privateKeyPassword">给定的私钥解密密码。</param>
        /// <returns>返回解密字符串。</returns>
        string Decrypt(string encrypt, string privateKeyString, string privateKeyPassword);
        /// <summary>
        /// 解密字符串。
        /// </summary>
        /// <param name="encrypt">给定的加密字符串。</param>
        /// <param name="privateKey">给定的私钥。</param>
        /// <returns>返回解密字符串。</returns>
        string Decrypt(string encrypt, AsymmetricKeyParameter privateKey);


        /// <summary>
        /// 生成密钥对。
        /// </summary>
        /// <param name="keyGenerator">给定的非对称算法密钥对生成器。</param>
        /// <returns>返回 <see cref="AsymmetricCipherKeyPair"/>。</returns>
        AsymmetricCipherKeyPair GenerateKeyPair(IAsymmetricCipherKeyPairGenerator keyGenerator = null);

    }
}
