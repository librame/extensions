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
    /// <typeparam name="TService">指定的服务类型。</typeparam>
    public class AbstractDrawingService<TService> : AbstractService<TService>
        where TService : class, IService
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractDrawingService{TService}"/> 实例。
        /// </summary>
        /// <param name="options">给定的 <see cref="IOptions{DrawingBuilderOptions}"/>。</param>
        /// <param name="logger">给定的 <see cref="ILogger{TService}"/>。</param>
        protected AbstractDrawingService(IOptions<DrawingBuilderOptions> options,
            ILogger<TService> logger)
            : base(logger)
        {
            Options = options.NotNull(nameof(options)).Value;
        }


        /// <summary>
        /// 构建器选项。
        /// </summary>
        public DrawingBuilderOptions Options { get; }
    }
}
