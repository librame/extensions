#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Librame.Builders
{
    using Extensions.Storage;

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
        /// <param name="configuration">给定的 <see cref="IConfiguration"/>（可选）。</param>
        /// <returns>返回 <see cref="IStorageBuilder"/>。</returns>
        public static IStorageBuilder AddStorage(this IBuilder builder,
            Action<StorageBuilderOptions> configureOptions = null, IConfiguration configuration = null)
        {
            return builder.AddBuilder(configureOptions, configuration, _builder =>
            {
                return _builder.AsStorageBuilder()
                    .AddFileSystem();
            });
        }


        /// <summary>
        /// 转换为存储构建器。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IBuilder"/>。</param>
        /// <returns>返回 <see cref="IStorageBuilder"/>。</returns>
        public static IStorageBuilder AsStorageBuilder(this IBuilder builder)
        {
            return new InternalStorageBuilder(builder);
        }

        /// <summary>
        /// 添加文件系统。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IStorageBuilder"/>。</param>
        /// <returns>返回 <see cref="IStorageBuilder"/>。</returns>
        public static IStorageBuilder AddFileSystem(this IStorageBuilder builder)
        {
            builder.Services.AddSingleton<IFileReader, InternalFileReader>();
            builder.Services.AddSingleton<IFileWriter, InternalFileWriter>();
            builder.Services.AddSingleton<IFileSystemService, InternalFileSystemService>();

            return builder;
        }

    }
}
