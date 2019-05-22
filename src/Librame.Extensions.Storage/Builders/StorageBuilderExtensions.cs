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

namespace Librame.Extensions.Storage
{
    using Core;

    /// <summary>
    /// 存储构建器静态扩展。
    /// </summary>
    public static class StorageBuilderExtensions
    {
        /// <summary>
        /// 添加存储扩展。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IBuilder"/>。</param>
        /// <param name="configureOptions">给定的 <see cref="Action{StorageBuilderOptions}"/>（可选）。</param>
        /// <returns>返回 <see cref="IStorageBuilder"/>。</returns>
        public static IStorageBuilder AddStorage(this IBuilder builder,
            Action<StorageBuilderOptions> configureOptions = null)
        {
            // Configure Options
            if (configureOptions != null)
                builder.Services.Configure(configureOptions);

            var storageBuilder = new InternalStorageBuilder(builder);

            return storageBuilder
                .AddServices();
        }

    }
}
