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
    /// 适配器管理器引用接口。
    /// </summary>
    public interface IAdapterManagerReference
    {
        /// <summary>
        /// 获取适配器管理器。
        /// </summary>
        IAdapterManager Adapters { get; }
    }
}
