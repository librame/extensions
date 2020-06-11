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
    using Data.ValueGenerators;

    /// <summary>
    /// 数据构建器接口。
    /// </summary>
    public interface IDataBuilder : IExtensionBuilder
    {
        /// <summary>
        /// 数据库设计时类型。
        /// </summary>
        Type DatabaseDesignTimeType { get; }

        /// <summary>
        /// 访问器泛型类型参数集合。
        /// </summary>
        /// <value>返回 <see cref="AccessorGenericTypeArguments"/>。</value>
        AccessorGenericTypeArguments GenericTypeArguments { get; }


        /// <summary>
        /// 查找访问器泛型类型参数集合。
        /// </summary>
        /// <typeparam name="TAccessor">指定的访问器类型。</typeparam>
        /// <returns>返回 <see cref="AccessorGenericTypeArguments"/>。</returns>
        AccessorGenericTypeArguments FindGenericTypeArguments<TAccessor>();

        /// <summary>
        /// 查找访问器泛型类型参数集合。
        /// </summary>
        /// <param name="accessorType">给定的访问器类型。</param>
        /// <returns>返回 <see cref="AccessorGenericTypeArguments"/>。</returns>
        AccessorGenericTypeArguments FindGenericTypeArguments(Type accessorType);


        /// <summary>
        /// 通过填充泛型类型参数集合添加泛型服务。
        /// </summary>
        /// <param name="serviceType">给定的服务类型（支持非泛型）。</param>
        /// <param name="implementationTypeDefinition">给定的实现类型定义。</param>
        /// <param name="populateServiceFactory">给定的填充服务类型工厂方法（可选；当服务类型为泛型类型定义时，此参数必填）。</param>
        /// <param name="populateImplementationFactory">给定的填充实现类型工厂方法（可选；默认填充主要泛型类型参数集合到实现类型定义）。</param>
        /// <param name="addEnumerable">添加为可枚举集合（可选；默认不是可枚举集合）。</param>
        /// <param name="genericTypeArguments">给定的 <see cref="AccessorGenericTypeArguments"/>（可选；默认使用当前访问器泛型类型参数集合）。</param>
        /// <returns>返回 <see cref="IDataBuilder"/>。</returns>
        IDataBuilder AddGenericServiceByPopulateGenericTypeArguments(Type serviceType,
            Type implementationTypeDefinition,
            Func<Type, AccessorGenericTypeArguments, Type> populateServiceFactory = null,
            Func<Type, AccessorGenericTypeArguments, Type> populateImplementationFactory = null,
            bool addEnumerable = false, AccessorGenericTypeArguments genericTypeArguments = null);


        /// <summary>
        /// 添加默认值生成器。
        /// </summary>
        /// <typeparam name="TGenerator">指定实现 <see cref="IDefaultValueGenerator{TValue}"/> 接口的默认值生成器类型，推荐从 <see cref="AbstractValueGenerator{TValue}"/> 派生。</typeparam>
        /// <returns>返回 <see cref="IDataBuilder"/>。</returns>
        IDataBuilder AddDefaultValueGenerator<TGenerator>()
            where TGenerator : IValueGeneratorIndication;

        /// <summary>
        /// 添加默认值生成器。
        /// </summary>
        /// <param name="generatorType">给定的默认值生成器类型（推荐从 <see cref="AbstractValueGenerator{TValue}"/> 派生）。</param>
        /// <returns>返回 <see cref="IDataBuilder"/>。</returns>
        IDataBuilder AddDefaultValueGenerator(Type generatorType);


        /// <summary>
        /// 添加存储标识符生成器。
        /// </summary>
        /// <typeparam name="TGenerator">指定实现 <see cref="IDataStoreIdentifierGenerator{TGenId}"/> 接口的存储标识符类型，推荐从 <see cref="AbstractDataStoreIdentifierGenerator{TGenId}"/> 派生。</typeparam>
        /// <returns>返回 <see cref="IDataBuilder"/>。</returns>
        IDataBuilder AddStoreIdentifierGenerator<TGenerator>()
            where TGenerator : class, IStoreIdentifierGeneratorIndication;

        /// <summary>
        /// 添加存储标识符生成器。
        /// </summary>
        /// <param name="generatorType">给定的生成器类型。</param>
        /// <returns>返回 <see cref="IDataBuilder"/>。</returns>
        IDataBuilder AddStoreIdentifierGenerator(Type generatorType);
    }
}
