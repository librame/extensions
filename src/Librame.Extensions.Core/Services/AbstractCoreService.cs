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

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 抽象核心服务。
    /// </summary>
    public abstract class AbstractCoreService : AbstractService<CoreBuilderOptions>
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractService{TService}"/> 实例。
        /// </summary>
        /// <param name="options">给定的 <see cref="IOptions{CoreBuilderOptions}"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        public AbstractCoreService(IOptions<CoreBuilderOptions> options, ILoggerFactory loggerFactory)
            : base(options, loggerFactory)
        {
        }

    }
}
