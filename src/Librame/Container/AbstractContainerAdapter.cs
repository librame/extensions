#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Container
{
    /// <summary>
    /// 抽象容器适配器。
    /// </summary>
    public abstract class AbstractContainerAdapter : Adaptation.AbstractAdapter
    {
        /// <summary>
        /// 获取适配器信息。
        /// </summary>
        public override Adaptation.AdapterInfo AdapterInfo
        {
            get { return new Adaptation.AdapterInfo("Container"); }
        }

    }
}
