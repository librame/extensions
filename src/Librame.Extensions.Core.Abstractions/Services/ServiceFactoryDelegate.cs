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

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 服务工厂委托。
    /// </summary>
    /// <param name="serviceType">给定的服务类型。</param>
    /// <returns>返回对象。</returns>
    public delegate object ServiceFactoryDelegate(Type serviceType);


    /// <summary>
    /// 服务工厂委托静态扩展。
    /// </summary>
    public static class ServiceFactoryDelegateExtensions
    {
        /// <summary>
        /// 获取必需的服务。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <typeparam name="TImplementation">指定的服务实现类型。</typeparam>
        /// <param name="serviceFactory">给定的 <see cref="ServiceFactoryDelegate"/>。</param>
        /// <returns>返回服务。</returns>
        public static TService GetRequiredService<TService, TImplementation>(this ServiceFactoryDelegate serviceFactory)
            where TImplementation : TService
        {
            var service = serviceFactory.GetService<TService, TImplementation>(out Type implementationType);
            return service.NotNullService(implementationType);
        }

        /// <summary>
        /// 获取必需的服务。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <param name="serviceFactory">给定的 <see cref="ServiceFactoryDelegate"/>。</param>
        /// <param name="implementationType">给定的服务实现类型。</param>
        /// <returns>返回服务。</returns>
        public static TService GetRequiredService<TService>(this ServiceFactoryDelegate serviceFactory, Type implementationType)
        {
            var service = serviceFactory.GetService<TService>(implementationType);
            return service.NotNullService(implementationType);
        }

        /// <summary>
        /// 获取必需的服务。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <param name="serviceFactory">给定的 <see cref="ServiceFactoryDelegate"/>。</param>
        /// <returns>返回服务。</returns>
        public static TService GetRequiredService<TService>(this ServiceFactoryDelegate serviceFactory)
        {
            var service = serviceFactory.GetService<TService>(out Type serviceType);
            return service.NotNullService(serviceType);
        }

        /// <summary>
        /// 获取必需的服务。
        /// </summary>
        /// <param name="serviceFactory">给定的 <see cref="ServiceFactoryDelegate"/>。</param>
        /// <param name="serviceType">给定的服务类型。</param>
        /// <returns>返回对象。</returns>
        public static object GetRequiredService(this ServiceFactoryDelegate serviceFactory, Type serviceType)
        {
            var service = serviceFactory.GetService(serviceType);
            return service.NotNullService(serviceType);
        }

        private static TService NotNullService<TService>(this TService service, Type serviceType)
        {
            if (service.IsNull())
                throw new ArgumentException($"Cannot resolve service {serviceType.GetSimpleFullName()}");

            return service;
        }


        /// <summary>
        /// 获取服务。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <typeparam name="TImplementation">指定的服务实现类型。</typeparam>
        /// <param name="serviceFactory">给定的 <see cref="ServiceFactoryDelegate"/>。</param>
        /// <returns>返回服务。</returns>
        public static TService GetService<TService, TImplementation>(this ServiceFactoryDelegate serviceFactory)
            where TImplementation : TService
            => serviceFactory.GetService<TService, TImplementation>(out _);

        private static TService GetService<TService, TImplementation>(this ServiceFactoryDelegate serviceFactory, out Type implementationType)
            where TImplementation : TService
        {
            implementationType = typeof(TImplementation);
            return (TService)serviceFactory.GetService(implementationType);
        }

        /// <summary>
        /// 获取服务。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <param name="serviceFactory">给定的 <see cref="ServiceFactoryDelegate"/>。</param>
        /// <param name="implementationType">给定的服务实现类型。</param>
        /// <returns>返回服务。</returns>
        public static TService GetService<TService>(this ServiceFactoryDelegate serviceFactory, Type implementationType)
        {
            implementationType.AssignableToBase(typeof(TService));
            return (TService)serviceFactory.GetService(implementationType);
        }

        /// <summary>
        /// 获取服务。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <param name="serviceFactory">给定的 <see cref="ServiceFactoryDelegate"/>。</param>
        /// <returns>返回服务。</returns>
        public static TService GetService<TService>(this ServiceFactoryDelegate serviceFactory)
            => serviceFactory.GetService<TService>(out _);

        private static TService GetService<TService>(this ServiceFactoryDelegate serviceFactory, out Type serviceType)
        {
            serviceType = typeof(TService);
            return (TService)serviceFactory.GetService(serviceType);
        }

        /// <summary>
        /// 获取服务。
        /// </summary>
        /// <param name="serviceFactory">给定的 <see cref="ServiceFactoryDelegate"/>。</param>
        /// <param name="serviceType">给定的服务类型。</param>
        /// <returns>返回对象。</returns>
        public static object GetService(this ServiceFactoryDelegate serviceFactory, Type serviceType)
            => serviceFactory.NotNull(nameof(serviceFactory)).Invoke(serviceType);

    }
}
