#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Algorithm
{
    using Asymmetries;
    using Symmetries;
    using Utility;

    /// <summary>
    /// 默认算法适配器。
    /// </summary>
    public class DefaultAlgorithmAdapter : AbstractAlgorithmAdapter, IAlgorithmAdapter
    {
        private static readonly string _standardAesKey = BuildKey<StandardAes>();
        private static readonly string _standardDesKey = BuildKey<StandardDes>();
        private static readonly string _bouncyCastleAesKey = BuildKey<BouncyCastleAes>();
        private static readonly string _bouncyCastleDesKey = BuildKey<BouncyCastleDes>();
        private static readonly string _bouncyCastleRsaKey = BuildKey<BouncyCastleRsa>();


        /// <summary>
        /// 获取散列算法。
        /// </summary>
        public virtual IHashAlgorithm Hash
        {
            get { return SingletonManager.Resolve<IHashAlgorithm>(key => new HashAlgorithm(AlgoSettings)); }
        }


        /// <summary>
        /// 标准 AES 对称算法。
        /// </summary>
        public virtual ISymmetryAlgorithm StandardAes
        {
            get { return SingletonManager.Resolve<ISymmetryAlgorithm>(_standardAesKey, key => new StandardAes(AlgoSettings)); }
        }

        /// <summary>
        /// 标准 DES 对称算法。
        /// </summary>
        public virtual ISymmetryAlgorithm StandardDes
        {
            get { return SingletonManager.Resolve<ISymmetryAlgorithm>(_standardDesKey, key => new StandardDes(AlgoSettings)); }
        }


        /// <summary>
        /// 获取 BouncyCastle AES 对称算法。
        /// </summary>
        public virtual ISymmetryAlgorithm BouncyCastleAes
        {
            get { return SingletonManager.Resolve<ISymmetryAlgorithm>(_bouncyCastleAesKey, key => new BouncyCastleAes(AlgoSettings)); }
        }

        /// <summary>
        /// 获取 BouncyCastle DES 对称算法。
        /// </summary>
        public virtual ISymmetryAlgorithm BouncyCastleDes
        {
            get { return SingletonManager.Resolve<ISymmetryAlgorithm>(_bouncyCastleDesKey, key => new BouncyCastleDes(AlgoSettings)); }
        }

        /// <summary>
        /// 获取 BouncyCastle RSA 非对称算法。
        /// </summary>
        public virtual IBouncyCastleAsymmetryAlgorithm BouncyCastleRsa
        {
            get { return SingletonManager.Resolve<IBouncyCastleAsymmetryAlgorithm>(_bouncyCastleRsaKey, key => new BouncyCastleRsa(AlgoSettings)); }
        }

    }
}
