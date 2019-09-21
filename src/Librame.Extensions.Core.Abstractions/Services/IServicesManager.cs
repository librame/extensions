#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Collections.Generic;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 服务集合管理器接口。
    /// </summary>
    /// <typeparam name="TService">指定的服务类型。</typeparam>
    /// <typeparam name="TDefault">指定的默认服务类型。</typeparam>
    public interface IServicesManager<out TService, TDefault> : IServicesManager<TService>
        where TService : IService
        where TDefault : TService
    {
    }


    /// <summary>
    /// 服务集合管理器接口。
    /// </summary>
    /// <typeparam name="TService">指定的服务类型。</typeparam>
    public interface IServicesManager<out TService> : IEnumerable<TService>
        where TService : IService
    {
        /// <summary>
        /// 服务集合。
        /// </summary>
        IEnumerable<TService> Services { get; }

        /// <summary>
        /// 默认服务。
        /// </summary>
        TService Default { get; }
    }
}
