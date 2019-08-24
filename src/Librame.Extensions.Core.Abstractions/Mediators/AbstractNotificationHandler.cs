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
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 抽象通知处理程序。
    /// </summary>
    /// <typeparam name="TNotification">指定的通知类型。</typeparam>
    public abstract class AbstractNotificationHandler<TNotification> : INotificationHandler<TNotification>
        where TNotification : INotification
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractNotificationHandler{TNotification}"/>。
        /// </summary>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        public AbstractNotificationHandler(ILoggerFactory loggerFactory)
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
            => LoggerFactory.CreateLogger(GetType());


        /// <summary>
        /// 异步处理通知。
        /// </summary>
        /// <param name="notification">给定的通知。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个异步操作。</returns>
        public abstract Task HandleAsync(TNotification notification, CancellationToken cancellationToken = default);
    }
}
