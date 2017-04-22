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
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;

namespace Librame.Algorithm.Asymmetries
{
    using Utility;

    /// <summary>
    /// RSA 非对称算法。
    /// </summary>
    public class BouncyCastleRsa : AbstractBouncyCastleAsymmetryAlgorithm
    {
        private static readonly int _keyLength = 2048;
        private static readonly int _certainty = 25;
        
        /// <summary>
        /// 构造一个 <see cref="BouncyCastleRsa"/> 实例。
        /// </summary>
        /// <param name="algoSettings">给定的 <see cref="AlgorithmSettings"/>。</param>
        public BouncyCastleRsa(AlgorithmSettings algoSettings)
            : base(algoSettings)
        {
        }


        /// <summary>
        /// 创建 RSA 引擎。
        /// </summary>
        /// <returns>返回 <see cref="IAsymmetricBlockCipher"/>。</returns>
        protected override IAsymmetricBlockCipher CreateEngine()
        {
            return new RsaEngine();
        }


        /// <summary>
        /// 生成密钥对。
        /// </summary>
        /// <param name="keyGenerator">给定的非对称算法密钥对生成器。</param>
        /// <returns>返回 <see cref="AsymmetricCipherKeyPair"/>。</returns>
        public override AsymmetricCipherKeyPair GenerateKeyPair(IAsymmetricCipherKeyPairGenerator keyGenerator = null)
        {
            if (ReferenceEquals(keyGenerator, null))
                keyGenerator = CreateKeyPairGenerator();
            else
                keyGenerator.GuardNotType<RsaKeyPairGenerator>(nameof(keyGenerator));
            
            // 生成键名对
            var keyPair = keyGenerator.GenerateKeyPair();

            if (((RsaKeyParameters)keyPair.Public).Modulus.BitLength < _keyLength)
            {
                // Console.WriteLine("failed key generation length test");
                keyPair = GenerateKeyPair(keyGenerator);
            }

            return keyPair;
        }

        /// <summary>
        /// 创建 RSA 密钥对生成器。
        /// </summary>
        /// <returns>返回 <see cref="RsaKeyPairGenerator"/>。</returns>
        protected virtual RsaKeyPairGenerator CreateKeyPairGenerator()
        {
            // RSA密钥对的构造器
            var keyGenerator = new RsaKeyPairGenerator();

            // RSA密钥构造器的参数
            var param = new RsaKeyGenerationParameters(
                BigInteger.ValueOf(3),
                new Org.BouncyCastle.Security.SecureRandom(),
                _keyLength,  
                _certainty);

            // 用参数初始化密钥构造器
            keyGenerator.Init(param);

            return keyGenerator;
        }

    }
}
