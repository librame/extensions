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
using System.Threading.Tasks;

namespace Librame.Extensions.Storage
{
    using Core;

    /// <summary>
    /// 内部物理文件服务。
    /// </summary>
    internal class InternalPhysicalFileService : AbstractService<InternalPhysicalFileService>, IFileService
    {
        /// <summary>
        /// 构造一个 <see cref="InternalPhysicalFileService"/> 实例。
        /// </summary>
        /// <param name="logger">给定的 <see cref="ILogger{PhysicalFileService}"/>。</param>
        public InternalPhysicalFileService(ILogger<InternalPhysicalFileService> logger)
            : base(logger)
        {
        }


        /// <summary>
        /// 异步获取提供程序。
        /// </summary>
        /// <param name="root">给定的根。</param>
        /// <returns>返回一个包含 <see cref="IFileProvider"/> 的异步操作。</returns>
        public Task<IFileProvider> GetProviderAsync(string root)
        {
            IFileProvider provider = new PhysicalFileProvider(root);

            return Task.FromResult(provider);
        }

    }
}
