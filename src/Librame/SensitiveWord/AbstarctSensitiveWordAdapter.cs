#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.SensitiveWord
{
    /// <summary>
    /// 抽象敏感词适配器。
    /// </summary>
    public class AbstarctSensitiveWordAdapter : Adaptation.AbstractAdapter
    {
        /// <summary>
        /// 获取适配器信息。
        /// </summary>
        public override Adaptation.AdapterInfo AdapterInfo
        {
            get { return new Adaptation.AdapterInfo("SensitiveWord"); }
        }

    }
}
