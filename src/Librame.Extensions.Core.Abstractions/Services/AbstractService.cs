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
    /// 抽象服务。
    /// </summary>
    /// <typeparam name="TService">指定的服务类型。</typeparam>
    /// <typeparam name="TBuilderOptions">指定的构建器选项类型。</typeparam>
    public abstract class AbstractService<TService, TBuilderOptions> : AbstractService<TService>, IService<TBuilderOptions>
        where TService : class, IService
        where TBuilderOptions : class, IBuilderOptions, new()
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractService{TService}"/> 实例。
        /// </summary>
        /// <param name="options">给定的 <see cref="IOptions{TBuilderOptions}"/>。</param>
        /// <param name="logger">给定的 <see cref="ILogger{TService}"/>。</param>
        protected AbstractService(IOptions<TBuilderOptions> options, ILogger<TService> logger)
            : base(logger)
        {
            Options = options.NotNull(nameof(options)).Value;
        }


        /// <summary>
        /// 构建器选项。
        /// </summary>
        public TBuilderOptions Options { get; }
    }


    /// <summary>
    /// 抽象服务。
    /// </summary>
    /// <typeparam name="TService">指定的服务类型。</typeparam>
    public abstract class AbstractService<TService> : AbstractDisposable<TService>, IService
        where TService : class, IService
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractService{TService}"/> 实例。
        /// </summary>
        /// <param name="logger">给定的 <see cref="ILogger{TService}"/>。</param>
        protected AbstractService(ILogger<TService> logger)
        {
            Logger = logger.NotNull(nameof(logger));
        }


        /// <summary>
        /// 记录器。
        /// </summary>
        /// <value>返回 <see cref="ILogger"/>。</value>
        protected ILogger Logger { get; }
    }
}
