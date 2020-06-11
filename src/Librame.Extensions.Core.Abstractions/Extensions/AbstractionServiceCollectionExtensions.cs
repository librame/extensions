#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
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
        /// 尝试获取指定类型的所有服务描述符。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="descriptors">输出 <see cref="IReadOnlyList{ServiceDescriptor}"/>。</param>
        /// <returns>返回是否成功获取的布尔值。</returns>
        public static bool TryGetAll<TService>(this IServiceCollection services,
            out IReadOnlyList<ServiceDescriptor> descriptors)
            where TService : class
            => services.TryGetAll(typeof(TService), out descriptors);

        /// <summary>
        /// 尝试获取指定类型的所有服务描述符。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <typeparam name="TImplementation">指定的实现类型。</typeparam>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="descriptors">输出 <see cref="IReadOnlyList{ServiceDescriptor}"/>。</param>
        /// <returns>返回是否成功获取的布尔值。</returns>
        public static bool TryGetAll<TService, TImplementation>(this IServiceCollection services,
            out IReadOnlyList<ServiceDescriptor> descriptors)
            where TService : class
            where TImplementation : class, TService
            => services.TryGetAll(typeof(TService), out descriptors, typeof(TImplementation));

        /// <summary>
        /// 尝试获取指定类型的所有服务描述符。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="serviceType">给定的服务类型。</param>
        /// <param name="descriptors">输出 <see cref="IReadOnlyList{ServiceDescriptor}"/>。</param>
        /// <param name="implementationType">给定的实现类型（可选）。</param>
        /// <returns>返回是否成功获取的布尔值。</returns>
        public static bool TryGetAll(this IServiceCollection services, Type serviceType,
            out IReadOnlyList<ServiceDescriptor> descriptors, Type implementationType = null)
        {
            // 存在多个相同服务与实现类型的服务集合
            descriptors = services.Where(GetPredicateDescriptor(serviceType, implementationType))
                .AsReadOnlyList();

            return descriptors.Count > 0;
        }

        private static Func<ServiceDescriptor, bool> GetPredicateDescriptor(Type serviceType,
            Type implementationType = null)
        {
            if (implementationType.IsNull())
                return p => p.ServiceType == serviceType;
            else
                return p => p.ServiceType == serviceType && p.ImplementationType == implementationType;
        }


        /// <summary>
        /// 尝试获取指定类型的服务描述符。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="predicate">给定的断定工厂方法。</param>
        /// <param name="descriptor">输出 <see cref="ServiceDescriptor"/>。</param>
        /// <returns>返回是否成功获取的布尔值。</returns>
        public static bool TryGetSingle<TService>(this IServiceCollection services,
            Func<ServiceDescriptor, bool> predicate, out ServiceDescriptor descriptor)
            => services.TryGetSingle(typeof(TService), predicate, out descriptor);

        /// <summary>
        /// 尝试获取指定类型的服务描述符。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="serviceType">给定的服务类型。</param>
        /// <param name="predicate">给定的断定工厂方法。</param>
        /// <param name="descriptor">输出 <see cref="ServiceDescriptor"/>。</param>
        /// <returns>返回是否成功获取的布尔值。</returns>
        public static bool TryGetSingle(this IServiceCollection services, Type serviceType,
            Func<ServiceDescriptor, bool> predicate, out ServiceDescriptor descriptor)
        {
            // 存在多个相同服务与实现类型的服务集合
            descriptor = services.Where(p => p.ServiceType == serviceType)
                .SingleOrDefault(predicate);

            return descriptor.IsNotNull();
        }

        #endregion


        #region TryReplace.ImplementationType

        /// <summary>
        /// 尝试替换所有服务描述符。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="throwIfNotFound">未找到服务类型时抛出异常（可选；默认启用）。</param>
        /// <returns>返回是否成功替换的布尔值。</returns>
        public static bool TryReplaceAll<TService>(this IServiceCollection services,
            bool throwIfNotFound = true)
            where TService : class
            => services.TryReplaceAll<TService>(out _, throwIfNotFound);

        /// <summary>
        /// 尝试替换所有服务描述符。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <typeparam name="TNewImplementation">指定的新实现类型。</typeparam>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="throwIfNotFound">未找到服务类型时抛出异常（可选；默认启用）。</param>
        /// <returns>返回是否成功替换的布尔值。</returns>
        public static bool TryReplaceAll<TService, TNewImplementation>(this IServiceCollection services,
            bool throwIfNotFound = true)
            where TService : class
            where TNewImplementation : class, TService
            => services.TryReplaceAll<TService, TNewImplementation>(out _, throwIfNotFound);

        /// <summary>
        /// 尝试替换所有服务描述符。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <typeparam name="TOldImplementation">指定的旧实现类型。</typeparam>
        /// <typeparam name="TNewImplementation">指定的新实现类型。</typeparam>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="throwIfNotFound">未找到服务类型时抛出异常（可选；默认启用）。</param>
        /// <returns>返回是否成功替换的布尔值。</returns>
        public static bool TryReplaceAll<TService, TOldImplementation, TNewImplementation>(this IServiceCollection services,
            bool throwIfNotFound = true)
            where TService : class
            where TOldImplementation : class, TService
            where TNewImplementation : class, TService
            => services.TryReplaceAll<TService, TOldImplementation, TNewImplementation>(out _, throwIfNotFound);

        /// <summary>
        /// 尝试替换所有服务描述符。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="serviceType">给定的服务类型。</param>
        /// <param name="newImplementationType">给定用于替换的新实现类型。</param>
        /// <param name="oldImplementationType">给定的旧实现类型（可选）。</param>
        /// <param name="throwIfNotFound">未找到服务类型时抛出异常（可选；默认启用）。</param>
        /// <returns>返回是否成功替换的布尔值。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static bool TryReplaceAll(this IServiceCollection services, Type serviceType,
            Type newImplementationType, Type oldImplementationType = null, bool throwIfNotFound = true)
            => services.TryReplaceAll(serviceType, newImplementationType, out _, oldImplementationType, throwIfNotFound);


        /// <summary>
        /// 尝试替换所有服务描述符。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="oldDescriptors">输出旧 <see cref="IReadOnlyList{ServiceDescriptor}"/>。</param>
        /// <param name="throwIfNotFound">未找到服务类型时抛出异常（可选；默认启用）。</param>
        /// <returns>返回是否成功替换的布尔值。</returns>
        public static bool TryReplaceAll<TService>(this IServiceCollection services,
            out IReadOnlyList<ServiceDescriptor> oldDescriptors, bool throwIfNotFound = true)
            where TService : class
            => services.TryReplaceAll<TService, TService>(out oldDescriptors, throwIfNotFound);

        /// <summary>
        /// 尝试替换所有服务描述符。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <typeparam name="TNewImplementation">指定的新实现类型。</typeparam>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="oldDescriptors">输出旧 <see cref="IReadOnlyList{ServiceDescriptor}"/>。</param>
        /// <param name="throwIfNotFound">未找到服务类型时抛出异常（可选；默认启用）。</param>
        /// <returns>返回是否成功替换的布尔值。</returns>
        public static bool TryReplaceAll<TService, TNewImplementation>(this IServiceCollection services,
            out IReadOnlyList<ServiceDescriptor> oldDescriptors, bool throwIfNotFound = true)
            where TService : class
            where TNewImplementation : class, TService
            => services.TryReplaceAll(typeof(TService), typeof(TNewImplementation), out oldDescriptors,
                oldImplementationType: null, throwIfNotFound);

        /// <summary>
        /// 尝试替换所有服务描述符。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <typeparam name="TOldImplementation">指定的旧实现类型。</typeparam>
        /// <typeparam name="TNewImplementation">指定的新实现类型。</typeparam>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="oldDescriptors">输出旧 <see cref="IReadOnlyList{ServiceDescriptor}"/>。</param>
        /// <param name="throwIfNotFound">未找到服务类型时抛出异常（可选；默认启用）。</param>
        /// <returns>返回是否成功替换的布尔值。</returns>
        public static bool TryReplaceAll<TService, TOldImplementation, TNewImplementation>(this IServiceCollection services,
            out IReadOnlyList<ServiceDescriptor> oldDescriptors, bool throwIfNotFound = true)
            where TService : class
            where TOldImplementation : class, TService
            where TNewImplementation : class, TService
            => services.TryReplaceAll(typeof(TService), typeof(TNewImplementation), out oldDescriptors,
                typeof(TOldImplementation), throwIfNotFound);

        /// <summary>
        /// 尝试替换所有服务描述符。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="serviceType">给定的服务类型。</param>
        /// <param name="newImplementationType">给定用于替换的新实现类型。</param>
        /// <param name="oldDescriptors">输出旧 <see cref="IReadOnlyList{ServiceDescriptor}"/>。</param>
        /// <param name="oldImplementationType">给定的旧实现类型（可选）。</param>
        /// <param name="throwIfNotFound">未找到服务类型时抛出异常（可选；默认启用）。</param>
        /// <returns>返回是否成功替换的布尔值。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static bool TryReplaceAll(this IServiceCollection services, Type serviceType,
            Type newImplementationType, out IReadOnlyList<ServiceDescriptor> oldDescriptors,
            Type oldImplementationType = null, bool throwIfNotFound = true)
        {
            if (services.TryGetAll(serviceType, out oldDescriptors, oldImplementationType))
            {
                var lifetime = oldDescriptors[0].Lifetime;

                // 移除找到的所有描述符
                oldDescriptors.ForEach(descriptor => services.Remove(descriptor));
                
                // 添加新描述符
                services.Add(ServiceDescriptor.Describe(serviceType, newImplementationType, lifetime));
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
        /// 尝试替换单个服务描述符。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="predicate">给定的断定工厂方法。</param>
        /// <param name="newDescriptorFactory">给定用于替换的新 <see cref="ServiceDescriptor"/> 工厂方法。</param>
        /// <param name="throwIfNotFound">未找到服务类型时抛出异常（可选；默认启用）。</param>
        /// <returns>返回是否成功替换的布尔值。</returns>
        public static bool TryReplaceSingle<TService>(this IServiceCollection services,
            Func<ServiceDescriptor, bool> predicate,
            Func<ServiceDescriptor, ServiceDescriptor> newDescriptorFactory, bool throwIfNotFound = true)
            => services.TryReplaceSingle(typeof(TService), predicate, newDescriptorFactory, throwIfNotFound);

        /// <summary>
        /// 尝试替换单个服务描述符。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="serviceType">给定的服务类型。</param>
        /// <param name="predicate">给定的断定工厂方法。</param>
        /// <param name="newDescriptorFactory">给定用于替换的新 <see cref="ServiceDescriptor"/> 工厂方法。</param>
        /// <param name="throwIfNotFound">未找到服务类型时抛出异常（可选；默认启用）。</param>
        /// <returns>返回是否成功替换的布尔值。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static bool TryReplaceSingle(this IServiceCollection services, Type serviceType,
            Func<ServiceDescriptor, bool> predicate,
            Func<ServiceDescriptor, ServiceDescriptor> newDescriptorFactory, bool throwIfNotFound = true)
        {
            newDescriptorFactory.NotNull(nameof(newDescriptorFactory));

            if (services.TryGetSingle(serviceType, predicate, out ServiceDescriptor oldDescriptor))
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


        #region TryReplace.ImplementationFactory

        /// <summary>
        /// 尝试替换服务描述符。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="newImplementationFactory">给定用于替换的新实现类型工厂方法。</param>
        /// <param name="throwIfNotFound">未找到服务类型时抛出异常（可选；默认启用）。</param>
        /// <returns>返回是否成功替换的布尔值。</returns>
        public static bool TryReplaceAll<TService>(this IServiceCollection services,
            Func<IServiceProvider, TService> newImplementationFactory, bool throwIfNotFound = true)
            where TService : class
            => services.TryReplaceAll<TService>(newImplementationFactory, out _, throwIfNotFound);

        /// <summary>
        /// 尝试替换服务描述符。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <typeparam name="TNewImplementation">指定的新实现类型。</typeparam>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="newImplementationFactory">给定用于替换的新实现类型工厂方法。</param>
        /// <param name="throwIfNotFound">未找到服务类型时抛出异常（可选；默认启用）。</param>
        /// <returns>返回是否成功替换的布尔值。</returns>
        public static bool TryReplaceAll<TService, TNewImplementation>(this IServiceCollection services,
            Func<IServiceProvider, TNewImplementation> newImplementationFactory, bool throwIfNotFound = true)
            where TService : class
            where TNewImplementation : class, TService
            => services.TryReplaceAll<TService, TNewImplementation>(newImplementationFactory,
                out _, throwIfNotFound);

        /// <summary>
        /// 尝试替换服务描述符。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <typeparam name="TOldImplementation">指定的旧实现类型。</typeparam>
        /// <typeparam name="TNewImplementation">指定的新实现类型。</typeparam>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="newImplementationFactory">给定用于替换的新实现类型工厂方法。</param>
        /// <param name="throwIfNotFound">未找到服务类型时抛出异常（可选；默认启用）。</param>
        /// <returns>返回是否成功替换的布尔值。</returns>
        public static bool TryReplaceAll<TService, TOldImplementation, TNewImplementation>(this IServiceCollection services,
            Func<IServiceProvider, TNewImplementation> newImplementationFactory, bool throwIfNotFound = true)
            where TService : class
            where TOldImplementation : class, TService
            where TNewImplementation : class, TService
            => services.TryReplaceAll<TService, TOldImplementation, TNewImplementation>(newImplementationFactory,
                out _, throwIfNotFound);

        /// <summary>
        /// 尝试替换服务描述符。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="serviceType">给定的服务类型。</param>
        /// <param name="oldImplementationType">给定的旧实现类型。</param>
        /// <param name="newImplementationFactory">给定用于替换的新实现类型工厂方法。</param>
        /// <param name="throwIfNotFound">未找到服务类型时抛出异常（可选；默认启用）。</param>
        /// <returns>返回是否成功替换的布尔值。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static bool TryReplaceAll(this IServiceCollection services, Type serviceType,
            Func<IServiceProvider, object> newImplementationFactory,
            Type oldImplementationType = null, bool throwIfNotFound = true)
            => services.TryReplaceAll(serviceType, newImplementationFactory, out _,
                oldImplementationType, throwIfNotFound);


        /// <summary>
        /// 尝试替换服务描述符。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="newImplementationFactory">给定用于替换的新实现类型工厂方法。</param>
        /// <param name="oldDescriptors">输出旧 <see cref="IReadOnlyList{ServiceDescriptor}"/>。</param>
        /// <param name="throwIfNotFound">未找到服务类型时抛出异常（可选；默认启用）。</param>
        /// <returns>返回是否成功替换的布尔值。</returns>
        public static bool TryReplaceAll<TService>(this IServiceCollection services,
            Func<IServiceProvider, TService> newImplementationFactory,
            out IReadOnlyList<ServiceDescriptor> oldDescriptors, bool throwIfNotFound = true)
            where TService : class
            => services.TryReplaceAll<TService, TService>(newImplementationFactory, out oldDescriptors,
                throwIfNotFound);

        /// <summary>
        /// 尝试替换服务描述符。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <typeparam name="TNewImplementation">指定的新实现类型。</typeparam>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="newImplementationFactory">给定用于替换的新实现类型工厂方法。</param>
        /// <param name="oldDescriptors">输出旧 <see cref="IReadOnlyList{ServiceDescriptor}"/>。</param>
        /// <param name="throwIfNotFound">未找到服务类型时抛出异常（可选；默认启用）。</param>
        /// <returns>返回是否成功替换的布尔值。</returns>
        public static bool TryReplaceAll<TService, TNewImplementation>(this IServiceCollection services,
            Func<IServiceProvider, TNewImplementation> newImplementationFactory,
            out IReadOnlyList<ServiceDescriptor> oldDescriptors, bool throwIfNotFound = true)
            where TService : class
            where TNewImplementation : class, TService
            => services.TryReplaceAll(typeof(TService), newImplementationFactory, out oldDescriptors,
                oldImplementationType: null, throwIfNotFound);

        /// <summary>
        /// 尝试替换服务描述符。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <typeparam name="TOldImplementation">指定的旧实现类型。</typeparam>
        /// <typeparam name="TNewImplementation">指定的新实现类型。</typeparam>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="newImplementationFactory">给定用于替换的新实现类型工厂方法。</param>
        /// <param name="oldDescriptors">输出旧 <see cref="IReadOnlyList{ServiceDescriptor}"/>。</param>
        /// <param name="throwIfNotFound">未找到服务类型时抛出异常（可选；默认启用）。</param>
        /// <returns>返回是否成功替换的布尔值。</returns>
        public static bool TryReplaceAll<TService, TOldImplementation, TNewImplementation>(this IServiceCollection services,
            Func<IServiceProvider, TNewImplementation> newImplementationFactory,
            out IReadOnlyList<ServiceDescriptor> oldDescriptors, bool throwIfNotFound = true)
            where TService : class
            where TOldImplementation : class, TService
            where TNewImplementation : class, TService
            => services.TryReplaceAll(typeof(TService), newImplementationFactory, out oldDescriptors,
                typeof(TOldImplementation), throwIfNotFound);

        /// <summary>
        /// 尝试替换服务描述符。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="serviceType">给定的服务类型。</param>
        /// <param name="oldImplementationType">给定的旧实现类型。</param>
        /// <param name="newImplementationFactory">给定用于替换的新实现类型工厂方法。</param>
        /// <param name="oldDescriptors">输出旧 <see cref="IReadOnlyList{ServiceDescriptor}"/>。</param>
        /// <param name="throwIfNotFound">未找到服务类型时抛出异常（可选；默认启用）。</param>
        /// <returns>返回是否成功替换的布尔值。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static bool TryReplaceAll(this IServiceCollection services, Type serviceType,
            Func<IServiceProvider, object> newImplementationFactory,
            out IReadOnlyList<ServiceDescriptor> oldDescriptors,
            Type oldImplementationType = null, bool throwIfNotFound = true)
        {
            if (services.TryGetAll(serviceType, out oldDescriptors, oldImplementationType))
            {
                var lifetime = oldDescriptors[0].Lifetime;

                // 移除找到的所有描述符
                oldDescriptors.ForEach(descriptor => services.Remove(descriptor));

                services.Add(ServiceDescriptor.Describe(serviceType, newImplementationFactory, lifetime));
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


        #region TryReplace.Instance

        /// <summary>
        /// 尝试替换所有服务描述符。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="newInstance">给定的新实例。</param>
        /// <param name="throwIfNotFound">未找到服务类型时抛出异常（可选；默认启用）。</param>
        /// <returns>返回是否成功替换的布尔值。</returns>
        public static bool TryReplaceAll<TService>(this IServiceCollection services,
            TService newInstance, bool throwIfNotFound = true)
            where TService : class
            => services.TryReplaceAll(newInstance, out _, throwIfNotFound);

        /// <summary>
        /// 尝试替换所有服务描述符。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <typeparam name="TNewImplementation">指定的新实现类型。</typeparam>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="newInstance">给定的新实例。</param>
        /// <param name="throwIfNotFound">未找到服务类型时抛出异常（可选；默认启用）。</param>
        /// <returns>返回是否成功替换的布尔值。</returns>
        public static bool TryReplaceAll<TService, TNewImplementation>(this IServiceCollection services,
            TNewImplementation newInstance, bool throwIfNotFound = true)
            where TService : class
            where TNewImplementation : class, TService
            => services.TryReplaceAll<TService, TNewImplementation>(newInstance, out _, throwIfNotFound);

        /// <summary>
        /// 尝试替换所有服务描述符。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <typeparam name="TOldImplementation">指定的旧实现类型。</typeparam>
        /// <typeparam name="TNewImplementation">指定的新实现类型。</typeparam>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="newInstance">给定的新实例。</param>
        /// <param name="throwIfNotFound">未找到服务类型时抛出异常（可选；默认启用）。</param>
        /// <returns>返回是否成功替换的布尔值。</returns>
        public static bool TryReplaceAll<TService, TOldImplementation, TNewImplementation>(this IServiceCollection services,
            TNewImplementation newInstance, bool throwIfNotFound = true)
            where TService : class
            where TOldImplementation : class, TService
            where TNewImplementation : class, TService
            => services.TryReplaceAll<TService, TOldImplementation, TNewImplementation>(newInstance, out _, throwIfNotFound);

        /// <summary>
        /// 尝试替换所有服务描述符。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="serviceType">给定的服务类型。</param>
        /// <param name="newInstance">给定的新实例。</param>
        /// <param name="oldImplementationType">给定的旧实现类型（可选）。</param>
        /// <param name="throwIfNotFound">未找到服务类型时抛出异常（可选；默认启用）。</param>
        /// <returns>返回是否成功替换的布尔值。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static bool TryReplaceAll(this IServiceCollection services, Type serviceType,
            object newInstance, Type oldImplementationType = null, bool throwIfNotFound = true)
            => services.TryReplaceAll(serviceType, newInstance, out _, oldImplementationType, throwIfNotFound);


        /// <summary>
        /// 尝试替换所有服务描述符。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="newInstance">给定的新实例。</param>
        /// <param name="oldDescriptors">输出旧 <see cref="IReadOnlyList{ServiceDescriptor}"/>。</param>
        /// <param name="throwIfNotFound">未找到服务类型时抛出异常（可选；默认启用）。</param>
        /// <returns>返回是否成功替换的布尔值。</returns>
        public static bool TryReplaceAll<TService>(this IServiceCollection services,
            TService newInstance, out IReadOnlyList<ServiceDescriptor> oldDescriptors, bool throwIfNotFound = true)
            where TService : class
            => services.TryReplaceAll<TService, TService>(newInstance, out oldDescriptors, throwIfNotFound);

        /// <summary>
        /// 尝试替换所有服务描述符。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <typeparam name="TNewImplementation">指定的新实现类型。</typeparam>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="newInstance">给定的新实例。</param>
        /// <param name="oldDescriptors">输出旧 <see cref="IReadOnlyList{ServiceDescriptor}"/>。</param>
        /// <param name="throwIfNotFound">未找到服务类型时抛出异常（可选；默认启用）。</param>
        /// <returns>返回是否成功替换的布尔值。</returns>
        public static bool TryReplaceAll<TService, TNewImplementation>(this IServiceCollection services,
            TNewImplementation newInstance, out IReadOnlyList<ServiceDescriptor> oldDescriptors, bool throwIfNotFound = true)
            where TService : class
            where TNewImplementation : class, TService
            => services.TryReplaceAll(typeof(TService), newInstance, out oldDescriptors,
                oldImplementationType: null, throwIfNotFound);

        /// <summary>
        /// 尝试替换所有服务描述符。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <typeparam name="TOldImplementation">指定的旧实现类型。</typeparam>
        /// <typeparam name="TNewImplementation">指定的新实现类型。</typeparam>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="newInstance">给定的新实例。</param>
        /// <param name="oldDescriptors">输出旧 <see cref="IReadOnlyList{ServiceDescriptor}"/>。</param>
        /// <param name="throwIfNotFound">未找到服务类型时抛出异常（可选；默认启用）。</param>
        /// <returns>返回是否成功替换的布尔值。</returns>
        public static bool TryReplaceAll<TService, TOldImplementation, TNewImplementation>(this IServiceCollection services,
            TNewImplementation newInstance, out IReadOnlyList<ServiceDescriptor> oldDescriptors, bool throwIfNotFound = true)
            where TService : class
            where TOldImplementation : class, TService
            where TNewImplementation : class, TService
            => services.TryReplaceAll(typeof(TService), newInstance, out oldDescriptors,
                typeof(TOldImplementation), throwIfNotFound);

        /// <summary>
        /// 尝试替换所有服务描述符。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="serviceType">给定的服务类型。</param>
        /// <param name="newInstance">给定的新实例。</param>
        /// <param name="oldDescriptors">输出旧 <see cref="IReadOnlyList{ServiceDescriptor}"/>。</param>
        /// <param name="oldImplementationType">给定的旧实现类型（可选）。</param>
        /// <param name="throwIfNotFound">未找到服务类型时抛出异常（可选；默认启用）。</param>
        /// <returns>返回是否成功替换的布尔值。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static bool TryReplaceAll(this IServiceCollection services, Type serviceType,
            object newInstance, out IReadOnlyList<ServiceDescriptor> oldDescriptors,
            Type oldImplementationType = null, bool throwIfNotFound = true)
        {
            if (services.TryGetAll(serviceType, out oldDescriptors, oldImplementationType))
            {
                // 移除找到的所有描述符
                oldDescriptors.ForEach(descriptor => services.Remove(descriptor));

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
