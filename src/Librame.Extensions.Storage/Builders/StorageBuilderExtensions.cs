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
    using Extensions;
    using Extensions.Storage;

    /// <summary>
    /// 存储构建器静态扩展。
    /// </summary>
    public static class StorageBuilderExtensions
    {

        /// <summary>
        /// 添加存储。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IBuilder"/>。</param>
        /// <param name="configuration">给定的 <see cref="IConfiguration"/>。</param>
        /// <param name="configureOptions">给定的 <see cref="Action{IStorageBuilderOptions}"/>。</param>
        /// <returns>返回 <see cref="IStorageBuilder"/>。</returns>
        public static IStorageBuilder AddStorage(this IBuilder builder,
            IConfiguration configuration = null, Action<IStorageBuilderOptions> configureOptions = null)
        {
            return builder.AddStorage<DefaultStorageBuilderOptions>(configuration, configureOptions);
        }
        /// <summary>
        /// 添加存储。
        /// </summary>
        /// <typeparam name="TBuilderOptions">指定的构建器选项类型。</typeparam>
        /// <param name="builder">给定的 <see cref="IBuilder"/>。</param>
        /// <param name="configuration">给定的 <see cref="IConfiguration"/>。</param>
        /// <param name="configureOptions">给定的 <see cref="Action{TBuilderOptions}"/>。</param>
        /// <returns>返回 <see cref="IStorageBuilder"/>。</returns>
        public static IStorageBuilder AddStorage<TBuilderOptions>(this IBuilder builder,
            IConfiguration configuration = null, Action<TBuilderOptions> configureOptions = null)
            where TBuilderOptions : class, IStorageBuilderOptions
        {
            if (configuration.IsNotDefault())
                builder.Services.Configure<TBuilderOptions>(configuration);

            if (configureOptions.IsNotDefault())
                builder.Services.Configure(configureOptions);

            var storageBuilder = builder.AsStorageBuilder();

            storageBuilder.AddFileSystem();

            return storageBuilder;
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
