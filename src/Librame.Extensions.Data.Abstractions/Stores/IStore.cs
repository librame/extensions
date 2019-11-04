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
    public interface IStore : IDisposable
    {
        /// <summary>
        /// 访问器。
        /// </summary>
        /// <value>返回 <see cref="IAccessor"/>。</value>
        IAccessor Accessor { get; }


        /// <summary>
        /// 内部服务提供程序。
        /// </summary>
        /// <value>返回 <see cref="IServiceProvider"/>。</value>
        IServiceProvider InternalServiceProvider { get; }

        /// <summary>
        /// 服务工厂。
        /// </summary>
        /// <value>返回 <see cref="Core.ServiceFactory"/>。</value>
        ServiceFactory ServiceFactory { get; }
    }
}
