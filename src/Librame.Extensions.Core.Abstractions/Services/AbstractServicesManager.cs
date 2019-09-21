#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 抽象服务集合管理器。
    /// </summary>
    /// <typeparam name="TService">指定的服务类型。</typeparam>
    /// <typeparam name="TDefault">指定的默认服务类型。</typeparam>
    public abstract class AbstractServicesManager<TService, TDefault> : AbstractServicesManager<TService>, IServicesManager<TService, TDefault>
        where TService : IService
        where TDefault : TService
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractServicesManager{TService, TDefault}"/>。
        /// </summary>
        /// <param name="services">给定的服务集合。</param>
        protected AbstractServicesManager(IEnumerable<TService> services)
            : base(services)
        {
        }


        /// <summary>
        /// 默认服务。
        /// </summary>
        public override TService Default
            => Services.Single(p => p.GetType() == typeof(TDefault));
    }


    /// <summary>
    /// 抽象服务集合管理器。
    /// </summary>
    /// <typeparam name="TService">指定的服务类型。</typeparam>
    public abstract class AbstractServicesManager<TService> : IServicesManager<TService>
        where TService : IService
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractServicesManager{TService}"/>。
        /// </summary>
        /// <param name="services">给定的服务集合。</param>
        protected AbstractServicesManager(IEnumerable<TService> services)
        {
            Services = services.NotNullOrEmpty(nameof(services));
        }


        /// <summary>
        /// 服务集合。
        /// </summary>
        public IEnumerable<TService> Services { get; }

        /// <summary>
        /// 默认服务。
        /// </summary>
        public virtual TService Default => Services.First();


        /// <summary>
        /// 获取枚举器。
        /// </summary>
        /// <returns>返回 <see cref="IEnumerator{TService}"/>。</returns>
        public IEnumerator<TService> GetEnumerator()
            => Services.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }
}
