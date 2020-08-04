#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Librame.Extensions.Data.Builders
{
    using Core.Builders;
    using Core.Services;
    using Data.Mappers;
    using Data.Mediators;
    using Data.Protectors;
    using Data.Stores;
    using Data.Validators;

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
        /// 内存缓存。
        /// </summary>
        public IMemoryCache MemoryCache { get; private set; }

        /// <summary>
        /// 访问器类型参数映射器。
        /// </summary>
        public AccessorTypeParameterMapper AccessorTypeParameterMapper { get; private set; }

        /// <summary>
        /// 数据库设计时类型。
        /// </summary>
        public Type DatabaseDesignTimeType { get; private set; }


        private void AddInternalServices()
        {
            // Mediators
            AddService(typeof(AuditNotificationHandler<,>));
            AddService(typeof(TabulationNotificationHandler<>));
            AddService(typeof(MigrationNotificationHandler<>));

            // Protectors
            AddService<IPrivacyDataProtector, PrivacyDataProtector>();

            // Validators
            AddService<IDatabaseCreationValidator, DatabaseCreationValidator>();
            AddService<IDataInitializationValidator, DataInitializationValidator>();
            AddService<IMigrationCommandExecutionValidator, MigrationCommandExecutionValidator>();
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
        /// <param name="populateImplementationFactory">给定的填充实现类型工厂方法（可选；默认使用 <see cref="Mappers.AccessorTypeParameterMapper"/> 填充实现类型定义）。</param>
        /// <param name="addEnumerable">添加为可枚举集合（可选；默认不是可枚举集合）。</param>
        /// <param name="accessorTypeParameterMapper">给定的 <see cref="Mappers.AccessorTypeParameterMapper"/>（可选；默认使用 <see cref="Mappers.AccessorTypeParameterMapper"/>）。</param>
        /// <returns>返回 <see cref="IDataBuilder"/>。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数")]
        public virtual IDataBuilder AddGenericServiceByPopulateAccessorTypeParameters(Type serviceType,
            Type implementationTypeDefinition,
            Func<Type, AccessorTypeParameterMapper, Type> populateServiceFactory = null,
            Func<Type, AccessorTypeParameterMapper, Type> populateImplementationFactory = null,
            bool addEnumerable = false, AccessorTypeParameterMapper accessorTypeParameterMapper = null)
        {
            if (false == implementationTypeDefinition?.IsGenericTypeDefinition)
                throw new NotSupportedException($"The implementation type '{implementationTypeDefinition}' only support generic type definition.");
            
            if (!implementationTypeDefinition.IsImplementedInterfaceType(serviceType, out var resultType))
                throw new InvalidOperationException($"The type '{implementationTypeDefinition}' does not implement '{serviceType}' interface.");

            var characteristics = GetServiceCharacteristics(serviceType);
            if (serviceType.IsGenericTypeDefinition)
                serviceType = PopulateGenericTypeArguments(serviceType, populateServiceFactory);

            var implementationType = PopulateGenericTypeArguments(implementationTypeDefinition, populateImplementationFactory);

            // 如果不添加为可枚举集合，则尝试移除可能已存在的服务集合
            if (!addEnumerable)
                Services.TryRemoveAll(serviceType, throwIfNotFound: false);

            Services.AddByCharacteristics(serviceType, implementationType, characteristics);
            return this;

            // PopulateGenericTypeArguments
            Type PopulateGenericTypeArguments(Type populateTypeDefinition,
                Func<Type, AccessorTypeParameterMapper, Type> populateTypeDefinitionFactory = null)
            {
                if (populateTypeDefinitionFactory.IsNull())
                {
                    populateTypeDefinitionFactory = (type, mapper) => mapper
                        .PopulateGenericTypeDefinitionByAllMappingWithoutCreatedTime(type);
                }

                accessorTypeParameterMapper = accessorTypeParameterMapper ?? AccessorTypeParameterMapper;
                if (accessorTypeParameterMapper.IsNull())
                    throw new InvalidOperationException($"The {nameof(Mappers.AccessorTypeParameterMapper)} is null. You should use the {nameof(AccessorDataBuilderExtensions.AddAccessor)}().");

                return populateTypeDefinitionFactory.Invoke(populateTypeDefinition, accessorTypeParameterMapper);
            }
        }


        /// <summary>
        /// 添加存储中心。
        /// </summary>
        /// <typeparam name="THub">指定实现 <see cref="IStoreHub"/> 接口的存储中心类型，推荐从 <see cref="DataStoreHub{TAccessor}"/> 派生。</typeparam>
        /// <returns>返回 <see cref="IDataBuilder"/>。</returns>
        public virtual IDataBuilder AddStoreHub<THub>()
            where THub : class, IStoreHub
        {
            AddService<IStoreHub, THub>();
            AddService(sp => (THub)sp.GetService<IStoreHub>());
            return this;
        }

        /// <summary>
        /// 添加存储标识符生成器。
        /// </summary>
        /// <typeparam name="TGenerator">指定实现 <see cref="IStoreIdentityGenerator"/> 接口的存储标识符类型，推荐使用 <see cref="GuidDataStoreIdentityGenerator"/>。</typeparam>
        /// <returns>返回 <see cref="IDataBuilder"/>。</returns>
        public virtual IDataBuilder AddStoreIdentifierGenerator<TGenerator>()
            where TGenerator : class, IStoreIdentityGenerator
        {
            AddService<IStoreIdentityGenerator, TGenerator>();
            AddService(sp => (TGenerator)sp.GetService<IStoreIdentityGenerator>());
            return this;
        }

        /// <summary>
        /// 添加存储初始化器。
        /// </summary>
        /// <typeparam name="TInitializer">指定实现 <see cref="IStoreInitializer"/> 接口的初始化器类型，推荐从 <see cref="DataStoreInitializer{TAccessor}"/> 派生。</typeparam>
        /// <returns>返回 <see cref="IDataBuilder"/>。</returns>
        public virtual IDataBuilder AddStoreInitializer<TInitializer>()
            where TInitializer : class, IStoreInitializer
        {
            AddService<IStoreInitializer, TInitializer>();
            AddService(sp => (TInitializer)sp.GetService<IStoreInitializer>());
            return this;
        }

    }
}
