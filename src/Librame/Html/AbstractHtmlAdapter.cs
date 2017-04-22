#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Html
{
    /// <summary>
    /// 抽象 HTML 适配器。
    /// </summary>
    public abstract class AbstractHtmlAdapter : Adaptation.AbstractAdapter
    {
        /// <summary>
        /// 获取插件架构信息。
        /// </summary>
        public override Adaptation.AdapterInfo AdapterInfo
        {
            get { return new Adaptation.AdapterInfo("Html"); }
        }

    }
}
