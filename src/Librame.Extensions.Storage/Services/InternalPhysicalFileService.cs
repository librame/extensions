#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Storage
{
    /// <summary>
    /// 内部物理文件服务。
    /// </summary>
    internal class InternalPhysicalFileService : AbstractStorageService, IFileService
    {
        /// <summary>
        /// 构造一个 <see cref="InternalPhysicalFileService"/> 实例。
        /// </summary>
        /// <param name="options">给定的 <see cref="IOptions{StorageBuilderOptions}"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        public InternalPhysicalFileService(IOptions<StorageBuilderOptions> options,
            ILoggerFactory loggerFactory)
            : base(options, loggerFactory)
        {
        }


        /// <summary>
        /// 异步获取提供程序。
        /// </summary>
        /// <param name="root">给定的根。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <returns>返回一个包含 <see cref="IFileProvider"/> 的异步操作。</returns>
        public Task<IFileProvider> GetProviderAsync(string root, CancellationToken cancellationToken)
        {
            return cancellationToken.RunFactoryOrCancellationAsync(() =>
            {
                IFileProvider provider = new PhysicalFileProvider(root);
                return provider;
            });
        }

    }
}
