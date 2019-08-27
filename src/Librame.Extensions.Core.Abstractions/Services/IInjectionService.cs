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

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 注入服务接口。
    /// </summary>
    public interface IInjectionService : IService
    {
        /// <summary>
        /// 服务提供程序。
        /// </summary>
        IServiceProvider ServiceProvider { get; }

        /// <summary>
        /// 服务注入。
        /// </summary>
        /// <param name="service">给定要注入的服务对象。</param>
        void Inject(object service);
    }
}
