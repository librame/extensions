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

namespace Librame.Extensions.Data.Services
{
    using Core.Services;

    /// <summary>
    /// 基础设施服务接口。
    /// </summary>
    public interface IInfrastructureService : IService
    {
        /// <summary>
        /// 获取服务。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <returns>返回 <typeparamref name="TService"/>。</returns>
        TService GetService<TService>();

        /// <summary>
        /// 获取服务提供程序。
        /// </summary>
        /// <returns>返回 <see cref="IServiceProvider"/>。</returns>
        IServiceProvider GetServiceProvider();
    }
}
