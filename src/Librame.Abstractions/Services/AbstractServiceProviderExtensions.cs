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

namespace Librame.Extensions
{
    /// <summary>
    /// 抽象服务提供程序静态扩展。
    /// </summary>
    public static class AbstractServiceProviderExtensions
    {

        /// <summary>
        /// 获取服务实例。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <param name="serviceProvider">给定的 <see cref="IServiceProvider"/>。</param>
        /// <returns>返回服务实例。</returns>
        public static TService GetService<TService>(this IServiceProvider serviceProvider)
            where TService : class
        {
            return serviceProvider.GetService(typeof(TService)) as TService;
        }

        /// <summary>
        /// 获取必需的服务实例。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <param name="serviceProvider">给定的 <see cref="IServiceProvider"/>。</param>
        /// <returns>返回服务实例。</returns>
        public static TService GetRequiredService<TService>(this IServiceProvider serviceProvider)
            where TService : class
        {
            var service = serviceProvider.GetService<TService>();

            return service.NotDefault(nameof(service));
        }

    }
}
