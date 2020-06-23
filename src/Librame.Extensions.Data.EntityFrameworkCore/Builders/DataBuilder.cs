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
using System.Diagnostics.CodeAnalysis;

namespace Librame.Extensions.Data.Builders
{
    using Core.Builders;
    using Core.Services;
    using Data.Accessors;
    using Data.Mediators;
    using Data.Protectors;
    using Data.Stores;

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
        /// 访问器泛型类型映射描述符。
        /// </summary>
        public AccessorGenericTypeMappingDescriptor AccessorMappingDescriptor { get; private set; }

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
            AddService<IStoreInitializationValidator, StoreInitializationValidator>();
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
        /// <param name="accessorMappingDescriptor">给定的 <see cref="AccessorGenericTypeMappingDescriptor"/>（可选；默认使用当前访问器泛型类型映射描述符）。</param>
        /// <returns>返回 <see cref="IDataBuilder"/>。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数")]
        public virtual IDataBuilder AddGenericServiceByPopulateMappingDescriptor(Type serviceType,
            Type implementationTypeDefinition,
            Func<Type, AccessorGenericTypeMappingDescriptor, Type> populateServiceFactory = null,
            Func<Type, AccessorGenericTypeMappingDescriptor, Type> populateImplementationFactory = null,
            bool addEnumerable = false, AccessorGenericTypeMappingDescriptor accessorMappingDescriptor = null)
        {
            if (false == implementationTypeDefinition?.IsGenericTypeDefinition)
                throw new NotSupportedException($"The implementation type '{implementationTypeDefinition}' only support generic type definition.");
            
            if (!implementationTypeDefinition.IsImplementedInterface(serviceType, out var resultType))
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
            Type PopulateGenericTypeArguments(Type populateType,
                Func<Type, AccessorGenericTypeMappingDescriptor, Type> populateFactory = null)
            {
                if (populateFactory.IsNull())
                {
                    populateFactory = (type, args) => type.MakeGenericType(
                        args.Audit.ArgumentType,
                        args.AuditProperty.ArgumentType,
                        args.Entity.ArgumentType,
                        args.Migration.ArgumentType,
                        args.Tenant.ArgumentType,
                        args.GenId.ArgumentType,
                        args.IncremId.ArgumentType,
                        args.CreatedBy.ArgumentType);
                }

                accessorMappingDescriptor = accessorMappingDescriptor ?? AccessorMappingDescriptor;
                if (accessorMappingDescriptor.IsNull())
                    throw new InvalidOperationException($"The {nameof(AccessorMappingDescriptor)} is null. You should use the {nameof(AccessorDataBuilderExtensions.AddAccessor)}().");

                return populateFactory.Invoke(populateType, accessorMappingDescriptor);
            }
        }


        /// <summary>
        /// 添加存储中心。
        /// </summary>
        /// <typeparam name="THub">指定实现 <see cref="IStoreHub"/> 接口的存储中心类型，推荐从 <see cref="AbstractStoreHub"/> 派生，可选实现 <see cref="IDataStoreHub{TAudit, TAuditProperty, TEntity, TMigration, TTenant}"/> 接口。</typeparam>
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
        /// <typeparam name="TGenerator">指定实现 <see cref="IStoreIdentifierGenerator"/> 接口的存储标识符类型，推荐从 <see cref="AbstractDataStoreIdentifierGenerator{TGenId}"/> 派生。</typeparam>
        /// <returns>返回 <see cref="IDataBuilder"/>。</returns>
        public virtual IDataBuilder AddStoreIdentifierGenerator<TGenerator>()
            where TGenerator : class, IStoreIdentifierGenerator
        {
            AddService<IStoreIdentifierGenerator, TGenerator>();
            return this;
        }

        /// <summary>
        /// 添加存储初始化器。
        /// </summary>
        /// <typeparam name="TInitializer">指定实现 <see cref="IStoreInitializer"/> 接口的初始化器类型，推荐从 <see cref="AbstractDataStoreInitializer{TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId, TCreatedBy}"/> 派生。</typeparam>
        /// <returns>返回 <see cref="IDataBuilder"/>。</returns>
        public virtual IDataBuilder AddStoreInitializer<TInitializer>()
            where TInitializer : class, IStoreInitializer
        {
            AddService<IStoreInitializer, TInitializer>();
            return this;
        }

    }
}
