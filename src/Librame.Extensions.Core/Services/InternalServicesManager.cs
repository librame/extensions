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
    /// 内部服务集合管理器。
    /// </summary>
    /// <typeparam name="TService">指定的服务类型。</typeparam>
    /// <typeparam name="TDefaulter">指定的默认服务类型。</typeparam>
    public class InternalServicesManager<TService, TDefaulter> : AbstractServicesManager<TService, TDefaulter>
        where TService : IService
        where TDefaulter : TService
    {
        /// <summary>
        /// 构造一个 <see cref="InternalServicesManager{TService, TDefaulter}"/>。
        /// </summary>
        /// <param name="services">给定的服务集合。</param>
        public InternalServicesManager(IEnumerable<TService> services)
            : base(services)
        {
        }

    }


    /// <summary>
    /// 内部服务集合管理器。
    /// </summary>
    /// <typeparam name="TService">指定的服务类型。</typeparam>
    public class InternalServicesManager<TService> : AbstractServicesManager<TService>
        where TService : IService
    {
        /// <summary>
        /// 构造一个 <see cref="InternalServicesManager{TService}"/>。
        /// </summary>
        /// <param name="services">给定的服务集合。</param>
        public InternalServicesManager(IEnumerable<TService> services)
            : base(services)
        {
        }

    }

}
