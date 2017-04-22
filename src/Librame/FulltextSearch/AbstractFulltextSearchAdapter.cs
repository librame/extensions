#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.FulltextSearch
{
    /// <summary>
    /// 抽象全文检索适配器。
    /// </summary>
    public abstract class AbstractFulltextSearchAdapter : Adaptation.AbstractAdapter
    {
        /// <summary>
        /// 获取插件架构信息。
        /// </summary>
        public override Adaptation.AdapterInfo AdapterInfo
        {
            get { return new Adaptation.AdapterInfo("FulltextSearch"); }
        }

    }
}
