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
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Storage
{
    using Core;

    /// <summary>
    /// 内部物理文件服务。
    /// </summary>
    internal class InternalPhysicalFileService : ExtensionBuilderServiceBase<StorageBuilderOptions>, IFileService
    {
        private readonly IMemoryCache _memoryCache;


        /// <summary>
        /// 构造一个 <see cref="InternalPhysicalFileService"/> 实例。
        /// </summary>
        /// <param name="memoryCache">给定的 <see cref="IMemoryCache"/>。</param>
        /// <param name="options">给定的 <see cref="IOptions{StorageBuilderOptions}"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        public InternalPhysicalFileService(IMemoryCache memoryCache,
            IOptions<StorageBuilderOptions> options, ILoggerFactory loggerFactory)
            : base(options, loggerFactory)
        {
            _memoryCache = memoryCache.NotNull(nameof(memoryCache));
        }



        /// <summary>
        /// 异步获取内容。
        /// </summary>
        /// <param name="root">给定的根。</param>
        /// <param name="subpath">给定的子路径。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含字符串的异步操作。</returns>
        public Task<string> GetContentAsync(string root, string subpath, CancellationToken cancellationToken = default)
        {
            var key = root.CombinePath(subpath);

            return _memoryCache.GetOrCreateAsync(key, entry =>
            {
                var provider = new PhysicalFileProvider(root);

                entry.AddExpirationToken(provider.Watch(subpath));

                var fileInfo = provider.GetFileInfo(subpath);
                return fileInfo.Exists
                    ? cancellationToken.RunFactoryOrCancellationAsync(() => File.ReadAllText(fileInfo.PhysicalPath))
                    : cancellationToken.RunFactoryOrCancellationAsync(() => string.Empty);
            });
        }

        /// <summary>
        /// 异步获取提供程序。
        /// </summary>
        /// <param name="root">给定的根。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <see cref="IFileProvider"/> 的异步操作。</returns>
        public Task<IFileProvider> GetProviderAsync(string root, CancellationToken cancellationToken = default)
        {
            return cancellationToken.RunFactoryOrCancellationAsync(() =>
            {
                IFileProvider provider = new PhysicalFileProvider(root);
                return provider;
            });
        }

    }
}
