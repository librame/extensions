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
    using Serializers;
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
        /// 名称。
        /// </summary>
        public string Name
            => GetType().Name;

        /// <summary>
        /// 类型。
        /// </summary>
        public SerializableString<Type> Type
            => new SerializableString<Type>(GetType());


        /// <summary>
        /// 父级构建器。
        /// </summary>
        /// <value>
        /// 返回 <see cref="IExtensionBuilder"/>。
        /// </value>
        [Newtonsoft.Json.JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
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
        [Newtonsoft.Json.JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public IServiceCollection Services { get; }


        /// <summary>
        /// 添加泛型服务（适用于服务类型为泛型类型定义且实现类型已完全实现该泛型类型定义的服务类型）。
        /// </summary>
        /// <param name="serviceTypeDefinition">给定的服务类型定义。</param>
        /// <param name="implementationType">给定的实现类型（不支持类型定义）。</param>
        /// <param name="addEnumerable">添加为可枚举集合（可选；默认不是可枚举集合）。</param>
        /// <param name="addImplementationTypeItself">添加实现类型服务自身（可选；默认不添加）。</param>
        /// <returns>返回 <see cref="IExtensionBuilder"/>。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数")]
        public virtual IExtensionBuilder AddGenericService(Type serviceTypeDefinition,
            Type implementationType, bool addEnumerable = false, bool addImplementationTypeItself = false)
        {
            if (false == serviceTypeDefinition?.IsGenericTypeDefinition)
                throw new NotSupportedException($"The service type '{serviceTypeDefinition}' only support generic type definition.");

            if ((bool)implementationType?.IsGenericTypeDefinition)
                throw new NotSupportedException($"The implementation type '{implementationType}' do not support generic type definition.");

            if (!implementationType.IsImplementedInterfaceType(serviceTypeDefinition, out var resultType))
                throw new InvalidOperationException($"The type '{implementationType}' does not implement '{serviceTypeDefinition}' interface.");

            // 使用已实现泛型类型定义的服务泛型类型参数数组来填充服务类型定义
            var serviceType = serviceTypeDefinition.MakeGenericType(resultType.GenericTypeArguments);

            // 如果不添加为可枚举集合，则尝试移除可能已存在的服务集合
            if (!addEnumerable)
                Services.TryRemoveAll(serviceType, throwIfNotFound: false);

            var characteristics = GetServiceCharacteristics(serviceTypeDefinition);
            Services.AddByCharacteristics(serviceType, implementationType, characteristics);

            // 如果要添加现类型服务自身
            if (addImplementationTypeItself)
                AddService(implementationType, sp => sp.GetRequiredService(serviceType));

            return this;
        }


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
