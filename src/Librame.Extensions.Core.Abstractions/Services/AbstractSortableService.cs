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

namespace Librame.Extensions.Core.Services
{
    /// <summary>
    /// 抽象可排序服务。
    /// </summary>
    public abstract class AbstractSortableService : AbstractSortable, ISortableService
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractSortableService"/>。
        /// </summary>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        /// <param name="priority">给定的服务优先级（数值越小越优先）。</param>
        protected AbstractSortableService(ILoggerFactory loggerFactory, float? priority = null)
            : base(priority)
        {
            LoggerFactory = loggerFactory.NotNull(nameof(loggerFactory));
        }


        /// <summary>
        /// 日志工厂。
        /// </summary>
        /// <value>返回 <see cref="ILoggerFactory"/>。</value>
        public ILoggerFactory LoggerFactory { get; }

        /// <summary>
        /// 日志。
        /// </summary>
        /// <value>返回 <see cref="ILogger"/>。</value>
        protected virtual ILogger Logger
            => LoggerFactory.CreateLogger(GetType());
    }
}
