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
using System.Diagnostics.CodeAnalysis;

namespace Librame.Extensions.Data.Builders
{
    using Core.Builders;
    using Core.Services;
    using Data.Accessors;
    using Data.Mediators;
    using Data.Protectors;
    using Data.Stores;
    using Data.ValueGenerators;

    /// <summary>
    /// 数据构建器。
    /// </summary>
    public class DataBuilder : AbstractExtensionBuilder, IDataBuilder
    {
        /// <summary>
        /// 构造一个 <see cref="DataBuilder"/>。
        /// </summary>
        /// <param name="parentBuilder">给定的 <see cref="IExtensionBuilder"/>。</param>
        /// <param name="dependency">给定的 <see cref="DataBuilderDependency"/>。</param>
        public DataBuilder(IExtensionBuilder parentBuilder, DataBuilderDependency dependency)
            : base(parentBuilder, dependency)
        {
            Services.AddSingleton<IDataBuilder>(this);

            AddInternalServices();
        }


        /// <summary>
        /// 访问器泛型参数集合。
        /// </summary>
        public AccessorGenericTypeArguments GenericTypeArguments { get; internal set; }

        /// <summary>
        /// 数据库设计时类型。
        /// </summary>
        public Type DatabaseDesignTimeType { get; internal set; }


        private void AddInternalServices()
        {
            // Mediators
            AddService(typeof(AuditNotificationHandler<,>));
            AddService(typeof(EntityNotificationHandler<>));
            AddService(typeof(MigrationNotificationHandler<>));

            // Protectors
            AddService<IPrivacyDataProtector, PrivacyDataProtector>();

            // Stores
            AddStoreIdentifierGenerator<GuidDataStoreIdentifierGenerator>();

            // ValueGenerators
            AddDefaultValueGenerator<GuidDefaultValueGenerator>();
        }


        /// <summary>
        /// 获取指定服务类型的特征。
        /// </summary>
        /// <param name="serviceType">给定的服务类型。</param>
        /// <returns>返回 <see cref="ServiceCharacteristics"/>。</returns>
        public override ServiceCharacteristics GetServiceCharacteristics(Type serviceType)
            => DataBuilderServiceCharacteristicsRegistration.Register.GetOrDefault(serviceType);


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
        [SuppressMessage("Design", "CA1062:验证公共方法的参数")]
        public virtual IDataBuilder AddGenericServiceByPopulateGenericTypeArguments(Type serviceType,
            Type implementationTypeDefinition,
            Func<Type, AccessorGenericTypeArguments, Type> populateServiceFactory = null,
            Func<Type, AccessorGenericTypeArguments, Type> populateImplementationFactory = null,
            bool addEnumerable = false, AccessorGenericTypeArguments genericTypeArguments = null)
        {
            if (false == implementationTypeDefinition?.IsGenericTypeDefinition)
                throw new NotSupportedException($"The implementation type '{implementationTypeDefinition}' only support generic type definition.");

            if (!implementationTypeDefinition.IsImplementedInterface(serviceType, out var resultType))
                throw new InvalidOperationException($"The type '{implementationTypeDefinition}' does not implement '{serviceType}' interface.");

            var characteristics = GetServiceCharacteristics(serviceType);
            if (serviceType.IsGenericTypeDefinition)
                serviceType = PopulateGenericTypeArguments(serviceType, populateServiceFactory);

            var implementationType = PopulateGenericTypeArguments(implementationTypeDefinition, populateImplementationFactory);

            // 如果不添加为可枚举集合
            if (!addEnumerable)
                Services.TryReplaceAll(serviceType, implementationType, throwIfNotFound: false);

            Services.AddByCharacteristics(serviceType, implementationType, characteristics);
            return this;

            // PopulateGenericTypeArguments
            Type PopulateGenericTypeArguments(Type populateType,
                Func<Type, AccessorGenericTypeArguments, Type> populateFactory = null)
            {
                if (populateFactory.IsNull())
                {
                    populateFactory = (type, args) => type.MakeGenericType(
                        args.AuditType,
                        args.AuditPropertyType,
                        args.EntityType,
                        args.MigrationType,
                        args.TenantType,
                        args.GenIdType,
                        args.IncremIdType,
                        args.CreatedByType);
                }

                genericTypeArguments = genericTypeArguments ?? GenericTypeArguments;
                if (genericTypeArguments.IsNull())
                    throw new InvalidOperationException("Registration builder.AddAccessor().");

                return populateFactory.Invoke(populateType, genericTypeArguments);
            }
        }


        /// <summary>
        /// 添加默认值生成器。
        /// </summary>
        /// <typeparam name="TGenerator">指定实现 <see cref="IDefaultValueGenerator{TValue}"/> 接口的默认值生成器类型，推荐从 <see cref="AbstractValueGenerator{TValue}"/> 派生。</typeparam>
        /// <returns>返回 <see cref="IDataBuilder"/>。</returns>
        public virtual IDataBuilder AddDefaultValueGenerator<TGenerator>()
            where TGenerator : IValueGeneratorIndication
            => AddDefaultValueGenerator(typeof(TGenerator));

        /// <summary>
        /// 添加默认值生成器。
        /// </summary>
        /// <param name="generatorType">给定的默认值生成器类型（推荐从 <see cref="AbstractValueGenerator{TValue}"/> 派生）。</param>
        /// <returns>返回 <see cref="IDataBuilder"/>。</returns>
        public virtual IDataBuilder AddDefaultValueGenerator(Type generatorType)
        {
            AddGenericService(typeof(IDefaultValueGenerator<>), generatorType);
            return this;
        }


        /// <summary>
        /// 添加存储标识符生成器。
        /// </summary>
        /// <typeparam name="TGenerator">指定实现 <see cref="IDataStoreIdentifierGenerator{TGenId}"/> 接口的存储标识符类型，推荐从 <see cref="AbstractDataStoreIdentifierGenerator{TGenId}"/> 派生。</typeparam>
        /// <returns>返回 <see cref="IDataBuilder"/>。</returns>
        public virtual IDataBuilder AddStoreIdentifierGenerator<TGenerator>()
            where TGenerator : class, IStoreIdentifierGeneratorIndication
            => AddStoreIdentifierGenerator(typeof(TGenerator));

        /// <summary>
        /// 添加存储标识符生成器。
        /// </summary>
        /// <param name="generatorType">给定的生成器类型。</param>
        /// <returns>返回 <see cref="IDataBuilder"/>。</returns>
        public virtual IDataBuilder AddStoreIdentifierGenerator(Type generatorType)
        {
            AddGenericService(typeof(IStoreIdentifierGenerator<>), generatorType);
            return this;
        }

    }
}
