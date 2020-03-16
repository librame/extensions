#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Librame.Extensions.Storage.Builders
{
    using Services;

    static class ServiceStorageBuilderExtensions
    {
        internal static IStorageBuilder AddServices(this IStorageBuilder builder)
        {
            builder.Services.TryAddSingleton<IFileService, FileService>();
            builder.Services.TryAddSingleton<IFileTransferService, FileTransferService>();
            builder.Services.TryAddSingleton<IFilePermissionService, FilePermissionService>();

            return builder;
        }

    }
}
