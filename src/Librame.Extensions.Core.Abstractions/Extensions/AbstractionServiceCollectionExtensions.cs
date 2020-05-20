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
using Librame.Extensions.Core.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Microsoft.Extensions.DependencyInjection
{
    using Extensions;

    /// <summary>
    /// 抽象服务集合静态扩展。
    /// </summary>
    public static class AbstractionServiceCollectionExtensions
    {

        #region AddByCharacteristics

        /// <summary>
        /// 通过特征添加服务。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="serviceType">给定的服务类型。</param>
        /// <param name="factory">给定的服务对象工厂方法。</param>
        /// <param name="characteristics">给定的 <see cref="ServiceCharacteristics"/>。</param>
        /// <returns>返回 <see cref="IServiceCollection"/>。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数")]
        public static IServiceCollection AddByCharacteristics(this IServiceCollection services,
            Type serviceType, Func<IServiceProvider, object> factory, ServiceCharacteristics characteristics)
        {
            services.NotNull(nameof(services));

            var descriptor = new ServiceDescriptor(serviceType, factory, characteristics.Lifetime);
            return services.AddByCharacteristics(descriptor, characteristics);
        }

        /// <summary>
        /// 通过特征添加服务。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="serviceType">给定的服务类型。</param>
        /// <param name="implementationType">给定的实现类型。</param>
        /// <param name="characteristics">给定的 <see cref="ServiceCharacteristics"/>。</param>
        /// <returns>返回 <see cref="IServiceCollection"/>。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数")]
        public static IServiceCollection AddByCharacteristics(this IServiceCollection services,
            Type serviceType, Type implementationType, ServiceCharacteristics characteristics)
        {
            services.NotNull(nameof(services));
            
            var descriptor = new ServiceDescriptor(serviceType, implementationType, characteristics.Lifetime);
            return services.AddByCharacteristics(descriptor, characteristics);
        }

        /// <summary>
        /// 通过特征添加服务集合。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="serviceType">给定的服务类型。</param>
        /// <param name="implementationTypes">给定的实现类型集合。</param>
        /// <param name="characteristics">给定的 <see cref="ServiceCharacteristics"/>。</param>
        /// <returns>返回 <see cref="IServiceCollection"/>。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数")]
        public static IServiceCollection AddByCharacteristics(this IServiceCollection services,
            Type serviceType, IEnumerable<Type> implementationTypes, ServiceCharacteristics characteristics)
        {
            services.NotNull(nameof(services));

            foreach (var implType in implementationTypes)
            {
                var descriptor = new ServiceDescriptor(serviceType, implType, characteristics.Lifetime);
                services.AddByCharacteristics(descriptor, characteristics);
            }

            return services;
        }

        private static IServiceCollection AddByCharacteristics(this IServiceCollection services,
            ServiceDescriptor descriptor, ServiceCharacteristics characteristics)
        {
            if (characteristics.TryAdd && services.Contains(descriptor))
                return services;

            services.Add(descriptor);
            return services;
        }

        #endregion


        #region TryAddEnumerable

        /// <summary>
        /// 尝试添加可枚举服务集合。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="implementationTypes">给定的实现类型集合。</param>
        /// <param name="lifetime">给定的 <see cref="ServiceLifetime"/>（可选；默认为单例）。</param>
        public static void TryAddEnumerable<TService>(this IServiceCollection services,
            IEnumerable<Type> implementationTypes,
            ServiceLifetime lifetime = ServiceLifetime.Singleton)
            => services.TryAddEnumerable(typeof(TService), implementationTypes, lifetime);

        /// <summary>
        /// 尝试添加可枚举服务集合。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="serviceType">给定的服务类型。</param>
        /// <param name="implementationTypes">给定的实现类型集合。</param>
        /// <param name="lifetime">给定的 <see cref="ServiceLifetime"/>（可选；默认为单例）。</param>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static void TryAddEnumerable(this IServiceCollection services,
            Type serviceType, IEnumerable<Type> implementationTypes,
            ServiceLifetime lifetime = ServiceLifetime.Singleton)
        {
            implementationTypes.NotEmpty(nameof(implementationTypes));

            services.TryAddEnumerable(ToDescriptors());

            // ToDescriptors
            IEnumerable<ServiceDescriptor> ToDescriptors()
            {
                foreach (var implType in implementationTypes)
                    yield return new ServiceDescriptor(serviceType, implType, lifetime);
            }
        }

        #endregion


        #region TryGet

        /// <summary>
        /// 尝试得到服务描述符。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="descriptor">输出 <see cref="ServiceDescriptor"/>。</param>
        /// <returns>返回是否成功获取的布尔值。</returns>
        public static bool TryGet<TService>(this IServiceCollection services, out ServiceDescriptor descriptor)
            where TService : class
            => services.TryGet(typeof(TService), out descriptor);

        /// <summary>
        /// 尝试得到服务描述符。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <typeparam name="TImplementation">指定的实现类型。</typeparam>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="descriptor">输出 <see cref="ServiceDescriptor"/>。</param>
        /// <returns>返回是否成功获取的布尔值。</returns>
        public static bool TryGet<TService, TImplementation>(this IServiceCollection services, out ServiceDescriptor descriptor)
            where TService : class
            where TImplementation : class, TService
            => services.TryGet(typeof(TService), typeof(TImplementation), out descriptor);

        /// <summary>
        /// 尝试得到服务描述符。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="serviceType">给定的服务类型。</param>
        /// <param name="descriptor">输出 <see cref="ServiceDescriptor"/>。</param>
        /// <returns>返回是否成功获取的布尔值。</returns>
        public static bool TryGet(this IServiceCollection services, Type serviceType, out ServiceDescriptor descriptor)
            => services.TryGet(serviceType, implementationType: null, out descriptor);

        /// <summary>
        /// 尝试获取指定类型的服务描述符。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="serviceType">给定的服务类型。</param>
        /// <param name="implementationType">给定的实现类型。</param>
        /// <param name="descriptor">输出 <see cref="ServiceDescriptor"/>。</param>
        /// <returns>返回是否成功获取的布尔值。</returns>
        public static bool TryGet(this IServiceCollection services, Type serviceType, Type implementationType,
            out ServiceDescriptor descriptor)
        {
            Func<ServiceDescriptor, bool> predicate = null;

            if (implementationType.IsNull())
                predicate = p => p.ServiceType == serviceType;
            else
                predicate = p => p.ServiceType == serviceType && p.ImplementationType == implementationType;

            descriptor = services.SingleOrDefault(predicate);
            return descriptor.IsNotNull();
        }

        /// <summary>
        /// 尝试获取指定类型的服务描述符。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="serviceType">给定的服务类型。</param>
        /// <param name="predicate">给定的断定工厂方法。</param>
        /// <param name="descriptor">输出 <see cref="ServiceDescriptor"/>。</param>
        /// <returns>返回是否成功获取的布尔值。</returns>
        public static bool TryGet(this IServiceCollection services, Type serviceType, Func<ServiceDescriptor, bool> predicate,
            out ServiceDescriptor descriptor)
        {
            descriptor = services.Where(p => p.ServiceType == serviceType).SingleOrDefault(predicate);
            return descriptor.IsNotNull();
        }

        #endregion


        #region TryReplace

        /// <summary>
        /// 尝试替换服务描述符。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="throwIfNotFound">未找到服务类型时抛出异常（可选；默认启用）。</param>
        /// <returns>返回是否成功替换的布尔值。</returns>
        public static bool TryReplace<TService>(this IServiceCollection services, bool throwIfNotFound = true)
            where TService : class
            => services.TryReplace(typeof(TService), typeof(TService), out _, throwIfNotFound);

        /// <summary>
        /// 尝试替换服务描述符。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <typeparam name="TNewImplementation">指定的新实现类型。</typeparam>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="throwIfNotFound">未找到服务类型时抛出异常（可选；默认启用）。</param>
        /// <returns>返回是否成功替换的布尔值。</returns>
        public static bool TryReplace<TService, TNewImplementation>(this IServiceCollection services, bool throwIfNotFound = true)
            where TService : class
            where TNewImplementation : class, TService
            => services.TryReplace(typeof(TService), typeof(TNewImplementation), out _, throwIfNotFound);

        /// <summary>
        /// 尝试替换服务描述符。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <typeparam name="TOldImplementation">指定的旧实现类型。</typeparam>
        /// <typeparam name="TNewImplementation">指定的新实现类型。</typeparam>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="throwIfNotFound">未找到服务类型时抛出异常（可选；默认启用）。</param>
        /// <returns>返回是否成功替换的布尔值。</returns>
        public static bool TryReplace<TService, TOldImplementation, TNewImplementation>(this IServiceCollection services, bool throwIfNotFound = true)
            where TService : class
            where TOldImplementation : class, TService
            where TNewImplementation : class, TService
            => services.TryReplace(typeof(TService), typeof(TOldImplementation), typeof(TNewImplementation), out _, throwIfNotFound);


        /// <summary>
        /// 尝试替换服务描述符。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="serviceType">给定的服务类型。</param>
        /// <param name="newImplementationType">给定用于替换的新实现类型。</param>
        /// <param name="throwIfNotFound">未找到服务类型时抛出异常（可选；默认启用）。</param>
        /// <returns>返回是否成功替换的布尔值。</returns>
        public static bool TryReplace(this IServiceCollection services, Type serviceType, Type newImplementationType, bool throwIfNotFound = true)
            => services.TryReplace(serviceType, oldImplementationType: null, newImplementationType, out _, throwIfNotFound);

        /// <summary>
        /// 尝试替换服务描述符。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="serviceType">给定的服务类型。</param>
        /// <param name="oldImplementationType">给定的旧实现类型。</param>
        /// <param name="newImplementationType">给定用于替换的新实现类型。</param>
        /// <param name="throwIfNotFound">未找到服务类型时抛出异常（可选；默认启用）。</param>
        /// <returns>返回是否成功替换的布尔值。</returns>
        public static bool TryReplace(this IServiceCollection services, Type serviceType, Type oldImplementationType, Type newImplementationType,
            bool throwIfNotFound = true)
            => services.TryReplace(serviceType, oldImplementationType, newImplementationType, out _, throwIfNotFound);


        /// <summary>
        /// 尝试替换服务描述符。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="oldDescriptor">输出旧 <see cref="ServiceDescriptor"/>。</param>
        /// <param name="throwIfNotFound">未找到服务类型时抛出异常（可选；默认启用）。</param>
        /// <returns>返回是否成功替换的布尔值。</returns>
        public static bool TryReplace<TService>(this IServiceCollection services, out ServiceDescriptor oldDescriptor, bool throwIfNotFound = true)
            where TService : class
            => services.TryReplace(typeof(TService), typeof(TService), out oldDescriptor, throwIfNotFound);

        /// <summary>
        /// 尝试替换服务描述符。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <typeparam name="TNewImplementation">指定的新实现类型。</typeparam>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="oldDescriptor">输出旧 <see cref="ServiceDescriptor"/>。</param>
        /// <param name="throwIfNotFound">未找到服务类型时抛出异常（可选；默认启用）。</param>
        /// <returns>返回是否成功替换的布尔值。</returns>
        public static bool TryReplace<TService, TNewImplementation>(this IServiceCollection services,
            out ServiceDescriptor oldDescriptor, bool throwIfNotFound = true)
            where TService : class
            where TNewImplementation : class, TService
            => services.TryReplace(typeof(TService), typeof(TNewImplementation), out oldDescriptor, throwIfNotFound);

        /// <summary>
        /// 尝试替换服务描述符。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <typeparam name="TOldImplementation">指定的旧实现类型。</typeparam>
        /// <typeparam name="TNewImplementation">指定的新实现类型。</typeparam>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="oldDescriptor">输出旧 <see cref="ServiceDescriptor"/>。</param>
        /// <param name="throwIfNotFound">未找到服务类型时抛出异常（可选；默认启用）。</param>
        /// <returns>返回是否成功替换的布尔值。</returns>
        public static bool TryReplace<TService, TOldImplementation, TNewImplementation>(this IServiceCollection services,
            out ServiceDescriptor oldDescriptor, bool throwIfNotFound = true)
            where TService : class
            where TOldImplementation : class, TService
            where TNewImplementation : class, TService
            => services.TryReplace(typeof(TService), typeof(TOldImplementation), typeof(TNewImplementation), out oldDescriptor, throwIfNotFound);


        /// <summary>
        /// 尝试替换服务描述符。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="serviceType">给定的服务类型。</param>
        /// <param name="newImplementationType">给定用于替换的新实现类型。</param>
        /// <param name="oldDescriptor">输出旧 <see cref="ServiceDescriptor"/>。</param>
        /// <param name="throwIfNotFound">未找到服务类型时抛出异常（可选；默认启用）。</param>
        /// <returns>返回是否成功替换的布尔值。</returns>
        public static bool TryReplace(this IServiceCollection services, Type serviceType, Type newImplementationType,
            out ServiceDescriptor oldDescriptor, bool throwIfNotFound = true)
            => services.TryReplace(serviceType, oldImplementationType: null, newImplementationType, out oldDescriptor, throwIfNotFound);

        /// <summary>
        /// 尝试替换服务描述符。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="serviceType">给定的服务类型。</param>
        /// <param name="oldImplementationType">给定的旧实现类型。</param>
        /// <param name="newImplementationType">给定用于替换的新实现类型。</param>
        /// <param name="oldDescriptor">输出旧 <see cref="ServiceDescriptor"/>。</param>
        /// <param name="throwIfNotFound">未找到服务类型时抛出异常（可选；默认启用）。</param>
        /// <returns>返回是否成功替换的布尔值。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static bool TryReplace(this IServiceCollection services, Type serviceType, Type oldImplementationType, Type newImplementationType,
            out ServiceDescriptor oldDescriptor, bool throwIfNotFound = true)
        {
            if (services.TryGet(serviceType, oldImplementationType, out oldDescriptor))
            {
                services.Remove(oldDescriptor);
                services.Add(ServiceDescriptor.Describe(serviceType, newImplementationType, oldDescriptor.Lifetime));
                return true;
            }

            if (throwIfNotFound)
            {
                if (oldImplementationType.IsNotNull())
                    throw new ArgumentException($"No service type '{serviceType.FullName}' and old implementation type '{oldImplementationType.FullName}' were found.");
                else
                    throw new ArgumentException($"No service type '{serviceType.FullName}' were found.");
            }

            return false;
        }


        /// <summary>
        /// 尝试替换服务描述符。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="predicate">给定的断定工厂方法。</param>
        /// <param name="newDescriptorFactory">给定用于替换的新 <see cref="ServiceDescriptor"/> 工厂方法。</param>
        /// <param name="throwIfNotFound">未找到服务类型时抛出异常（可选；默认启用）。</param>
        /// <returns>返回是否成功替换的布尔值。</returns>
        public static bool TryReplace<TService>(this IServiceCollection services, Func<ServiceDescriptor, bool> predicate,
            Func<ServiceDescriptor, ServiceDescriptor> newDescriptorFactory, bool throwIfNotFound = true)
            => services.TryReplace(typeof(TService), predicate, newDescriptorFactory, throwIfNotFound);

        /// <summary>
        /// 尝试替换服务描述符。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="serviceType">给定的服务类型。</param>
        /// <param name="predicate">给定的断定工厂方法。</param>
        /// <param name="newDescriptorFactory">给定用于替换的新 <see cref="ServiceDescriptor"/> 工厂方法。</param>
        /// <param name="throwIfNotFound">未找到服务类型时抛出异常（可选；默认启用）。</param>
        /// <returns>返回是否成功替换的布尔值。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static bool TryReplace(this IServiceCollection services, Type serviceType, Func<ServiceDescriptor, bool> predicate,
            Func<ServiceDescriptor, ServiceDescriptor> newDescriptorFactory, bool throwIfNotFound = true)
        {
            newDescriptorFactory.NotNull(nameof(newDescriptorFactory));

            if (services.TryGet(serviceType, predicate, out ServiceDescriptor oldDescriptor))
            {
                var newDescriptor = newDescriptorFactory.Invoke(oldDescriptor);
                services.Remove(oldDescriptor);
                services.Add(newDescriptor);
                return true;
            }

            if (throwIfNotFound)
                throw new ArgumentException($"No service type '{serviceType.FullName}' were found.");

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
        /// <param name="throwIfNotFound">未找到服务类型时抛出异常（可选；默认启用）。</param>
        /// <returns>返回是否成功替换的布尔值。</returns>
        public static bool TryReplace<TService>(this IServiceCollection services,
            Func<IServiceProvider, TService> implementationFactory, bool throwIfNotFound = true)
            where TService : class
            => services.TryReplace(typeof(TService), implementationFactory, out _, throwIfNotFound);

        /// <summary>
        /// 尝试替换服务描述符。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <typeparam name="TNewImplementation">指定的新实现类型。</typeparam>
        /// <param name="newImplementationFactory">给定用于替换的新实现类型工厂方法。</param>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="throwIfNotFound">未找到服务类型时抛出异常（可选；默认启用）。</param>
        /// <returns>返回是否成功替换的布尔值。</returns>
        public static bool TryReplace<TService, TNewImplementation>(this IServiceCollection services,
            Func<IServiceProvider, TNewImplementation> newImplementationFactory, bool throwIfNotFound = true)
            where TService : class
            where TNewImplementation : class, TService
            => services.TryReplace(typeof(TService), newImplementationFactory, out _, throwIfNotFound);

        /// <summary>
        /// 尝试替换服务描述符。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <typeparam name="TOldImplementation">指定的旧实现类型。</typeparam>
        /// <typeparam name="TNewImplementation">指定的新实现类型。</typeparam>
        /// <param name="newImplementationFactory">给定用于替换的新实现类型工厂方法。</param>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="throwIfNotFound">未找到服务类型时抛出异常（可选；默认启用）。</param>
        /// <returns>返回是否成功替换的布尔值。</returns>
        public static bool TryReplace<TService, TOldImplementation, TNewImplementation>(this IServiceCollection services,
            Func<IServiceProvider, TNewImplementation> newImplementationFactory, bool throwIfNotFound = true)
            where TService : class
            where TOldImplementation : class, TService
            where TNewImplementation : class, TService
            => services.TryReplace(typeof(TService), typeof(TOldImplementation), newImplementationFactory, out _, throwIfNotFound);


        /// <summary>
        /// 尝试替换服务描述符。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="serviceType">给定的服务类型。</param>
        /// <param name="newImplementationFactory">给定用于替换的新实现类型工厂方法。</param>
        /// <param name="throwIfNotFound">未找到服务类型时抛出异常（可选；默认启用）。</param>
        /// <returns>返回是否成功替换的布尔值。</returns>
        public static bool TryReplace(this IServiceCollection services, Type serviceType,
            Func<IServiceProvider, object> newImplementationFactory, bool throwIfNotFound = true)
            => services.TryReplace(serviceType, oldImplementationType: null, newImplementationFactory, out _, throwIfNotFound);

        /// <summary>
        /// 尝试替换服务描述符。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="serviceType">给定的服务类型。</param>
        /// <param name="oldImplementationType">给定的旧实现类型。</param>
        /// <param name="newImplementationFactory">给定用于替换的新实现类型工厂方法。</param>
        /// <param name="throwIfNotFound">未找到服务类型时抛出异常（可选；默认启用）。</param>
        /// <returns>返回是否成功替换的布尔值。</returns>
        public static bool TryReplace(this IServiceCollection services, Type serviceType, Type oldImplementationType,
            Func<IServiceProvider, object> newImplementationFactory, bool throwIfNotFound = true)
            => services.TryReplace(serviceType, oldImplementationType, newImplementationFactory, out _, throwIfNotFound);


        /// <summary>
        /// 尝试替换服务描述符。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="implementationFactory">给定用于替换的实现类型工厂方法。</param>
        /// <param name="oldDescriptor">输出旧 <see cref="ServiceDescriptor"/>。</param>
        /// <param name="throwIfNotFound">未找到服务类型时抛出异常（可选；默认启用）。</param>
        /// <returns>返回是否成功替换的布尔值。</returns>
        public static bool TryReplace<TService>(this IServiceCollection services,
            Func<IServiceProvider, TService> implementationFactory, out ServiceDescriptor oldDescriptor, bool throwIfNotFound = true)
            where TService : class
            => services.TryReplace(typeof(TService), implementationFactory, out oldDescriptor, throwIfNotFound);

        /// <summary>
        /// 尝试替换服务描述符。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <typeparam name="TNewImplementation">指定的新实现类型。</typeparam>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="newImplementationFactory">给定用于替换的新实现类型工厂方法。</param>
        /// <param name="oldDescriptor">输出旧 <see cref="ServiceDescriptor"/>。</param>
        /// <param name="throwIfNotFound">未找到服务类型时抛出异常（可选；默认启用）。</param>
        /// <returns>返回是否成功替换的布尔值。</returns>
        public static bool TryReplace<TService, TNewImplementation>(this IServiceCollection services,
            Func<IServiceProvider, TNewImplementation> newImplementationFactory, out ServiceDescriptor oldDescriptor, bool throwIfNotFound = true)
            where TService : class
            where TNewImplementation : class, TService
            => services.TryReplace(typeof(TService), newImplementationFactory, out oldDescriptor, throwIfNotFound);

        /// <summary>
        /// 尝试替换服务描述符。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <typeparam name="TOldImplementation">指定的旧实现类型。</typeparam>
        /// <typeparam name="TNewImplementation">指定的新实现类型。</typeparam>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="newImplementationFactory">给定用于替换的新实现类型工厂方法。</param>
        /// <param name="oldDescriptor">输出旧 <see cref="ServiceDescriptor"/>。</param>
        /// <param name="throwIfNotFound">未找到服务类型时抛出异常（可选；默认启用）。</param>
        /// <returns>返回是否成功替换的布尔值。</returns>
        public static bool TryReplace<TService, TOldImplementation, TNewImplementation>(this IServiceCollection services,
            Func<IServiceProvider, TNewImplementation> newImplementationFactory, out ServiceDescriptor oldDescriptor, bool throwIfNotFound = true)
            where TService : class
            where TOldImplementation : class, TService
            where TNewImplementation : class, TService
            => services.TryReplace(typeof(TService), typeof(TOldImplementation), newImplementationFactory, out oldDescriptor, throwIfNotFound);


        /// <summary>
        /// 尝试替换服务描述符。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="serviceType">给定的服务类型。</param>
        /// <param name="newImplementationFactory">给定用于替换的新实现类型工厂方法。</param>
        /// <param name="oldDescriptor">输出旧 <see cref="ServiceDescriptor"/>。</param>
        /// <param name="throwIfNotFound">未找到服务类型时抛出异常（可选；默认启用）。</param>
        /// <returns>返回是否成功替换的布尔值。</returns>
        public static bool TryReplace(this IServiceCollection services, Type serviceType,
            Func<IServiceProvider, object> newImplementationFactory, out ServiceDescriptor oldDescriptor, bool throwIfNotFound = true)
            => services.TryReplace(serviceType, oldImplementationType: null, newImplementationFactory, out oldDescriptor, throwIfNotFound);

        /// <summary>
        /// 尝试替换服务描述符。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="serviceType">给定的服务类型。</param>
        /// <param name="oldImplementationType">给定的旧实现类型。</param>
        /// <param name="newImplementationFactory">给定用于替换的新实现类型工厂方法。</param>
        /// <param name="oldDescriptor">输出旧 <see cref="ServiceDescriptor"/>。</param>
        /// <param name="throwIfNotFound">未找到服务类型时抛出异常（可选；默认启用）。</param>
        /// <returns>返回是否成功替换的布尔值。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static bool TryReplace(this IServiceCollection services, Type serviceType, Type oldImplementationType,
            Func<IServiceProvider, object> newImplementationFactory, out ServiceDescriptor oldDescriptor, bool throwIfNotFound = true)
        {
            if (services.TryGet(serviceType, oldImplementationType, out oldDescriptor))
            {
                services.Remove(oldDescriptor);
                services.Add(ServiceDescriptor.Describe(serviceType, newImplementationFactory, oldDescriptor.Lifetime));
                return true;
            }

            if (throwIfNotFound)
            {
                if (oldImplementationType.IsNotNull())
                    throw new ArgumentException($"No service type '{serviceType.FullName}' and old implementation type '{oldImplementationType.FullName}' were found.");
                else
                    throw new ArgumentException($"No service type '{serviceType.FullName}' were found.");
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
        /// <param name="throwIfNotFound">未找到服务类型时抛出异常（可选；默认启用）。</param>
        /// <returns>返回是否成功替换的布尔值。</returns>
        public static bool TryReplace<TService>(this IServiceCollection services, TService newInstance, bool throwIfNotFound = true)
            => services.TryReplace(typeof(TService), newInstance, out _, throwIfNotFound);

        /// <summary>
        /// 尝试替换服务描述符。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <typeparam name="TNewImplementation">指定的新实现类型。</typeparam>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="newInstance">给定的新实例。</param>
        /// <param name="throwIfNotFound">未找到服务类型时抛出异常（可选；默认启用）。</param>
        /// <returns>返回是否成功替换的布尔值。</returns>
        public static bool TryReplace<TService, TNewImplementation>(this IServiceCollection services, TNewImplementation newInstance,
            bool throwIfNotFound = true)
            => services.TryReplace(typeof(TService), newInstance, out _, throwIfNotFound);

        /// <summary>
        /// 尝试替换服务描述符。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <typeparam name="TOldImplementation">指定的旧实现类型。</typeparam>
        /// <typeparam name="TNewImplementation">指定的新实现类型。</typeparam>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="newInstance">给定的新实例。</param>
        /// <param name="throwIfNotFound">未找到服务类型时抛出异常（可选；默认启用）。</param>
        /// <returns>返回是否成功替换的布尔值。</returns>
        public static bool TryReplace<TService, TOldImplementation, TNewImplementation>(this IServiceCollection services,
            TNewImplementation newInstance, bool throwIfNotFound = true)
            where TService : class
            where TOldImplementation : class, TService
            where TNewImplementation : class, TService
            => services.TryReplace(typeof(TService), typeof(TOldImplementation), newInstance, out _, throwIfNotFound);


        /// <summary>
        /// 尝试替换服务描述符。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="serviceType">给定的服务类型。</param>
        /// <param name="newInstance">给定的新实例。</param>
        /// <param name="throwIfNotFound">未找到服务类型时抛出异常（可选；默认启用）。</param>
        /// <returns>返回是否成功替换的布尔值。</returns>
        public static bool TryReplace(this IServiceCollection services, Type serviceType, object newInstance, bool throwIfNotFound = true)
            => services.TryReplace(serviceType, oldImplementationType: null, newInstance, out _, throwIfNotFound);

        /// <summary>
        /// 尝试替换服务描述符。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="serviceType">给定的服务类型。</param>
        /// <param name="oldImplementationType">给定的旧实现类型。</param>
        /// <param name="newInstance">给定的新实例。</param>
        /// <param name="throwIfNotFound">未找到服务类型时抛出异常（可选；默认启用）。</param>
        /// <returns>返回是否成功替换的布尔值。</returns>
        public static bool TryReplace(this IServiceCollection services, Type serviceType, Type oldImplementationType, object newInstance,
            bool throwIfNotFound = true)
            => services.TryReplace(serviceType, oldImplementationType, newInstance, out _, throwIfNotFound);


        /// <summary>
        /// 尝试替换服务描述符。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="newInstance">给定的新实例。</param>
        /// <param name="oldDescriptor">输出旧 <see cref="ServiceDescriptor"/>。</param>
        /// <param name="throwIfNotFound">未找到服务类型时抛出异常（可选；默认启用）。</param>
        /// <returns>返回是否成功替换的布尔值。</returns>
        public static bool TryReplace<TService>(this IServiceCollection services, TService newInstance, out ServiceDescriptor oldDescriptor,
            bool throwIfNotFound = true)
            => services.TryReplace(typeof(TService), newInstance, out oldDescriptor, throwIfNotFound);

        /// <summary>
        /// 尝试替换服务描述符。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <typeparam name="TNewImplementation">指定的新实现类型。</typeparam>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="newInstance">给定的新实例。</param>
        /// <param name="oldDescriptor">输出旧 <see cref="ServiceDescriptor"/>。</param>
        /// <param name="throwIfNotFound">未找到服务类型时抛出异常（可选；默认启用）。</param>
        /// <returns>返回是否成功替换的布尔值。</returns>
        public static bool TryReplace<TService, TNewImplementation>(this IServiceCollection services,
            TNewImplementation newInstance, out ServiceDescriptor oldDescriptor, bool throwIfNotFound = true)
            => services.TryReplace(typeof(TService), newInstance, out oldDescriptor, throwIfNotFound);

        /// <summary>
        /// 尝试替换服务描述符。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <typeparam name="TOldImplementation">指定的旧实现类型。</typeparam>
        /// <typeparam name="TNewImplementation">指定的新实现类型。</typeparam>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="newInstance">给定的新实例。</param>
        /// <param name="oldDescriptor">输出旧 <see cref="ServiceDescriptor"/>。</param>
        /// <param name="throwIfNotFound">未找到服务类型时抛出异常（可选；默认启用）。</param>
        /// <returns>返回是否成功替换的布尔值。</returns>
        public static bool TryReplace<TService, TOldImplementation, TNewImplementation>(this IServiceCollection services,
            TNewImplementation newInstance, out ServiceDescriptor oldDescriptor, bool throwIfNotFound = true)
            where TService : class
            where TOldImplementation : class, TService
            where TNewImplementation : class, TService
            => services.TryReplace(typeof(TService), typeof(TOldImplementation), newInstance, out oldDescriptor, throwIfNotFound);


        /// <summary>
        /// 尝试替换服务描述符。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="serviceType">给定的服务类型。</param>
        /// <param name="newInstance">给定的新实例。</param>
        /// <param name="oldDescriptor">输出旧 <see cref="ServiceDescriptor"/>。</param>
        /// <param name="throwIfNotFound">未找到服务类型时抛出异常（可选；默认启用）。</param>
        /// <returns>返回是否成功替换的布尔值。</returns>
        public static bool TryReplace(this IServiceCollection services, Type serviceType, object newInstance,
            out ServiceDescriptor oldDescriptor, bool throwIfNotFound = true)
            => services.TryReplace(serviceType, oldImplementationType: null, newInstance, out oldDescriptor, throwIfNotFound);

        /// <summary>
        /// 尝试替换服务描述符。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="serviceType">给定的服务类型。</param>
        /// <param name="oldImplementationType">给定的旧实现类型。</param>
        /// <param name="newInstance">给定的新实例。</param>
        /// <param name="oldDescriptor">输出旧 <see cref="ServiceDescriptor"/>。</param>
        /// <param name="throwIfNotFound">未找到服务类型时抛出异常（可选；默认启用）。</param>
        /// <returns>返回是否成功替换的布尔值。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static bool TryReplace(this IServiceCollection services, Type serviceType, Type oldImplementationType,
            object newInstance, out ServiceDescriptor oldDescriptor, bool throwIfNotFound = true)
        {
            if (services.TryGet(serviceType, oldImplementationType, out oldDescriptor))
            {
                services.Remove(oldDescriptor);
                services.Add(new ServiceDescriptor(serviceType, newInstance));
                return true;
            }

            if (throwIfNotFound)
            {
                if (oldImplementationType.IsNotNull())
                    throw new ArgumentException($"No service type '{serviceType.FullName}' and old implementation type '{oldImplementationType.FullName}' were found.");
                else
                    throw new ArgumentException($"No service type '{serviceType.FullName}' were found.");
            }

            return false;
        }

        #endregion

    }
}
