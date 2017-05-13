#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Crypto;

namespace Librame.Algorithm.Asymmetries
{
    /// <summary>
    /// 抽象非对称算法基类。
    /// </summary>
    public abstract class AbstractBouncyCastleAsymmetryAlgorithm : AbstractByteCodec, IBouncyCastleAsymmetryAlgorithm
    {
        // 3 key triple DES with SHA-1
        private static readonly string _algorithm = "1.2.840.113549.1.12.1.3";
        private static readonly byte[] _salt = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        //private static readonly int _iterationCount = 1000;


        /// <summary>
        /// 构造一个 <see cref="AbstractBouncyCastleAsymmetryAlgorithm"/> 实例。
        /// </summary>
        /// <param name="algoSettings">给定的 <see cref="AlgorithmSettings"/>。</param>
        public AbstractBouncyCastleAsymmetryAlgorithm(AlgorithmSettings algoSettings)
            : base(algoSettings)
        {
        }


        #region Public Key

        /// <summary>
        /// 导出为公钥字符串。
        /// </summary>
        /// <param name="publicKey">给定的公钥。</param>
        /// <returns>返回字符串。</returns>
        public virtual string ExportPublicKeyString(AsymmetricKeyParameter publicKey)
        {
            var keyInfo = Org.BouncyCastle.X509.SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(publicKey);
            var buffer = keyInfo.ToAsn1Object().GetEncoded();

            return EncodeBase64(buffer);
        }

        /// <summary>
        /// 导入公钥字符串。
        /// </summary>
        /// <param name="publicKeyString">给定的公钥字符串。</param>
        /// <returns>返回 <see cref="AsymmetricKeyParameter"/>。</returns>
        public virtual AsymmetricKeyParameter ImportPublicKeyString(string publicKeyString)
        {
            var buffer = DecodeBase64(publicKeyString);
            var asn1 = Asn1Object.FromByteArray(buffer);
            buffer = asn1.GetDerEncoded();
            var keyInfo = Org.BouncyCastle.Asn1.X509.SubjectPublicKeyInfo.GetInstance(asn1);

            return Org.BouncyCastle.Security.PublicKeyFactory.CreateKey(keyInfo);
        }

        #endregion


        #region Private Key

        /// <summary>
        /// 加密为私钥字符串。
        /// </summary>
        /// <param name="privateKey">给定的私钥。</param>
        /// <param name="password">给定的加密密码。</param>
        /// <returns>返回字符串。</returns>
        public virtual string EncryptPrivateKeyString(AsymmetricKeyParameter privateKey, string password)
        {
            var passPhrase = password.ToCharArray();
            var salt = GetBytes(AlgoSettings.KeySalt);

            var encryptedKeyInfo = Org.BouncyCastle.Pkcs.EncryptedPrivateKeyInfoFactory.CreateEncryptedPrivateKeyInfo(_algorithm,
                passPhrase, salt, AlgoSettings.IterationCount, privateKey);
            var buffer = encryptedKeyInfo.ToAsn1Object().GetEncoded();

            return EncodeBase64(buffer);
        }

        /// <summary>
        /// 解密私钥。
        /// </summary>
        /// <param name="privateKeyString">给定的私钥字符串。</param>
        /// <param name="password">给定的解密密码。</param>
        /// <returns>返回字符串。</returns>
        public virtual AsymmetricKeyParameter DecryptPrivateKeyString(string privateKeyString, string password)
        {
            var passPhrase = password.ToCharArray();

            var buffer = DecodeBase64(privateKeyString);
            var asn1 = Asn1Object.FromByteArray(buffer);
            buffer = asn1.GetDerEncoded();

            var encryptedKeyInfo = Org.BouncyCastle.Asn1.Pkcs.EncryptedPrivateKeyInfo.GetInstance(asn1);
            var keyInfo = Org.BouncyCastle.Pkcs.PrivateKeyInfoFactory.CreatePrivateKeyInfo(passPhrase, encryptedKeyInfo);

            return Org.BouncyCastle.Security.PrivateKeyFactory.CreateKey(keyInfo);
        }


