#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;

namespace Librame.Extensions.Data
{
    using Core;

    /// <summary>
    /// 存储接口。
    /// </summary>
    /// <typeparam name="TAccessor">指定的访问器类型。</typeparam>
    public interface IStore<out TAccessor> : IStore
        where TAccessor : IAccessor
    {
        /// <summary>
        /// 覆盖数据访问器接口实例。
        /// </summary>
        /// <value>返回 <typeparamref name="TAccessor"/>。</value>
        new TAccessor Accessor { get; }
    }


    /// <summary>
    /// 存储接口。
    /// </summary>
    public interface IStore : IDisposable
    {
        /// <summary>
        /// 数据访问器。
        /// </summary>
        /// <value>返回 <see cref="IAccessor"/>。</value>
        IAccessor Accessor { get; }


        /// <summary>
        /// 服务提供程序。
        /// </summary>
        IServiceProvider ServiceProvider { get; }

        /// <summary>
        /// 服务工厂。
        /// </summary>
        ServiceFactoryDelegate ServiceFactory { get; }
    }
}
