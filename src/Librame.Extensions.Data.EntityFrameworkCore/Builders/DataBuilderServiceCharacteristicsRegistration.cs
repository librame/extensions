#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;
using System.Collections.Generic;

namespace Librame.Extensions.Data.Builders
{
    using Core.Services;
    using Data.Aspects;
    using Data.Mediators;
    using Data.Protectors;
    using Data.Services;
    using Data.Stores;

    /// <summary>
    /// <see cref="IDataBuilder"/> 服务特征注册。
    /// </summary>
    public static class DataBuilderServiceCharacteristicsRegistration
    {
        private static IServiceCharacteristicsRegister _register;

        /// <summary>
        /// 当前注册器。
        /// </summary>
        public static IServiceCharacteristicsRegister Register
        {
            get => _register.EnsureSingleton(() => new ServiceCharacteristicsRegister(InitializeCharacteristics()));
            set => _register = value.NotNull(nameof(value));
        }


        private static IDictionary<Type, ServiceCharacteristics> InitializeCharacteristics()
        {
            return new Dictionary<Type, ServiceCharacteristics>
            {
                // Aspects
                { typeof(DbContextAccessorAspectDependencies<>), ServiceCharacteristics.Singleton() },
                { typeof(ISaveChangesDbContextAccessorAspect<,,,,,,>), ServiceCharacteristics.Singleton() },
                { typeof(IMigrateDbContextAccessorAspect<,,,,,,>), ServiceCharacteristics.Singleton() },
                
                // Mediators
                { typeof(AuditNotificationHandler<,>), ServiceCharacteristics.Transient() },
                { typeof(EntityNotificationHandler<,>), ServiceCharacteristics.Transient() },
                { typeof(MigrationNotificationHandler<,>), ServiceCharacteristics.Transient() },

                // Protectors
                { typeof(IPrivacyDataProtector), ServiceCharacteristics.Singleton() },

                // Services
                { typeof(IMigrationService<,,,,,,>), ServiceCharacteristics.Singleton() },
                { typeof(ITenantService<,,,,,,>), ServiceCharacteristics.Singleton() },

                // Stores
                { typeof(IStoreHub<,>), ServiceCharacteristics.Scoped() },
                { typeof(IStoreHub<,,,,,,>), ServiceCharacteristics.Scoped() },
                { typeof(IStoreIdentifierGenerator<>), ServiceCharacteristics.Singleton() },
                { typeof(IStoreInitializer<>), ServiceCharacteristics.Scoped() }
            };
        }

    }
}
