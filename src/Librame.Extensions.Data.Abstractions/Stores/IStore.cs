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
    using Core.Services;
    using Data.Accessors;

    /// <summary>
    /// 存储接口。
    /// </summary>
    public interface IStore : IService
    {
        /// <summary>
        /// 访问器。
        /// </summary>
        /// <value>返回 <see cref="IAccessor"/>。</value>
        IAccessor Accessor { get; }
    }
}
