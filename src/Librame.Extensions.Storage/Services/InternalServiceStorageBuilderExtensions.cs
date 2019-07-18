﻿#region License

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
    /// <summary>
    /// 内部服务存储构建器静态扩展。
    /// </summary>
    internal static class InternalServiceStorageBuilderExtensions
    {
        /// <summary>
        /// 添加服务集合。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IStorageBuilder"/>。</param>
        /// <returns>返回 <see cref="IStorageBuilder"/>。</returns>
        public static IStorageBuilder AddServices(this IStorageBuilder builder)
        {
            builder.Services.AddScoped<IFileService, InternalPhysicalFileService>();
            builder.Services.AddScoped<IFileTransferService, InternalFileTransferService>();
            builder.Services.AddScoped<IFilePermissionService, InternalFilePermissionService>();

            return builder;
        }

    }
}