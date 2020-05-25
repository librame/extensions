#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Librame.Extensions.Core.Services
{
    /// <summary>
    /// 服务集合管理器接口。
    /// </summary>
    /// <typeparam name="TService">指定的服务类型。</typeparam>
    /// <typeparam name="TDefault">指定的默认服务类型。</typeparam>
    [SuppressMessage("Microsoft.Design", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
    public interface IServicesManager<out TService, TDefault> : IServicesManager<TService>
        where TService : ISortableService
        where TDefault : TService
    {
    }


    /// <summary>
    /// 服务集合管理器接口。
    /// </summary>
    /// <typeparam name="TService">指定的服务类型。</typeparam>
    [SuppressMessage("Microsoft.Design", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
    public interface IServicesManager<out TService> : IEnumerable<TService>
        where TService : ISortableService
    {
        /// <summary>
        /// 服务集合。
        /// </summary>
        /// <value>返回 <typeparamref name="TService"/> of <see cref="IEnumerable{T}"/>。</value>
        IReadOnlyList<TService> Services { get; }

        /// <summary>
        /// 默认服务。
        /// </summary>
        /// <value>返回 <typeparamref name="TService"/>。</value>
        TService DefaultService { get; }
    }
}
