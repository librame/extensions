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
    /// 存储服务基类。
    /// </summary>
    public class StorageServiceBase : AbstractService<StorageBuilderOptions>
    {
        /// <summary>
        /// 构造一个 <see cref="StorageServiceBase"/> 实例。
        /// </summary>
        /// <param name="options">给定的 <see cref="IOptions{StorageBuilderOptions}"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        public StorageServiceBase(IOptions<StorageBuilderOptions> options,
            ILoggerFactory loggerFactory)
            : base(options, loggerFactory)
        {
        }

    }
}
