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

namespace Librame.Extensions.Storage.Builders
{
    using Core.Builders;
    using Core.Services;
    using Storage.Services;

    /// <summary>
    /// 存储构建器。
    /// </summary>
    public class StorageBuilder : AbstractExtensionBuilder, IStorageBuilder
    {
        /// <summary>
        /// 构造一个 <see cref="StorageBuilder"/>。
        /// </summary>
        /// <param name="parentBuilder">给定的 <see cref="IExtensionBuilder"/>。</param>
        /// <param name="dependency">给定的 <see cref="StorageBuilderDependency"/>。</param>
        public StorageBuilder(IExtensionBuilder parentBuilder, StorageBuilderDependency dependency)
            : base(parentBuilder, dependency)
        {
            Services.AddSingleton<IStorageBuilder>(this);

            AddStorageServices();
        }


        private void AddStorageServices()
        {
            // Services
            AddService<IFileService, FileService>();
            AddService<IFileTransferService, FileTransferService>();
            AddService<IFilePermissionService, FilePermissionService>();
        }


        /// <summary>
        /// 获取服务特征。
        /// </summary>
        /// <param name="serviceType">给定的服务类型。</param>
        /// <returns>返回 <see cref="ServiceCharacteristics"/>。</returns>
        public override ServiceCharacteristics GetServiceCharacteristics(Type serviceType)
            => StorageBuilderServiceCharacteristicsRegistration.Register.GetOrDefault(serviceType);

    }
}
