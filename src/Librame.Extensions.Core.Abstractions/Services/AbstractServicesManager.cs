#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Librame.Extensions.Core.Services
{
    /// <summary>
    /// 抽象服务集合管理器。
    /// </summary>
    /// <typeparam name="TService">指定的服务类型。</typeparam>
    /// <typeparam name="TDefault">指定的默认服务类型。</typeparam>
    [SuppressMessage("Microsoft.Design", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
    public abstract class AbstractServicesManager<TService, TDefault> : AbstractServicesManager<TService>, IServicesManager<TService, TDefault>
        where TService : ISortableService
        where TDefault : TService
    {
        private static readonly Type _defaultServiceType
            = typeof(TDefault);


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
        /// <value>返回 <typeparamref name="TService"/>。</value>
        public override TService DefaultService
            => Services.Single(p => p.GetType() == _defaultServiceType);
    }


    /// <summary>
    /// 抽象服务集合管理器。
    /// </summary>
    /// <typeparam name="TService">指定的服务类型。</typeparam>
    [SuppressMessage("Microsoft.Design", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
    public abstract class AbstractServicesManager<TService> : IServicesManager<TService>
        where TService : ISortableService
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractServicesManager{TService}"/>。
        /// </summary>
        /// <param name="services">给定的服务集合。</param>
        protected AbstractServicesManager(IEnumerable<TService> services)
        {
            services.NotEmpty(nameof(services));

            Services = services.OrderBy(key => key.Priority).AsReadOnlyList();
            DefaultService = Services[0];
        }


        /// <summary>
        /// 服务集合。
        /// </summary>
        /// <value>返回 <typeparamref name="TService"/> of <see cref="IEnumerable{T}"/>。</value>
        public IReadOnlyList<TService> Services { get; }

        /// <summary>
        /// 默认服务。
        /// </summary>
        /// <value>返回 <typeparamref name="TService"/>。</value>
        public virtual TService DefaultService { get; }


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
