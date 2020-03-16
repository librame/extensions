#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Storage.Services
{
    using Core.Services;
    using Storage.Builders;

    /// <summary>
    /// 文件服务。
    /// </summary>
    public class FileService : AbstractExtensionBuilderService<StorageBuilderOptions>, IFileService
    {
        private readonly IMemoryCache _memoryCache;


        /// <summary>
        /// 构造一个 <see cref="FileService"/>。
        /// </summary>
        /// <param name="memoryCache">给定的 <see cref="IMemoryCache"/>。</param>
        /// <param name="options">给定的 <see cref="IOptions{StorageBuilderOptions}"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        public FileService(IMemoryCache memoryCache,
            IOptions<StorageBuilderOptions> options, ILoggerFactory loggerFactory)
            : base(options, loggerFactory)
        {
            _memoryCache = memoryCache.NotNull(nameof(memoryCache));
        }


        /// <summary>
        /// 进度动作。
        /// </summary>
        public Action<StorageProgressDescriptor> ProgressAction { get; set; }


        private IStorageFileProvider GetFileProvider(Func<IEnumerable<IStorageFileProvider>, IStorageFileProvider> providerFactory = null)
        {
            if (Options.FileProviders.IsEmpty())
            {
                Logger.LogWarning($"Options.FileProviders is null or empty.");
                return null;
            }

            // 倒序优先
            var providers = Options.FileProviders.Reverse();
            return providerFactory?.Invoke(providers) ?? providers.First();
        }


        /// <summary>
        /// 异步获取文件信息。
        /// </summary>
        /// <param name="subpath">给定的子路径。</param>
        /// <param name="providerFactory">给定要使用的 <see cref="IStorageFileProvider"/> 工厂方法（可选）。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="Task{IStorageFileInfo}"/>。</returns>
        public Task<IStorageFileInfo> GetFileInfoAsync(string subpath,
            Func<IEnumerable<IStorageFileProvider>, IStorageFileProvider> providerFactory = null,
            CancellationToken cancellationToken = default)
        {
            var provider = GetFileProvider(providerFactory);
            if (provider.IsNull())
                return Task.FromResult((IStorageFileInfo)null);

            return cancellationToken.RunFactoryOrCancellationAsync(() => provider.GetFileInfo(subpath));
        }

        /// <summary>
        /// 异步获取目录内容集合。
        /// </summary>
        /// <param name="subpath">给定的子路径。</param>
        /// <param name="providerFactory">给定要使用的 <see cref="IStorageFileProvider"/> 工厂方法（可选）。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="Task{IStorageDirectoryContents}"/>。</returns>
        public Task<IStorageDirectoryContents> GetDirectoryContentsAsync(string subpath,
            Func<IEnumerable<IStorageFileProvider>, IStorageFileProvider> providerFactory = null,
            CancellationToken cancellationToken = default)
        {
            var provider = GetFileProvider(providerFactory);
            if (provider.IsNull())
                return Task.FromResult((IStorageDirectoryContents)null);

            return cancellationToken.RunFactoryOrCancellationAsync(() => provider.GetDirectoryContents(subpath));
        }


        /// <summary>
        /// 异步读取字符串。
        /// </summary>
        /// <param name="subpath">给定的子路径。</param>
        /// <param name="providerFactory">给定要使用的 <see cref="IStorageFileProvider"/> 工厂方法（可选）。</param>
        /// <returns>返回 <see cref="Task{String}"/>。</returns>
        public Task<string> ReadStringAsync(string subpath,
            Func<IEnumerable<IStorageFileProvider>, IStorageFileProvider> providerFactory = null)
        {
            var provider = GetFileProvider(providerFactory);
            if (provider.IsNull())
                return Task.FromResult((string)null);

            var fileInfo = provider.GetFileInfo(subpath);
            return _memoryCache.GetOrCreateAsync(fileInfo.PhysicalPath, entry =>
            {
                entry.AddExpirationToken(provider.Watch(subpath));
                return ReadStringAsync(fileInfo);
            });
        }

        /// <summary>
        /// 异步读取字符串。
        /// </summary>
        /// <param name="fileInfo">给定的 <see cref="IStorageFileInfo"/>。</param>
        /// <returns>返回 <see cref="Task{String}"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "fileInfo")]
        public Task<string> ReadStringAsync(IStorageFileInfo fileInfo)
        {
            fileInfo.NotNull(nameof(fileInfo));

            using (var readStream = fileInfo.CreateReadStream())
            using (var sr = new StreamReader(readStream))
            {
                return sr.ReadToEndAsync();
            }
        }

        /// <summary>
        /// 异步读取。
        /// </summary>
        /// <param name="fileInfo">给定的 <see cref="IStorageFileInfo"/>。</param>
        /// <param name="writeStream">给定的写入 <see cref="Stream"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="Task"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public async Task ReadAsync(IStorageFileInfo fileInfo, Stream writeStream, CancellationToken cancellationToken = default)
        {
            fileInfo.NotNull(nameof(fileInfo));
            writeStream.NotNull(nameof(writeStream));

            var buffer = new byte[Options.BufferSize];

            if (writeStream.CanSeek)
                writeStream.Seek(0, SeekOrigin.Begin);

            using (var readStream = fileInfo.CreateReadStream())
            {
                var currentCount = 1;
                while (currentCount > 0)
                {
                    // 每次从文件流中读取指定缓冲区的字节数，当读完后退出循环
                    currentCount = await readStream.ReadAsync(buffer, 0, buffer.Length, cancellationToken).ConfigureAndResultAsync();

                    // 将读取到的缓冲区字节数写入请求流
                    await writeStream.WriteAsync(buffer, 0, currentCount, cancellationToken).ConfigureAndWaitAsync();

                    if (ProgressAction.IsNotNull())
                    {
                        var descriptor = new StorageProgressDescriptor(
                        readStream.Length, readStream.Position,
                        writeStream.Length, writeStream.Position, currentCount);

                        ProgressAction.Invoke(descriptor);
                    }
                }
            }
        }


        /// <summary>
        /// 异步写入字符串。
        /// </summary>
        /// <param name="fileInfo">给定的 <see cref="IStorageFileInfo"/>。</param>
        /// <param name="content">给定的写入字符串。</param>
        /// <returns>返回 <see cref="Task"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "fileInfo")]
        public Task WriteStringAsync(IStorageFileInfo fileInfo, string content)
        {
            fileInfo.NotNull(nameof(fileInfo));
            content.NotEmpty(nameof(content));

            using (var readStream = fileInfo.CreateReadStream())
            using (var sw = new StreamWriter(readStream))
            {
                return sw.WriteAsync(content);
            }
        }

        /// <summary>
        /// 异步写入。
        /// </summary>
        /// <param name="fileInfo">给定的 <see cref="IStorageFileInfo"/>。</param>
        /// <param name="readStream">给定的读取 <see cref="Stream"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="Task"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public async Task WriteAsync(IStorageFileInfo fileInfo, Stream readStream, CancellationToken cancellationToken = default)
        {
            fileInfo.NotNull(nameof(fileInfo));
            readStream.NotNull(nameof(readStream));

            var buffer = new byte[Options.BufferSize];

            if (readStream.CanSeek)
                readStream.Seek(0, SeekOrigin.Begin);

            using (var writeStream = fileInfo.CreateWriteStream())
            {
                var currentCount = 1;
                while (currentCount > 0)
                {
                    // 每次从文件流中读取指定缓冲区的字节数，当读完后退出循环
                    currentCount = await readStream.ReadAsync(buffer, 0, buffer.Length, cancellationToken).ConfigureAndResultAsync();

                    // 将读取到的缓冲区字节数写入请求流
                    await writeStream.WriteAsync(buffer, 0, currentCount, cancellationToken).ConfigureAndWaitAsync();

                    if (ProgressAction.IsNotNull())
                    {
                        var descriptor = new StorageProgressDescriptor(
                        readStream.Length, readStream.Position,
                        writeStream.Length, writeStream.Position, currentCount);

                        ProgressAction.Invoke(descriptor);
                    }
                }
            }
        }
    }
}
