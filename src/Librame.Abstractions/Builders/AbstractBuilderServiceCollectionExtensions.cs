#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.DependencyInjection;
using System;

namespace Librame.Builders
{
    /// <summary>
    /// 抽象构建器服务集合静态扩展。
    /// </summary>
    public static class AbstractBuilderServiceCollectionExtensions
    {

        /// <summary>
        /// 尝试获取服务描述符。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="serviceDescriptor">输出服务描述符。</param>
        /// <returns>返回是否成功获取的布尔值。</returns>
        public static bool TryGetServiceDescriptor<TService>(this IServiceCollection services, out ServiceDescriptor serviceDescriptor)
        {
            return services.TryGetServiceDescriptor(typeof(TService), out serviceDescriptor);
        }

        /// <summary>
        /// 尝试获取服务描述符。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="serviceType">给定的服务类型。</param>
        /// <param name="serviceDescriptor">输出服务描述符。</param>
        /// <returns>返回是否成功获取的布尔值。</returns>
        public static bool TryGetServiceDescriptor(this IServiceCollection services, Type serviceType, out ServiceDescriptor serviceDescriptor)
        {
            foreach (var service in services)
            {
                if (service.ServiceType == serviceType)
                {
                    serviceDescriptor = service;
                    return true;
                }
            }

            serviceDescriptor = null;
            return false;
        }

    }
}