        /// <summary>
        /// 导出为私钥字符串。
        /// </summary>
        /// <param name="privateKey">给定的私钥。</param>
        /// <returns>返回字符串。</returns>
        public virtual string ExportPrivateKeyString(AsymmetricKeyParameter privateKey)
        {
            var keyInfo = Org.BouncyCastle.Pkcs.PrivateKeyInfoFactory.CreatePrivateKeyInfo(privateKey);
            var buffer = keyInfo.ToAsn1Object().GetEncoded();

            return EncodeBase64(buffer);
        }

        /// <summary>
        /// 导入私钥字符串。
        /// </summary>
        /// <param name="privateKeyString">给定的私钥字符串。</param>
        /// <returns>返回 <see cref="AsymmetricKeyParameter"/>。</returns>
        public virtual AsymmetricKeyParameter ImportPrivateKeyString(string privateKeyString)
        {
            var buffer = DecodeBase64(privateKeyString);
            var asn1 = Asn1Object.FromByteArray(buffer);
            buffer = asn1.GetDerEncoded();
            var keyInfo = Org.BouncyCastle.Asn1.Pkcs.PrivateKeyInfo.GetInstance(asn1);

            return Org.BouncyCastle.Security.PrivateKeyFactory.CreateKey(keyInfo);
        }

        #endregion

        
        /// <summary>
        /// 加密字符串。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <param name="privateKeyPassword">给定的私钥加密密码。</param>
        /// <param name="keyPair">输出密钥对。</param>
        /// <returns>返回加密字符串。</returns>
        public virtual string Encrypt(string str, string privateKeyPassword, out AsymmetryKeyPair keyPair)
        {
            var _keyPair = GenerateKeyPair();

            var publicKey = ExportPublicKeyString(_keyPair.Public);
            var privateKey = EncryptPrivateKeyString(_keyPair.Private, privateKeyPassword);
            keyPair = new AsymmetryKeyPair(publicKey, privateKey);

            return Encrypt(str, _keyPair.Public);
        }
        /// <summary>
        /// 加密字符串。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <param name="publicKey">给定的公钥。</param>
        /// <returns>返回加密字符串。</returns>
        public virtual string Encrypt(string str, AsymmetricKeyParameter publicKey)
        {
            var engine = CreateEngine();
            // 公钥加密
            engine.Init(true, publicKey);

            var buffer = GetBytes(str);
            buffer = engine.ProcessBlock(buffer, 0, buffer.Length);

            return EncodeBase64(buffer);
        }


        /// <summary>
        /// 解密字符串。
        /// </summary>
        /// <param name="encrypt">给定的加密字符串。</param>
        /// <param name="privateKeyString">给定的私钥字符串。</param>
        /// <param name="privateKeyPassword">给定的私钥解密密码。</param>
        /// <returns>返回解密字符串。</returns>
        public virtual string Decrypt(string encrypt, string privateKeyString, string privateKeyPassword)
        {
            var privateKey = DecryptPrivateKeyString(privateKeyString, privateKeyPassword);

            return Decrypt(encrypt, privateKey);
        }
        /// <summary>
        /// 解密字符串。
        /// </summary>
        /// <param name="encrypt">给定的加密字符串。</param>
        /// <param name="privateKey">给定的私钥。</param>
        /// <returns>返回解密字符串。</returns>
        public virtual string Decrypt(string encrypt, AsymmetricKeyParameter privateKey)
        {
            var engine = CreateEngine();
            // 私钥解密
            engine.Init(false, privateKey);

            var buffer = DecodeBase64(encrypt);
            buffer = engine.ProcessBlock(buffer, 0, buffer.Length);

            return GetString(buffer);
        }


        /// <summary>
        /// 创建非对称算法引擎。
        /// </summary>
        /// <returns>返回 <see cref="IAsymmetricBlockCipher"/>。</returns>
        protected abstract IAsymmetricBlockCipher CreateEngine();


        /// <summary>
        /// 生成密钥对。
        /// </summary>
        /// <param name="keyGenerator">给定的非对称算法密钥对生成器。</param>
        /// <returns>返回 <see cref="AsymmetricCipherKeyPair"/>。</returns>
        public abstract AsymmetricCipherKeyPair GenerateKeyPair(IAsymmetricCipherKeyPairGenerator keyGenerator = null);

    }
}
