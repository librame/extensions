#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;

namespace Librame.Extensions.Data.Builders
{
    using Core.Builders;
    using Data.Accessors;
    using Data.Stores;

    /// <summary>
    /// 数据构建器接口。
    /// </summary>
    public interface IDataBuilder : IExtensionBuilder
    {
        /// <summary>
        /// 访问器泛型类型映射描述符。
        /// </summary>
        /// <value>返回 <see cref="AccessorGenericTypeMappingDescriptor"/>。</value>
        AccessorGenericTypeMappingDescriptor AccessorMappingDescriptor { get; }

        /// <summary>
        /// 数据库设计时类型。
        /// </summary>
        Type DatabaseDesignTimeType { get; }


        /// <summary>
        /// 通过填充泛型类型参数集合添加泛型服务。
        /// </summary>
        /// <param name="serviceType">给定的服务类型（支持非泛型）。</param>
        /// <param name="implementationTypeDefinition">给定的实现类型定义。</param>
        /// <param name="populateServiceFactory">给定的填充服务类型工厂方法（可选；当服务类型为泛型类型定义时，此参数必填）。</param>
        /// <param name="populateImplementationFactory">给定的填充实现类型工厂方法（可选；默认填充主要泛型类型参数集合到实现类型定义）。</param>
        /// <param name="addEnumerable">添加为可枚举集合（可选；默认不是可枚举集合）。</param>
        /// <param name="accessorMappingDescriptor">给定的 <see cref="AccessorGenericTypeMappingDescriptor"/>（可选；默认使用当前访问器泛型类型参数集合）。</param>
        /// <returns>返回 <see cref="IDataBuilder"/>。</returns>
        IDataBuilder AddGenericServiceByPopulateMappingDescriptor(Type serviceType,
            Type implementationTypeDefinition,
            Func<Type, AccessorGenericTypeMappingDescriptor, Type> populateServiceFactory = null,
            Func<Type, AccessorGenericTypeMappingDescriptor, Type> populateImplementationFactory = null,
            bool addEnumerable = false, AccessorGenericTypeMappingDescriptor accessorMappingDescriptor = null);


        /// <summary>
        /// 添加存储中心。
        /// </summary>
        /// <typeparam name="THub">指定实现 <see cref="IStoreHub"/> 接口的存储中心类型，推荐从 <see cref="AbstractStoreHub"/> 派生，可选实现 <see cref="IDataStoreHub{TAccessor, TAudit, TAuditProperty, TEntity, TMigration, TTenant}"/> 接口。</typeparam>
        /// <returns>返回 <see cref="IDataBuilder"/>。</returns>
        IDataBuilder AddStoreHub<THub>()
            where THub : class, IStoreHub;

        /// <summary>
        /// 添加存储标识符生成器。
        /// </summary>
        /// <typeparam name="TGenerator">指定实现 <see cref="IStoreIdentifierGenerator"/> 接口的存储标识符类型，推荐从 <see cref="AbstractDataStoreIdentifierGenerator{TGenId}"/> 派生。</typeparam>
        /// <returns>返回 <see cref="IDataBuilder"/>。</returns>
        IDataBuilder AddStoreIdentifierGenerator<TGenerator>()
            where TGenerator : class, IStoreIdentifierGenerator;

        /// <summary>
        /// 添加存储初始化器。
        /// </summary>
        /// <typeparam name="TInitializer">指定实现 <see cref="IStoreInitializer"/> 接口的初始化器类型，推荐从 <see cref="AbstractDataStoreInitializer{TAccessor, TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId, TCreatedBy}"/> 派生。</typeparam>
        /// <returns>返回 <see cref="IDataBuilder"/>。</returns>
        IDataBuilder AddStoreInitializer<TInitializer>()
            where TInitializer : class, IStoreInitializer;
    }
}
