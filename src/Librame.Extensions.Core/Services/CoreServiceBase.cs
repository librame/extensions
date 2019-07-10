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
    /// 核心服务基类。
    /// </summary>
    public class CoreServiceBase : AbstractService<CoreBuilderOptions>
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractService{TService}"/> 实例。
        /// </summary>
        /// <param name="options">给定的 <see cref="IOptions{CoreBuilderOptions}"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        public CoreServiceBase(IOptions<CoreBuilderOptions> options, ILoggerFactory loggerFactory)
            : base(options, loggerFactory)
        {
        }

    }
}
