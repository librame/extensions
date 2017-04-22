#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Adaptation
{
    /// <summary>
    /// 抽象适配器首选项。
    /// </summary>
    public abstract class AbstractAdapterSettings : LibrameBase<AbstractAdapterSettings>, IAdapterSettings
    {
        /// <summary>
        /// 适配器首选项。
        /// </summary>
        public AdapterSettings AdapterSettings { get; set; }
    }
}
