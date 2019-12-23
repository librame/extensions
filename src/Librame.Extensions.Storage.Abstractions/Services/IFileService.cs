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
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Storage.Services
{
    using Core.Services;

    /// <summary>
    /// 文件服务接口。
    /// </summary>
    public interface IFileService : IService
    {
        /// <summary>
        /// 进度动作。
        /// </summary>
        Action<StorageProgressDescriptor> ProgressAction { get; set; }


        /// <summary>
        /// 异步获取文件信息。
        /// </summary>
        /// <param name="subpath">给定的子路径。</param>
        /// <param name="providerFactory">给定要使用的 <see cref="IStorageFileProvider"/> 工厂方法（可选）。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="Task{IStorageFileInfo}"/>。</returns>
        Task<IStorageFileInfo> GetFileInfoAsync(string subpath,
            Func<IEnumerable<IStorageFileProvider>, IStorageFileProvider> providerFactory = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// 异步获取目录内容集合。
        /// </summary>
        /// <param name="subpath">给定的子路径。</param>
        /// <param name="providerFactory">给定要使用的 <see cref="IStorageFileProvider"/> 工厂方法（可选）。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="Task{IStorageDirectoryContents}"/>。</returns>
        Task<IStorageDirectoryContents> GetDirectoryContentsAsync(string subpath,
            Func<IEnumerable<IStorageFileProvider>, IStorageFileProvider> providerFactory = null,
            CancellationToken cancellationToken = default);


        /// <summary>
        /// 异步读取字符串。
        /// </summary>
        /// <param name="subpath">给定的子路径。</param>
        /// <param name="providerFactory">给定要使用的 <see cref="IStorageFileProvider"/> 工厂方法（可选）。</param>
        /// <returns>返回 <see cref="Task{String}"/>。</returns>
        Task<string> ReadStringAsync(string subpath,
            Func<IEnumerable<IStorageFileProvider>, IStorageFileProvider> providerFactory = null);

        /// <summary>
        /// 异步读取字符串。
        /// </summary>
        /// <param name="fileInfo">给定的 <see cref="IStorageFileInfo"/>。</param>
        /// <returns>返回 <see cref="Task{String}"/>。</returns>
        Task<string> ReadStringAsync(IStorageFileInfo fileInfo);

        /// <summary>
        /// 异步读取。
        /// </summary>
        /// <param name="fileInfo">给定的 <see cref="IStorageFileInfo"/>。</param>
        /// <param name="writeStream">给定的写入 <see cref="Stream"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="Task"/>。</returns>
        Task ReadAsync(IStorageFileInfo fileInfo, Stream writeStream, CancellationToken cancellationToken = default);


        /// <summary>
        /// 异步写入字符串。
        /// </summary>
        /// <param name="fileInfo">给定的 <see cref="IStorageFileInfo"/>。</param>
        /// <param name="content">给定的写入字符串。</param>
        /// <returns>返回 <see cref="Task"/>。</returns>
        Task WriteStringAsync(IStorageFileInfo fileInfo, string content);

        /// <summary>
        /// 异步写入。
        /// </summary>
        /// <param name="fileInfo">给定的 <see cref="IStorageFileInfo"/>。</param>
        /// <param name="readStream">给定的读取 <see cref="Stream"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="Task"/>。</returns>
        Task WriteAsync(IStorageFileInfo fileInfo, Stream readStream, CancellationToken cancellationToken = default);
    }
}
