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
    /// <summary>
    /// 算法首选项。
    /// </summary>
    public class AlgorithmSettings : Adaptation.AbstractAdapterSettings, Adaptation.IAdapterSettings
    {
        /// <summary>
        /// 用于派生密钥的密钥 salt。
        /// </summary>
        public string KeySalt { get; set; }

        /// <summary>
        /// 用于加密的迭代数。
        /// </summary>
        public int IterationCount { get; set; }
    }
}
