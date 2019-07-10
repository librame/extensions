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
    /// 图画服务基类。
    /// </summary>
    public class DrawingServiceBase : AbstractService<DrawingBuilderOptions>
    {
        /// <summary>
        /// 构造一个 <see cref="DrawingServiceBase"/> 实例。
        /// </summary>
        /// <param name="options">给定的 <see cref="IOptions{DrawingBuilderOptions}"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        public DrawingServiceBase(IOptions<DrawingBuilderOptions> options, ILoggerFactory loggerFactory)
            : base(options, loggerFactory)
        {
        }
    }
}
