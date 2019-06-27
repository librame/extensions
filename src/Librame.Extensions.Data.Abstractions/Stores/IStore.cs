#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Data
{
    using Core;

    /// <summary>
    /// 存储接口。
    /// </summary>
    /// <typeparam name="TAccessor">指定的访问器类型。</typeparam>
    public interface IStore<TAccessor> : IStore
        where TAccessor : IAccessor
    {
        /// <summary>
        /// 真实数据访问器。
        /// </summary>
        /// <value>返回 <see cref="IAccessor"/>。</value>
        TAccessor RealAccessor { get; }
    }


    /// <summary>
    /// 存储接口。
    /// </summary>
    public interface IStore : IService
    {
        /// <summary>
        /// 数据访问器。
        /// </summary>
        /// <value>返回 <see cref="IAccessor"/>。</value>
        IAccessor Accessor { get; }
    }
}
