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

namespace Librame.Extensions.Drawing
{
    using Core;

    /// <summary>
    /// 抽象图画服务。
    /// </summary>
    public abstract class AbstractDrawingService : AbstractService<DrawingBuilderOptions>
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractDrawingService"/> 实例。
        /// </summary>
        /// <param name="options">给定的 <see cref="IOptions{DrawingBuilderOptions}"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        public AbstractDrawingService(IOptions<DrawingBuilderOptions> options, ILoggerFactory loggerFactory)
            : base(options, loggerFactory)
        {
        }
    }
}
