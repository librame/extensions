﻿#region License

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

namespace Librame.Extensions.Core.Builders
{
    using Serializers;
    using Services;

    /// <summary>
    /// 扩展构建器接口。
    /// </summary>
    public interface IExtensionBuilder
    {
        /// <summary>
        /// 名称。
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 类型。
        /// </summary>
        SerializableString<Type> Type { get; }


        /// <summary>
        /// 父级构建器。
        /// </summary>
        /// <value>
        /// 返回 <see cref="IExtensionBuilder"/>。
        /// </value>
        IExtensionBuilder ParentBuilder { get; }

        /// <summary>
        /// 构建器依赖。
        /// </summary>
        /// <value>
        /// 返回 <see cref="IExtensionBuilderDependency"/>。
        /// </value>
        IExtensionBuilderDependency Dependency { get; }

        /// <summary>
        /// 服务集合。
        /// </summary>
        /// <value>
        /// 返回 <see cref="IServiceCollection"/>。
        /// </value>
        IServiceCollection Services { get; }


        /// <summary>
        /// 添加泛型服务（适用于服务类型为泛型类型定义且实现类型已完全实现该泛型类型定义的服务类型）。
        /// </summary>
        /// <param name="serviceTypeDefinition">给定的服务类型定义。</param>
        /// <param name="implementationType">给定的实现类型（不支持类型定义）。</param>
        /// <param name="addEnumerable">添加为可枚举集合（可选；默认不是可枚举集合）。</param>
        /// <param name="addImplementationTypeItself">添加实现类型服务自身（可选；默认不添加）。</param>
        /// <returns>返回 <see cref="IExtensionBuilder"/>。</returns>
        IExtensionBuilder AddGenericService(Type serviceTypeDefinition,
            Type implementationType, bool addEnumerable = false, bool addImplementationTypeItself = false);


        /// <summary>
        /// 添加服务引用（支持服务特征）。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <typeparam name="TImplementation">指定的实现类型。</typeparam>
        /// <param name="factory">给定的服务类型对象引用转换为实现实例的工厂方法（可选；默认直接表示为实现实例）。</param>
        /// <returns>返回 <see cref="IExtensionBuilder"/>。</returns>
        IExtensionBuilder AddServiceReference<TService, TImplementation>(Func<object, TImplementation> factory = null)
            where TImplementation : TService;


        /// <summary>
        /// 添加服务（支持服务特征）。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <param name="factory">给定的服务对象工厂方法。</param>
        /// <returns>返回 <see cref="IExtensionBuilder"/>。</returns>
        IExtensionBuilder AddService<TService>(Func<IServiceProvider, TService> factory)
            where TService : class;

        /// <summary>
        /// 添加服务（支持服务特征）。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <param name="factory">给定的服务对象工厂方法。</param>
        /// <returns>返回 <see cref="IExtensionBuilder"/>。</returns>
        IExtensionBuilder AddService<TService>(Func<IServiceProvider, object> factory);

        /// <summary>
        /// 添加服务（支持服务特征）。
        /// </summary>
        /// <param name="serviceType">给定的服务类型。</param>
        /// <param name="factory">给定的服务对象工厂方法。</param>
        /// <returns>返回 <see cref="IExtensionBuilder"/>。</returns>
        IExtensionBuilder AddService(Type serviceType, Func<IServiceProvider, object> factory);


        /// <summary>
        /// 添加服务（支持服务特征）。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <returns>返回 <see cref="IExtensionBuilder"/>。</returns>
        IExtensionBuilder AddService<TService>();

        /// <summary>
        /// 添加服务（支持服务特征）。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <typeparam name="TImplementationType">指定的实现类型。</typeparam>
        /// <returns>返回 <see cref="IExtensionBuilder"/>。</returns>
        IExtensionBuilder AddService<TService, TImplementationType>();

        /// <summary>
        /// 添加服务（支持服务特征）。
        /// </summary>
        /// <param name="serviceType">给定的服务类型。</param>
        /// <returns>返回 <see cref="IExtensionBuilder"/>。</returns>
        IExtensionBuilder AddService(Type serviceType);

        /// <summary>
        /// 添加服务（支持服务特征）。
        /// </summary>
        /// <param name="serviceType">给定的服务类型。</param>
        /// <param name="implementationType">给定的实现类型。</param>
        /// <returns>返回 <see cref="IExtensionBuilder"/>。</returns>
        IExtensionBuilder AddService(Type serviceType, Type implementationType);


        /// <summary>
        /// 添加服务数组（支持服务特征）。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <param name="implementationTypes">给定的实现类型数组。</param>
        /// <returns>返回 <see cref="IExtensionBuilder"/>。</returns>
        IExtensionBuilder AddServices<TService>(params Type[] implementationTypes);

        /// <summary>
        /// 添加服务集合（支持服务特征）。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <param name="implementationTypes">给定的实现类型集合。</param>
        /// <returns>返回 <see cref="IExtensionBuilder"/>。</returns>
        IExtensionBuilder AddServices<TService>(IEnumerable<Type> implementationTypes);

        /// <summary>
        /// 添加服务数组（支持服务特征）。
        /// </summary>
        /// <param name="serviceType">给定的服务类型。</param>
        /// <param name="implementationTypes">给定的实现类型数组。</param>
        /// <returns>返回 <see cref="IExtensionBuilder"/>。</returns>
        IExtensionBuilder AddServices(Type serviceType, params Type[] implementationTypes);

        /// <summary>
        /// 添加服务集合（支持服务特征）。
        /// </summary>
        /// <param name="serviceType">给定的服务类型。</param>
        /// <param name="implementationTypes">给定的实现类型集合。</param>
        /// <returns>返回 <see cref="IExtensionBuilder"/>。</returns>
        IExtensionBuilder AddServices(Type serviceType, IEnumerable<Type> implementationTypes);


        /// <summary>
        /// 获取指定服务类型的特征。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <returns>返回 <see cref="ServiceCharacteristics"/>。</returns>
        ServiceCharacteristics GetServiceCharacteristics<TService>();

        /// <summary>
        /// 获取指定服务类型的特征。
        /// </summary>
        /// <param name="serviceType">给定的服务类型。</param>
        /// <returns>返回 <see cref="ServiceCharacteristics"/>。</returns>
        ServiceCharacteristics GetServiceCharacteristics(Type serviceType);
    }
}
