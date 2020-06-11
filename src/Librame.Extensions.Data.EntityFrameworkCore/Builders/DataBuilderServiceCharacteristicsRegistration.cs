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
using System.Collections.Generic;

namespace Librame.Extensions.Data.Builders
{
    using Core.Services;
    using Data.Aspects;
    using Data.Mediators;
    using Data.Protectors;
    using Data.Services;
    using Data.Stores;
    using Data.ValueGenerators;

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
                { typeof(ISaveChangesAccessorAspect<,>), ServiceCharacteristics.Singleton() },
                { typeof(IMigrateAccessorAspect<,>), ServiceCharacteristics.Singleton() },
                
                // Mediators
                { typeof(AuditNotificationHandler<,>), ServiceCharacteristics.Transient() },
                { typeof(EntityNotificationHandler<>), ServiceCharacteristics.Transient() },
                { typeof(MigrationNotificationHandler<>), ServiceCharacteristics.Transient() },

                // Protectors
                { typeof(IPrivacyDataProtector), ServiceCharacteristics.Singleton() },

                // Services
                { typeof(IMigrationAccessorService), ServiceCharacteristics.Singleton() },
                { typeof(IMultiTenantAccessorService), ServiceCharacteristics.Singleton() },

                // Stores
                { typeof(IStoreHub<>), ServiceCharacteristics.Scoped() },
                { typeof(IStoreIdentifierGenerator<>), ServiceCharacteristics.Singleton() },
                { typeof(IStoreInitializer<>), ServiceCharacteristics.Scoped() },

                // ValueGenerators
                { typeof(IDefaultValueGenerator<>), ServiceCharacteristics.Singleton() }
            };
        }

    }
}
