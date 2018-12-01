#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions;
using System;
using System.Linq;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 抽象构建器服务集合静态扩展。
    /// </summary>
    public static class AbstractBuilderServiceCollectionExtensions
    {

        #region TryGet

        /// <summary>
        /// 尝试获取服务描述符。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="serviceDescriptor">输出服务描述符。</param>
        /// <returns>返回是否成功获取的布尔值。</returns>
        public static bool TryGet<TService>(this IServiceCollection services, out ServiceDescriptor serviceDescriptor)
           where TService : class
        {
            return services.TryGet(typeof(TService), out serviceDescriptor);
        }
        /// <summary>
        /// 尝试获取服务描述符。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <typeparam name="TImplementation">指定的实现类型。</typeparam>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="serviceDescriptor">输出服务描述符。</param>
        /// <returns>返回是否成功获取的布尔值。</returns>
        public static bool TryGet<TService, TImplementation>(this IServiceCollection services, out ServiceDescriptor serviceDescriptor)
           where TService : class
           where TImplementation : class, TService
        {
            return services.TryGet(typeof(TService), typeof(TImplementation), out serviceDescriptor);
        }

        /// <summary>
        /// 尝试获取服务描述符。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="serviceType">给定的服务类型。</param>
        /// <param name="serviceDescriptor">输出服务描述符。</param>
        /// <returns>返回是否成功获取的布尔值。</returns>
        public static bool TryGet(this IServiceCollection services, Type serviceType, out ServiceDescriptor serviceDescriptor)
        {
            return services.TryGet(serviceType, implementationType: null, out serviceDescriptor);
        }
        /// <summary>
        /// 尝试获取服务描述符。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="serviceType">给定的服务类型。</param>
        /// <param name="implementationType">给定的实现类型。</param>
        /// <param name="serviceDescriptor">输出服务描述符。</param>
        /// <returns>返回是否成功获取的布尔值。</returns>
        public static bool TryGet(this IServiceCollection services, Type serviceType, Type implementationType,
            out ServiceDescriptor serviceDescriptor)
        {
            Func<ServiceDescriptor, bool> predicate = null;

            if (implementationType.IsDefault())
                predicate = p => p.ServiceType == serviceType;
            else
                predicate = p => p.ServiceType == serviceType && p.ImplementationType == implementationType;

            serviceDescriptor = services.FirstOrDefault(predicate);

            return serviceDescriptor.IsNotDefault();
        }

        #endregion


        #region TryReplace

        /// <summary>
        /// 尝试替换服务描述符。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <returns>返回是否成功替换的布尔值。</returns>
        public static bool TryReplace<TService>(this IServiceCollection services)
           where TService : class
        {
            return services.TryReplace(typeof(TService), typeof(TService));
        }
        /// <summary>
        /// 尝试替换服务描述符。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <typeparam name="TNewImplementation">指定的新实现类型。</typeparam>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <returns>返回是否成功替换的布尔值。</returns>
        public static bool TryReplace<TService, TNewImplementation>(this IServiceCollection services)
           where TService : class
           where TNewImplementation : class, TService
        {
            return services.TryReplace(typeof(TService), typeof(TNewImplementation));
        }
        /// <summary>
        /// 尝试替换服务描述符。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <typeparam name="TOldImplementation">指定的旧实现类型。</typeparam>
        /// <typeparam name="TNewImplementation">指定的新实现类型。</typeparam>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <returns>返回是否成功替换的布尔值。</returns>
        public static bool TryReplace<TService, TOldImplementation, TNewImplementation>(this IServiceCollection services)
           where TService : class
           where TOldImplementation : class, TService
           where TNewImplementation : class, TService
        {
            return services.TryReplace(typeof(TService), typeof(TOldImplementation), typeof(TNewImplementation));
        }

        /// <summary>
        /// 尝试替换服务描述符。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="serviceType">给定的服务类型。</param>
        /// <param name="newImplementationType">给定用于替换的新实现类型。</param>
        /// <returns>返回是否成功替换的布尔值。</returns>
        public static bool TryReplace(this IServiceCollection services, Type serviceType, Type newImplementationType)
        {
            return services.TryReplace(serviceType, oldImplementationType: null, newImplementationType);
        }
        /// <summary>
        /// 尝试替换服务描述符。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="serviceType">给定的服务类型。</param>
        /// <param name="oldImplementationType">给定的旧实现类型。</param>
        /// <param name="newImplementationType">给定用于替换的新实现类型。</param>
        /// <returns>返回是否成功替换的布尔值。</returns>
        public static bool TryReplace(this IServiceCollection services, Type serviceType, Type oldImplementationType, Type newImplementationType)
        {
            if (services.TryGet(serviceType, oldImplementationType, out ServiceDescriptor serviceDescriptor))
            {
                services.Remove(serviceDescriptor);
                services.Add(ServiceDescriptor.Describe(serviceType, newImplementationType, serviceDescriptor.Lifetime));
                return true;
            }
            
            return false;
        }


        /// <summary>
        /// 尝试替换服务描述符。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <param name="implementationFactory">给定用于替换的实现类型工厂方法。</param>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <returns>返回是否成功替换的布尔值。</returns>
        public static bool TryReplace<TService>(this IServiceCollection services,
            Func<IServiceProvider, TService> implementationFactory)
           where TService : class
        {
            return services.TryReplace(typeof(TService), implementationFactory);
        }
        /// <summary>
        /// 尝试替换服务描述符。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <typeparam name="TNewImplementation">指定的新实现类型。</typeparam>
        /// <param name="newImplementationFactory">给定用于替换的新实现类型工厂方法。</param>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <returns>返回是否成功替换的布尔值。</returns>
        public static bool TryReplace<TService, TNewImplementation>(this IServiceCollection services,
            Func<IServiceProvider, TNewImplementation> newImplementationFactory)
           where TService : class
           where TNewImplementation : class, TService
        {
            return services.TryReplace(typeof(TService), newImplementationFactory);
        }
        /// <summary>
        /// 尝试替换服务描述符。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <typeparam name="TOldImplementation">指定的旧实现类型。</typeparam>
        /// <typeparam name="TNewImplementation">指定的新实现类型。</typeparam>
        /// <param name="newImplementationFactory">给定用于替换的新实现类型工厂方法。</param>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <returns>返回是否成功替换的布尔值。</returns>
        public static bool TryReplace<TService, TOldImplementation, TNewImplementation>(this IServiceCollection services,
            Func<IServiceProvider, TNewImplementation> newImplementationFactory)
           where TService : class
           where TOldImplementation : class, TService
           where TNewImplementation : class, TService
        {
            return services.TryReplace(typeof(TService), typeof(TOldImplementation), newImplementationFactory);
        }

        /// <summary>
        /// 尝试替换服务描述符。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="serviceType">给定的服务类型。</param>
        /// <param name="newImplementationFactory">给定用于替换的新实现类型工厂方法。</param>
        /// <returns>返回是否成功替换的布尔值。</returns>
        public static bool TryReplace(this IServiceCollection services, Type serviceType, Func<IServiceProvider, object> newImplementationFactory)
        {
            return services.TryReplace(serviceType, oldImplementationType: null, newImplementationFactory);
        }
        /// <summary>
        /// 尝试替换服务描述符。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="serviceType">给定的服务类型。</param>
        /// <param name="oldImplementationType">给定的旧实现类型。</param>
        /// <param name="newImplementationFactory">给定用于替换的新实现类型工厂方法。</param>
        /// <returns>返回是否成功替换的布尔值。</returns>
        public static bool TryReplace(this IServiceCollection services, Type serviceType, Type oldImplementationType,
            Func<IServiceProvider, object> newImplementationFactory)
        {
            if (services.TryGet(serviceType, oldImplementationType, out ServiceDescriptor serviceDescriptor))
            {
                services.Remove(serviceDescriptor);
                services.Add(ServiceDescriptor.Describe(serviceType, newImplementationFactory, serviceDescriptor.Lifetime));
                return true;
            }

            return false;
        }

        #endregion

    }
}
