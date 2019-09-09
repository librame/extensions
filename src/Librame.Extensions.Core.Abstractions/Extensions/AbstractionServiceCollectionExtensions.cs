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
    /// 抽象服务集合静态扩展。
    /// </summary>
    public static class AbstractionServiceCollectionExtensions
    {

        #region TryGet

        /// <summary>
        /// 尝试得到服务描述符。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="serviceDescriptor">输出服务描述符。</param>
        /// <returns>返回是否成功获取的布尔值。</returns>
        public static bool TryGet<TService>(this IServiceCollection services, out ServiceDescriptor serviceDescriptor)
            where TService : class
            => services.TryGet(typeof(TService), out serviceDescriptor);

        /// <summary>
        /// 尝试得到服务描述符。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <typeparam name="TImplementation">指定的实现类型。</typeparam>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="serviceDescriptor">输出服务描述符。</param>
        /// <returns>返回是否成功获取的布尔值。</returns>
        public static bool TryGet<TService, TImplementation>(this IServiceCollection services, out ServiceDescriptor serviceDescriptor)
            where TService : class
            where TImplementation : class, TService
            => services.TryGet(typeof(TService), typeof(TImplementation), out serviceDescriptor);

        /// <summary>
        /// 尝试得到服务描述符。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="serviceType">给定的服务类型。</param>
        /// <param name="serviceDescriptor">输出服务描述符。</param>
        /// <returns>返回是否成功获取的布尔值。</returns>
        public static bool TryGet(this IServiceCollection services, Type serviceType, out ServiceDescriptor serviceDescriptor)
            => services.TryGet(serviceType, implementationType: null, out serviceDescriptor);

        /// <summary>
        /// 尝试得到服务描述符。
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

            if (implementationType.IsNull())
                predicate = p => p.ServiceType == serviceType;
            else
                predicate = p => p.ServiceType == serviceType && p.ImplementationType == implementationType;

            serviceDescriptor = services.FirstOrDefault(predicate);

            return serviceDescriptor.IsNotNull();
        }

        #endregion


        #region TryReplace ServiceType

        /// <summary>
        /// 尝试替换服务描述符。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <returns>返回是否成功替换的布尔值。</returns>
        public static bool TryReplace<TService>(this IServiceCollection services)
            where TService : class
            => services.TryReplace(typeof(TService), typeof(TService), out _);

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
            => services.TryReplace(typeof(TService), typeof(TNewImplementation), out _);

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
            => services.TryReplace(typeof(TService), typeof(TOldImplementation), typeof(TNewImplementation), out _);


        /// <summary>
        /// 尝试替换服务描述符。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="serviceType">给定的服务类型。</param>
        /// <param name="newImplementationType">给定用于替换的新实现类型。</param>
        /// <returns>返回是否成功替换的布尔值。</returns>
        public static bool TryReplace(this IServiceCollection services, Type serviceType, Type newImplementationType)
            => services.TryReplace(serviceType, oldImplementationType: null, newImplementationType, out _);

        /// <summary>
        /// 尝试替换服务描述符。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="serviceType">给定的服务类型。</param>
        /// <param name="oldImplementationType">给定的旧实现类型。</param>
        /// <param name="newImplementationType">给定用于替换的新实现类型。</param>
        /// <returns>返回是否成功替换的布尔值。</returns>
        public static bool TryReplace(this IServiceCollection services, Type serviceType, Type oldImplementationType, Type newImplementationType)
            => services.TryReplace(serviceType, oldImplementationType, newImplementationType, out _);


        /// <summary>
        /// 尝试替换服务描述符。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="descriptor">输出得到的 <see cref="ServiceDescriptor"/>。</param>
        /// <returns>返回是否成功替换的布尔值。</returns>
        public static bool TryReplace<TService>(this IServiceCollection services, out ServiceDescriptor descriptor)
            where TService : class
            => services.TryReplace(typeof(TService), typeof(TService), out descriptor);

        /// <summary>
        /// 尝试替换服务描述符。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <typeparam name="TNewImplementation">指定的新实现类型。</typeparam>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="descriptor">输出得到的 <see cref="ServiceDescriptor"/>。</param>
        /// <returns>返回是否成功替换的布尔值。</returns>
        public static bool TryReplace<TService, TNewImplementation>(this IServiceCollection services,
            out ServiceDescriptor descriptor)
            where TService : class
            where TNewImplementation : class, TService
            => services.TryReplace(typeof(TService), typeof(TNewImplementation), out descriptor);

        /// <summary>
        /// 尝试替换服务描述符。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <typeparam name="TOldImplementation">指定的旧实现类型。</typeparam>
        /// <typeparam name="TNewImplementation">指定的新实现类型。</typeparam>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="descriptor">输出得到的 <see cref="ServiceDescriptor"/>。</param>
        /// <returns>返回是否成功替换的布尔值。</returns>
        public static bool TryReplace<TService, TOldImplementation, TNewImplementation>(this IServiceCollection services,
            out ServiceDescriptor descriptor)
            where TService : class
            where TOldImplementation : class, TService
            where TNewImplementation : class, TService
            => services.TryReplace(typeof(TService), typeof(TOldImplementation), typeof(TNewImplementation), out descriptor);


        /// <summary>
        /// 尝试替换服务描述符。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="serviceType">给定的服务类型。</param>
        /// <param name="newImplementationType">给定用于替换的新实现类型。</param>
        /// <param name="descriptor">输出得到的 <see cref="ServiceDescriptor"/>。</param>
        /// <returns>返回是否成功替换的布尔值。</returns>
        public static bool TryReplace(this IServiceCollection services, Type serviceType, Type newImplementationType,
            out ServiceDescriptor descriptor)
            => services.TryReplace(serviceType, oldImplementationType: null, newImplementationType, out descriptor);

        /// <summary>
        /// 尝试替换服务描述符。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="serviceType">给定的服务类型。</param>
        /// <param name="oldImplementationType">给定的旧实现类型。</param>
        /// <param name="newImplementationType">给定用于替换的新实现类型。</param>
        /// <param name="descriptor">输出得到的 <see cref="ServiceDescriptor"/>。</param>
        /// <returns>返回是否成功替换的布尔值。</returns>
        public static bool TryReplace(this IServiceCollection services, Type serviceType, Type oldImplementationType, Type newImplementationType,
            out ServiceDescriptor descriptor)
        {
            if (services.TryGet(serviceType, oldImplementationType, out descriptor))
            {
                services.Remove(descriptor);
                services.Add(ServiceDescriptor.Describe(serviceType, newImplementationType, descriptor.Lifetime));
                return true;
            }

            return false;
        }

        #endregion


        #region TryReplace IServiceProviderFactory

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
            => services.TryReplace(typeof(TService), implementationFactory, out _);

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
            => services.TryReplace(typeof(TService), newImplementationFactory, out _);

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
            => services.TryReplace(typeof(TService), typeof(TOldImplementation), newImplementationFactory, out _);


        /// <summary>
        /// 尝试替换服务描述符。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="serviceType">给定的服务类型。</param>
        /// <param name="newImplementationFactory">给定用于替换的新实现类型工厂方法。</param>
        /// <returns>返回是否成功替换的布尔值。</returns>
        public static bool TryReplace(this IServiceCollection services, Type serviceType, Func<IServiceProvider, object> newImplementationFactory)
            => services.TryReplace(serviceType, oldImplementationType: null, newImplementationFactory, out _);

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
            => services.TryReplace(serviceType, oldImplementationType, newImplementationFactory, out _);


        /// <summary>
        /// 尝试替换服务描述符。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="implementationFactory">给定用于替换的实现类型工厂方法。</param>
        /// <param name="descriptor">输出得到的 <see cref="ServiceDescriptor"/>。</param>
        /// <returns>返回是否成功替换的布尔值。</returns>
        public static bool TryReplace<TService>(this IServiceCollection services,
            Func<IServiceProvider, TService> implementationFactory, out ServiceDescriptor descriptor)
            where TService : class
            => services.TryReplace(typeof(TService), implementationFactory, out descriptor);

        /// <summary>
        /// 尝试替换服务描述符。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <typeparam name="TNewImplementation">指定的新实现类型。</typeparam>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="newImplementationFactory">给定用于替换的新实现类型工厂方法。</param>
        /// <param name="descriptor">输出得到的 <see cref="ServiceDescriptor"/>。</param>
        /// <returns>返回是否成功替换的布尔值。</returns>
        public static bool TryReplace<TService, TNewImplementation>(this IServiceCollection services,
            Func<IServiceProvider, TNewImplementation> newImplementationFactory, out ServiceDescriptor descriptor)
            where TService : class
            where TNewImplementation : class, TService
            => services.TryReplace(typeof(TService), newImplementationFactory, out descriptor);

        /// <summary>
        /// 尝试替换服务描述符。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <typeparam name="TOldImplementation">指定的旧实现类型。</typeparam>
        /// <typeparam name="TNewImplementation">指定的新实现类型。</typeparam>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="newImplementationFactory">给定用于替换的新实现类型工厂方法。</param>
        /// <param name="descriptor">输出得到的 <see cref="ServiceDescriptor"/>。</param>
        /// <returns>返回是否成功替换的布尔值。</returns>
        public static bool TryReplace<TService, TOldImplementation, TNewImplementation>(this IServiceCollection services,
            Func<IServiceProvider, TNewImplementation> newImplementationFactory, out ServiceDescriptor descriptor)
            where TService : class
            where TOldImplementation : class, TService
            where TNewImplementation : class, TService
            => services.TryReplace(typeof(TService), typeof(TOldImplementation), newImplementationFactory, out descriptor);


        /// <summary>
        /// 尝试替换服务描述符。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="serviceType">给定的服务类型。</param>
        /// <param name="newImplementationFactory">给定用于替换的新实现类型工厂方法。</param>
        /// <param name="descriptor">输出得到的 <see cref="ServiceDescriptor"/>。</param>
        /// <returns>返回是否成功替换的布尔值。</returns>
        public static bool TryReplace(this IServiceCollection services, Type serviceType,
            Func<IServiceProvider, object> newImplementationFactory, out ServiceDescriptor descriptor)
            => services.TryReplace(serviceType, oldImplementationType: null, newImplementationFactory, out descriptor);

        /// <summary>
        /// 尝试替换服务描述符。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="serviceType">给定的服务类型。</param>
        /// <param name="oldImplementationType">给定的旧实现类型。</param>
        /// <param name="newImplementationFactory">给定用于替换的新实现类型工厂方法。</param>
        /// <param name="descriptor">输出得到的 <see cref="ServiceDescriptor"/>。</param>
        /// <returns>返回是否成功替换的布尔值。</returns>
        public static bool TryReplace(this IServiceCollection services, Type serviceType, Type oldImplementationType,
            Func<IServiceProvider, object> newImplementationFactory, out ServiceDescriptor descriptor)
        {
            if (services.TryGet(serviceType, oldImplementationType, out descriptor))
            {
                services.Remove(descriptor);
                services.Add(ServiceDescriptor.Describe(serviceType, newImplementationFactory, descriptor.Lifetime));
                return true;
            }

            return false;
        }

        #endregion


        #region TryReplace Instance

        /// <summary>
        /// 尝试替换服务描述符。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="newInstance">给定的新实例。</param>
        /// <returns>返回是否成功替换的布尔值。</returns>
        public static bool TryReplace<TService>(this IServiceCollection services, TService newInstance)
            => services.TryReplace(typeof(TService), newInstance, out _);

        /// <summary>
        /// 尝试替换服务描述符。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <typeparam name="TNewImplementation">指定的新实现类型。</typeparam>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="newInstance">给定的新实例。</param>
        /// <returns>返回是否成功替换的布尔值。</returns>
        public static bool TryReplace<TService, TNewImplementation>(this IServiceCollection services, TNewImplementation newInstance)
            => services.TryReplace(typeof(TService), newInstance, out _);

        /// <summary>
        /// 尝试替换服务描述符。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <typeparam name="TOldImplementation">指定的旧实现类型。</typeparam>
        /// <typeparam name="TNewImplementation">指定的新实现类型。</typeparam>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="newInstance">给定的新实例。</param>
        /// <returns>返回是否成功替换的布尔值。</returns>
        public static bool TryReplace<TService, TOldImplementation, TNewImplementation>(this IServiceCollection services,
            TNewImplementation newInstance)
            where TService : class
            where TOldImplementation : class, TService
            where TNewImplementation : class, TService
            => services.TryReplace(typeof(TService), typeof(TOldImplementation), newInstance, out _);


        /// <summary>
        /// 尝试替换服务描述符。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="serviceType">给定的服务类型。</param>
        /// <param name="newInstance">给定的新实例。</param>
        /// <returns>返回是否成功替换的布尔值。</returns>
        public static bool TryReplace(this IServiceCollection services, Type serviceType, object newInstance)
            => services.TryReplace(serviceType, oldImplementationType: null, newInstance, out _);

        /// <summary>
        /// 尝试替换服务描述符。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="serviceType">给定的服务类型。</param>
        /// <param name="oldImplementationType">给定的旧实现类型。</param>
        /// <param name="newInstance">给定的新实例。</param>
        /// <returns>返回是否成功替换的布尔值。</returns>
        public static bool TryReplace(this IServiceCollection services, Type serviceType, Type oldImplementationType, object newInstance)
            => services.TryReplace(serviceType, oldImplementationType, newInstance, out _);


        /// <summary>
        /// 尝试替换服务描述符。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="newInstance">给定的新实例。</param>
        /// <param name="descriptor">输出得到的 <see cref="ServiceDescriptor"/>。</param>
        /// <returns>返回是否成功替换的布尔值。</returns>
        public static bool TryReplace<TService>(this IServiceCollection services, TService newInstance,
            out ServiceDescriptor descriptor)
            => services.TryReplace(typeof(TService), newInstance, out descriptor);

        /// <summary>
        /// 尝试替换服务描述符。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <typeparam name="TNewImplementation">指定的新实现类型。</typeparam>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="newInstance">给定的新实例。</param>
        /// <param name="descriptor">输出得到的 <see cref="ServiceDescriptor"/>。</param>
        /// <returns>返回是否成功替换的布尔值。</returns>
        public static bool TryReplace<TService, TNewImplementation>(this IServiceCollection services,
            TNewImplementation newInstance, out ServiceDescriptor descriptor)
            => services.TryReplace(typeof(TService), newInstance, out descriptor);

        /// <summary>
        /// 尝试替换服务描述符。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <typeparam name="TOldImplementation">指定的旧实现类型。</typeparam>
        /// <typeparam name="TNewImplementation">指定的新实现类型。</typeparam>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="newInstance">给定的新实例。</param>
        /// <param name="descriptor">输出得到的 <see cref="ServiceDescriptor"/>。</param>
        /// <returns>返回是否成功替换的布尔值。</returns>
        public static bool TryReplace<TService, TOldImplementation, TNewImplementation>(this IServiceCollection services,
            TNewImplementation newInstance, out ServiceDescriptor descriptor)
            where TService : class
            where TOldImplementation : class, TService
            where TNewImplementation : class, TService
            => services.TryReplace(typeof(TService), typeof(TOldImplementation), newInstance, out descriptor);


        /// <summary>
        /// 尝试替换服务描述符。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="serviceType">给定的服务类型。</param>
        /// <param name="newInstance">给定的新实例。</param>
        /// <param name="descriptor">输出得到的 <see cref="ServiceDescriptor"/>。</param>
        /// <returns>返回是否成功替换的布尔值。</returns>
        public static bool TryReplace(this IServiceCollection services, Type serviceType, object newInstance,
            out ServiceDescriptor descriptor)
            => services.TryReplace(serviceType, oldImplementationType: null, newInstance, out descriptor);

        /// <summary>
        /// 尝试替换服务描述符。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="serviceType">给定的服务类型。</param>
        /// <param name="oldImplementationType">给定的旧实现类型。</param>
        /// <param name="newInstance">给定的新实例。</param>
        /// <param name="descriptor">输出得到的 <see cref="ServiceDescriptor"/>。</param>
        /// <returns>返回是否成功替换的布尔值。</returns>
        public static bool TryReplace(this IServiceCollection services, Type serviceType, Type oldImplementationType,
            object newInstance, out ServiceDescriptor descriptor)
        {
            if (services.TryGet(serviceType, oldImplementationType, out descriptor))
            {
                services.Remove(descriptor);
                services.Add(new ServiceDescriptor(serviceType, newInstance));
                return true;
            }

            return false;
        }

        #endregion

    }
}
