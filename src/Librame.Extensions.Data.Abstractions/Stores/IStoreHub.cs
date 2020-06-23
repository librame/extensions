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
    /// <summary>
    /// 存储中心接口。
    /// </summary>
    public interface IStoreHub : IStore
    {
        /// <summary>
        /// 存储初始化器。
        /// </summary>
        /// <value>返回 <see cref="IStoreInitializer"/>。</value>
        IStoreInitializer Initializer { get; }
    }
}
