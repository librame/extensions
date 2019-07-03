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

namespace Librame.Extensions.Data
{
    using Core;

    /// <summary>
    /// 抽象数据服务。
    /// </summary>
    public abstract class AbstractDataService : AbstractService<DataBuilderOptions>
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractDataService"/> 实例。
        /// </summary>
        /// <param name="options">给定的 <see cref="IOptions{DataBuilderOptions}"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        public AbstractDataService(IOptions<DataBuilderOptions> options, ILoggerFactory loggerFactory)
            : base(options, loggerFactory)
        {
        }
    }
}
