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

namespace Librame.Extensions.Storage.Builders
{
    using Core.Services;
    using Storage.Services;

    /// <summary>
    /// <see cref="IStorageBuilder"/> 服务特征注册。
    /// </summary>
    public static class StorageBuilderServiceCharacteristicsRegistration
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
                // Services
                { typeof(IFileService), ServiceCharacteristics.Singleton() },
                { typeof(IFileTransferService), ServiceCharacteristics.Singleton() },
                { typeof(IFilePermissionService), ServiceCharacteristics.Singleton() }
            };
        }

    }
}
