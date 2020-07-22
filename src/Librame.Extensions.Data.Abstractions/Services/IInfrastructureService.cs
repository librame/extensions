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
        /// <param name="isRequired">是必需的服务（可选；默认必需，不存在将抛出异常）。</param>
        /// <returns>返回 <typeparamref name="TService"/>。</returns>
        TService GetService<TService>(bool isRequired = true);

        /// <summary>
        /// 获取服务提供程序。
        /// </summary>
        /// <returns>返回 <see cref="IServiceProvider"/>。</returns>
        IServiceProvider GetServiceProvider();
    }
}
