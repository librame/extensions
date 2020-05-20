#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
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

    internal class StorageBuilder : AbstractExtensionBuilder, IStorageBuilder
    {
        public StorageBuilder(IExtensionBuilder parentBuilder, StorageBuilderDependency dependency)
            : base(parentBuilder, dependency)
        {
            Services.AddSingleton<IStorageBuilder>(this);

            AddStorageServices();
        }


        public override ServiceCharacteristics GetServiceCharacteristics(Type serviceType)
            => StorageBuilderServiceCharacteristicsRegistration.Register.GetOrDefault(serviceType);


        private void AddStorageServices()
        {
            // Services
            AddService<IFileService, FileService>();
            AddService<IFileTransferService, FileTransferService>();
            AddService<IFilePermissionService, FilePermissionService>();
        }

    }
}
