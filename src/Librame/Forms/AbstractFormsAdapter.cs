#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Forms
{
    /// <summary>
    /// 抽象窗体适配器基类。
    /// </summary>
    public abstract class AbstractFormsAdapter : Adaptation.AbstractAdapter
    {
        /// <summary>
        /// 获取适配器信息。
        /// </summary>
        public override Adaptation.AdapterInfo AdapterInfo
        {
            get { return new Adaptation.AdapterInfo("Froms"); }
        }


        /// <summary>
        /// 获取或设置 <see cref="FormsSettings"/>。
        /// </summary>
        public FormsSettings FormsSettings { get; set; }

    }
}
