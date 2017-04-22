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
    /// 抽象算法适配器。
    /// </summary>
    public abstract class AbstractAlgorithmAdapter : Adaptation.AbstractAdapter
    {
        /// <summary>
        /// 获取适配器信息。
        /// </summary>
        public override Adaptation.AdapterInfo AdapterInfo
        {
            get { return new Adaptation.AdapterInfo("Algorithm"); }
        }


        /// <summary>
        /// 获取或设置 <see cref="Algorithm.AlgorithmSettings"/>。
        /// </summary>
        public AlgorithmSettings AlgoSettings { get; set; }

    }
}
