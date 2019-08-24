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

namespace Librame.Extensions.Storage
{
    static class ServiceStorageBuilderExtensions
    {
        public static IStorageBuilder AddServices(this IStorageBuilder builder)
        {
            builder.Services.AddScoped<IFileService, PhysicalFileService>();
            builder.Services.AddScoped<IFileTransferService, FileTransferService>();
            builder.Services.AddScoped<IFilePermissionService, FilePermissionService>();

            return builder;
        }

    }
}
