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

    class PhysicalFileService : ExtensionBuilderServiceBase<StorageBuilderOptions>, IFileService
    {
        private readonly IMemoryCache _memoryCache;


        public PhysicalFileService(IMemoryCache memoryCache,
            IOptions<StorageBuilderOptions> options, ILoggerFactory loggerFactory)
            : base(options, loggerFactory)
        {
            _memoryCache = memoryCache.NotNull(nameof(memoryCache));
        }


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
