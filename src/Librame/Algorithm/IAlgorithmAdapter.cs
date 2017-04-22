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

    /// <summary>
    /// 算法适配器接口。
    /// </summary>
    public interface IAlgorithmAdapter : Adaptation.IAdapter
    {
        /// <summary>
        /// 获取或设置 <see cref="Algorithm.AlgorithmSettings"/>。
        /// </summary>
        AlgorithmSettings AlgoSettings { get; set; }


        /// <summary>
        /// 获取散列算法。
        /// </summary>
        IHashAlgorithm Hash { get; }


        /// <summary>
        /// 标准 AES 对称算法。
        /// </summary>
        ISymmetryAlgorithm StandardAes { get; }

        /// <summary>
        /// 标准 DES 对称算法。
        /// </summary>
        ISymmetryAlgorithm StandardDes { get; }


        /// <summary>
        /// 获取 BouncyCastle AES 对称算法。
        /// </summary>
        ISymmetryAlgorithm BouncyCastleAes { get; }

        /// <summary>
        /// 获取 BouncyCastle DES 对称算法。
        /// </summary>
        ISymmetryAlgorithm BouncyCastleDes { get; }

        /// <summary>
        /// 获取 BouncyCastle RSA 非对称算法。
        /// </summary>
        IBouncyCastleAsymmetryAlgorithm BouncyCastleRsa { get; }
    }
}
