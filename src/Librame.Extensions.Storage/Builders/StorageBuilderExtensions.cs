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
        /// <param name="builderOptions">给定的 <see cref="StorageBuilderOptions"/>（可选）。</param>
        /// <param name="configuration">给定的 <see cref="IConfiguration"/>（可选）。</param>
        /// <param name="postConfigureOptions">给定的 <see cref="Action{StorageBuilderOptions}"/>（可选）。</param>
        /// <returns>返回 <see cref="IStorageBuilder"/>。</returns>
        public static IStorageBuilder AddStorage(this IBuilder builder, StorageBuilderOptions builderOptions = null,
            IConfiguration configuration = null, Action<StorageBuilderOptions> postConfigureOptions = null)
        {
            return builder.AddStorage<StorageBuilderOptions>(builderOptions ?? new StorageBuilderOptions(),
                configuration, postConfigureOptions);
        }
        /// <summary>
        /// 添加存储扩展。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IBuilder"/>。</param>
        /// <param name="builderOptions">给定的构建器选项。</param>
        /// <param name="configuration">给定的 <see cref="IConfiguration"/>（可选）。</param>
        /// <param name="postConfigureOptions">给定的 <see cref="Action{TBuilderOptions}"/>（可选）。</param>
        /// <returns>返回 <see cref="IStorageBuilder"/>。</returns>
        public static IStorageBuilder AddStorage<TBuilderOptions>(this IBuilder builder, TBuilderOptions builderOptions,
            IConfiguration configuration = null, Action<TBuilderOptions> postConfigureOptions = null)
            where TBuilderOptions : StorageBuilderOptions
        {
            return builder.AddBuilder(b =>
            {
                return b.AsStorageBuilder()
                    .AddFileSystem();
            },
            typeof(StorageBuilderOptions), builderOptions, configuration, postConfigureOptions);
        }


        /// <summary>
        /// 转换为内部存储构建器。
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
