#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Librame.Extensions.Storage
{
    using Core;

    /// <summary>
    /// 抽象存储服务。
    /// </summary>
    public abstract class AbstractStorageService : AbstractService<StorageBuilderOptions>
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractStorageService"/> 实例。
        /// </summary>
        /// <param name="options">给定的 <see cref="IOptions{StorageBuilderOptions}"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        public AbstractStorageService(IOptions<StorageBuilderOptions> options, ILoggerFactory loggerFactory)
            : base(options, loggerFactory)
        {
        }

    }
}
