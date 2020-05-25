#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Librame.Extensions.Core.Builders
{
    using Services;

    /// <summary>
    /// 抽象扩展构建器。
    /// </summary>
    public abstract class AbstractExtensionBuilder : IExtensionBuilder
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractExtensionBuilder"/>。
        /// </summary>
        /// <param name="parentBuilder">给定的父级 <see cref="IExtensionBuilder"/>。</param>
        /// <param name="dependency">给定的 <see cref="IExtensionBuilderDependency"/>。</param>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        protected AbstractExtensionBuilder(IExtensionBuilder parentBuilder, IExtensionBuilderDependency dependency)
        {
            Dependency = dependency.NotNull(nameof(dependency));
            ParentBuilder = parentBuilder.NotNull(nameof(parentBuilder));

            Services = parentBuilder.Services;
        }

        /// <summary>
        /// 构造一个 <see cref="AbstractExtensionBuilder"/>。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="dependency">给定的 <see cref="IExtensionBuilderDependency"/>。</param>
        protected AbstractExtensionBuilder(IServiceCollection services, IExtensionBuilderDependency dependency)
        {
            Dependency = dependency.NotNull(nameof(dependency));
            Services = services.NotNull(nameof(services));
        }


        /// <summary>
        /// 父级构建器。
        /// </summary>
        /// <value>
        /// 返回 <see cref="IExtensionBuilder"/>。
        /// </value>
        public IExtensionBuilder ParentBuilder { get; }

        /// <summary>
        /// 构建器依赖。
        /// </summary>
        /// <value>
        /// 返回 <see cref="IExtensionBuilderDependency"/>。
        /// </value>
        public IExtensionBuilderDependency Dependency { get; }

        /// <summary>
        /// 服务集合。
        /// </summary>
        /// <value>
        /// 返回 <see cref="IServiceCollection"/>。
        /// </value>
        public IServiceCollection Services { get; }


        /// <summary>
        /// 添加服务（支持服务特征）。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <param name="factory">给定的服务对象工厂方法。</param>
        /// <returns>返回 <see cref="IExtensionBuilder"/>。</returns>
        public virtual IExtensionBuilder AddService<TService>(Func<IServiceProvider, TService> factory)
        {
            var serviceType = typeof(TService);

            var characteristics = GetServiceCharacteristics(serviceType);
            Services.AddByCharacteristics(serviceType, sp => factory.Invoke(sp), characteristics);

            return this;
        }

        /// <summary>
        /// 添加服务（支持服务特征）。
        /// </summary>
        /// <param name="serviceType">给定的服务类型。</param>
        /// <param name="factory">给定的服务对象工厂方法。</param>
        /// <returns>返回 <see cref="IExtensionBuilder"/>。</returns>
        public virtual IExtensionBuilder AddService(Type serviceType, Func<IServiceProvider, object> factory)
        {
            var characteristics = GetServiceCharacteristics(serviceType);
            Services.AddByCharacteristics(serviceType, factory, characteristics);

            return this;
        }


        /// <summary>
        /// 添加服务（支持服务特征）。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <returns>返回 <see cref="IExtensionBuilder"/>。</returns>
        public virtual IExtensionBuilder AddService<TService>()
            => AddService(typeof(TService));

        /// <summary>
        /// 添加服务（支持服务特征）。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <typeparam name="TImplementationType">指定的实现类型。</typeparam>
        /// <returns>返回 <see cref="IExtensionBuilder"/>。</returns>
        public virtual IExtensionBuilder AddService<TService, TImplementationType>()
            => AddService(typeof(TService), typeof(TImplementationType));

        /// <summary>
        /// 添加服务（支持服务特征）。
        /// </summary>
        /// <param name="serviceType">给定的服务类型。</param>
        /// <returns>返回 <see cref="IExtensionBuilder"/>。</returns>
        public virtual IExtensionBuilder AddService(Type serviceType)
            => AddService(serviceType, serviceType);

        /// <summary>
        /// 添加服务（支持服务特征）。
        /// </summary>
        /// <param name="serviceType">给定的服务类型。</param>
        /// <param name="implementationType">给定的实现类型。</param>
        /// <returns>返回 <see cref="IExtensionBuilder"/>。</returns>
        public virtual IExtensionBuilder AddService(Type serviceType, Type implementationType)
        {
            var characteristics = GetServiceCharacteristics(serviceType);
            Services.AddByCharacteristics(serviceType, implementationType, characteristics);

            return this;
        }


        /// <summary>
        /// 添加服务数组（支持服务特征）。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <param name="implementationTypes">给定的实现类型数组。</param>
        /// <returns>返回 <see cref="IExtensionBuilder"/>。</returns>
        public virtual IExtensionBuilder AddServices<TService>(params Type[] implementationTypes)
            => AddServices(typeof(TService), implementationTypes);

        /// <summary>
        /// 添加服务集合（支持服务特征）。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <param name="implementationTypes">给定的实现类型集合。</param>
        /// <returns>返回 <see cref="IExtensionBuilder"/>。</returns>
        public virtual IExtensionBuilder AddServices<TService>(IEnumerable<Type> implementationTypes)
            => AddServices(typeof(TService), implementationTypes);

        /// <summary>
        /// 添加服务数组（支持服务特征）。
        /// </summary>
        /// <param name="serviceType">给定的服务类型。</param>
        /// <param name="implementationTypes">给定的实现类型数组。</param>
        /// <returns>返回 <see cref="IExtensionBuilder"/>。</returns>
        public virtual IExtensionBuilder AddServices(Type serviceType, params Type[] implementationTypes)
            => AddServices(serviceType, implementationTypes.AsEnumerable());

        /// <summary>
        /// 添加服务集合（支持服务特征）。
        /// </summary>
        /// <param name="serviceType">给定的服务类型。</param>
        /// <param name="implementationTypes">给定的实现类型集合。</param>
        /// <returns>返回 <see cref="IExtensionBuilder"/>。</returns>
        public virtual IExtensionBuilder AddServices(Type serviceType, IEnumerable<Type> implementationTypes)
        {
            var characteristics = GetServiceCharacteristics(serviceType);
            Services.AddByCharacteristics(serviceType, implementationTypes, characteristics);

            return this;
        }


        /// <summary>
        /// 获取指定服务类型的特征。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <returns>返回 <see cref="ServiceCharacteristics"/>。</returns>
        public virtual ServiceCharacteristics GetServiceCharacteristics<TService>()
            => GetServiceCharacteristics(typeof(TService));

        /// <summary>
        /// 获取指定服务类型的特征。
        /// </summary>
        /// <param name="serviceType">给定的服务类型。</param>
        /// <returns>返回 <see cref="ServiceCharacteristics"/>。</returns>
        public abstract ServiceCharacteristics GetServiceCharacteristics(Type serviceType);
    }
}
