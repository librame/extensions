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

namespace Librame.Extensions.Data.Builders
{
    using Core.Builders;
    using Core.Services;
    using Data.Aspects;
    using Data.Mediators;
    using Data.Protectors;
    using Data.Services;
    using Data.Stores;

    internal class DataBuilder : AbstractExtensionBuilder, IDataBuilder
    {
        public DataBuilder(IExtensionBuilder parentBuilder, DataBuilderDependency dependency)
            : base(parentBuilder, dependency)
        {
            Services.AddSingleton<IDataBuilder>(this);

            AddDataServices();
        }


        public Type DatabaseDesignTimeType { get; internal set; }


        public override ServiceCharacteristics GetServiceCharacteristics(Type serviceType)
            => DataBuilderServiceCharacteristicsRegistration.Register.GetOrDefault(serviceType);


        private void AddDataServices()
        {
            // Aspects
            AddService(typeof(DbContextAccessorAspectDependencies<>));
            AddServices(typeof(ISaveChangesDbContextAccessorAspect<,,,,,,>),
                typeof(DataAuditSaveChangesDbContextAccessorAspect<,,,,,,>));
            AddServices(typeof(IMigrateDbContextAccessorAspect<,,,,,,>),
                typeof(DataEntityMigrateDbContextAccessorAspect<,,,,,,>),
                typeof(DataMigrationMigrateDbContextAccessorAspect<,,,,,,>));

            // Mediators
            AddService(typeof(AuditNotificationHandler<,>));
            AddService(typeof(EntityNotificationHandler<,>));
            AddService(typeof(MigrationNotificationHandler<,>));

            // Protectors
            AddService<IPrivacyDataProtector, PrivacyDataProtector>();

            // Services
            AddService(typeof(IMigrationService<,,,,,,>), typeof(MigrationService<,,,,,,>));
            AddService(typeof(ITenantService<,,,,,,>), typeof(TenantService<,,,,,,>));

            // Stores
            AddService(typeof(IStoreHub<,>), typeof(StoreHub<,>));
            AddService(typeof(IStoreHub<,,,,,,>), typeof(StoreHub<,,,,,,>));
            AddStoreIdentifierGenerator<GuidStoreIdentifierGenerator>();
            AddStoreInitializer<GuidStoreInitializer>();
        }


        /// <summary>
        /// 添加存储中心。
        /// </summary>
        /// <typeparam name="THub">指定实现 <see cref="IStoreHub{TGenId, TIncremId}"/> 或 <see cref="IStoreHub{TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId}"/> 接口的存储中心类型，推荐从 <see cref="StoreHub{TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId}"/> 中派生。</typeparam>
        /// <returns>返回 <see cref="IDataBuilder"/>。</returns>
        public virtual IDataBuilder AddStoreHub<THub>()
            where THub : class, IStoreHub
        {
            var hubTypeDefinition = typeof(IStoreHub<,>);
            var hubType = typeof(THub);

            Type hubTypeGeneric = null;
            if (hubType.IsImplementedInterface(hubTypeDefinition, out Type resultType))
            {
                // 利用类型定义获取服务特征
                var characteristics = GetServiceCharacteristics(hubTypeDefinition);
                // 使用泛型参数填充服务类型
                hubTypeGeneric = hubTypeDefinition.MakeGenericType(resultType.GenericTypeArguments);
                Services.AddByCharacteristics(hubTypeGeneric, hubType, characteristics);

                AddService(sp => (THub)sp.GetRequiredService(hubTypeGeneric));
            }

            var hubTypeFullDefinition = typeof(IStoreHub<,,,,,,>);
            if (hubType.IsImplementedInterface(hubTypeFullDefinition, out resultType))
            {
                // 利用类型定义获取服务特征
                var characteristics = GetServiceCharacteristics(hubTypeFullDefinition);
                // 使用泛型参数填充服务类型
                hubTypeGeneric = hubTypeFullDefinition.MakeGenericType(resultType.GenericTypeArguments);
                Services.AddByCharacteristics(hubTypeGeneric, hubType, characteristics);

                AddService(sp => (THub)sp.GetRequiredService(hubTypeGeneric));
            }

            if (resultType.IsNull())
                throw new ArgumentException($"The store hub type '{hubType}' does not implement '{hubTypeDefinition}' or '{hubTypeFullDefinition}' interface.");

            return this;
        }


        /// <summary>
        /// 添加存储标识符生成器。
        /// </summary>
        /// <typeparam name="TGenerator">指定实现 <see cref="IStoreIdentifierGenerator{TGenId}"/> 接口的存储标识符类型，推荐从 <see cref="AbstractStoreIdentifierGenerator{TGenId}"/> 派生。</typeparam>
        /// <returns>返回 <see cref="IDataBuilder"/>。</returns>
        public virtual IDataBuilder AddStoreIdentifierGenerator<TGenerator>()
            where TGenerator : class, IStoreIdentifierGenerator
        {
            var generatorTypeDefinition = typeof(IStoreIdentifierGenerator<>);
            var generatorType = typeof(TGenerator);

            if (generatorType.IsImplementedInterface(generatorTypeDefinition, out Type resultType))
            {
                // 利用类型定义获取服务特征
                var characteristics = GetServiceCharacteristics(generatorTypeDefinition);
                // 使用泛型参数填充服务类型
                var generatorTypeGeneric = generatorTypeDefinition.MakeGenericType(resultType.GenericTypeArguments);

                if (!Services.TryReplace(generatorTypeGeneric, generatorType, throwIfNotFound: false))
                    Services.AddByCharacteristics(generatorTypeGeneric, generatorType, characteristics);

                AddService(sp => (TGenerator)sp.GetRequiredService(generatorTypeGeneric));
            }
            else
            {
                throw new ArgumentException($"The store identifier type '{generatorType}' does not implement '{generatorTypeDefinition}' interface.");
            }

            return this;
        }

        /// <summary>
        /// 添加存储初始化器。
        /// </summary>
        /// <typeparam name="TInitializer">指定实现 <see cref="IStoreInitializer{TGenId}"/> 接口的存储初始化器类型，推荐从 <see cref="AbstractStoreInitializer{TGenId}"/> 派生。</typeparam>
        /// <returns>返回 <see cref="IDataBuilder"/>。</returns>
        public virtual IDataBuilder AddStoreInitializer<TInitializer>()
            where TInitializer : class, IStoreInitializer
        {
            var initializerTypeDefinition = typeof(IStoreInitializer<>);
            var initializerType = typeof(TInitializer);

            if (initializerType.IsImplementedInterface(initializerTypeDefinition, out Type resultType))
            {
                // 利用类型定义获取服务特征
                var characteristics = GetServiceCharacteristics(initializerTypeDefinition);
                // 使用泛型参数填充服务类型
                var initializerTypeGeneric = initializerTypeDefinition.MakeGenericType(resultType.GenericTypeArguments);

                if (!Services.TryReplace(initializerTypeGeneric, initializerType, throwIfNotFound: false))
                    Services.AddByCharacteristics(initializerTypeGeneric, initializerType, characteristics);

                AddService(sp => (TInitializer)sp.GetRequiredService(initializerTypeGeneric));
            }
            else
            {
                throw new ArgumentException($"The store initializer type '{initializerType}' does not implement '{initializerTypeDefinition}' interface.");
            }

            return this;
        }

    }
}
