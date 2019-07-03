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
    /// <typeparam name="TBuilderOptions">指定的构建器选项类型。</typeparam>
    public abstract class AbstractService<TBuilderOptions> : AbstractService, IService<TBuilderOptions>
        where TBuilderOptions : class, IBuilderOptions, new()
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractService{TBuilderOptions}"/> 实例。
        /// </summary>
        /// <param name="options">给定的 <see cref="IOptions{TBuilderOptions}"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        protected AbstractService(IOptions<TBuilderOptions> options, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            Options = options.NotNull(nameof(options)).Value;
        }


        /// <summary>
        /// 构建器选项。
        /// </summary>
        /// <value>返回 <typeparamref name="TBuilderOptions"/>。</value>
        public TBuilderOptions Options { get; }
    }


    /// <summary>
    /// 抽象服务。
    /// </summary>
    public abstract class AbstractService : AbstractDisposable, IService
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractService"/> 实例。
        /// </summary>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        protected AbstractService(ILoggerFactory loggerFactory)
        {
            LoggerFactory = loggerFactory.NotNull(nameof(loggerFactory));
        }


        /// <summary>
        /// 记录器工厂。
        /// </summary>
        /// <value>返回 <see cref="ILoggerFactory"/>。</value>
        public ILoggerFactory LoggerFactory { get; }

        /// <summary>
        /// 记录器。
        /// </summary>
        /// <value>返回 <see cref="ILogger"/>。</value>
        protected virtual ILogger Logger
            => LoggerFactory.CreateLogger(GetDisposableType());
    }
}
