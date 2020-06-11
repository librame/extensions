#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Data.Stores
{
    using Core;

    /// <summary>
    /// 存储中心标示接口（用于标记、约束实例）。
    /// </summary>
    public interface IStoreHubIndication : IStore, IIndication
    {
    }
}
