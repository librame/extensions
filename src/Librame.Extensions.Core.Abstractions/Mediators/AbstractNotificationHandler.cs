﻿#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

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
        /// 异步处理。
        /// </summary>
        /// <param name="notification">给定的通知。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个异步操作。</returns>
        public abstract Task HandleAsync(TNotification notification, CancellationToken cancellationToken = default);
    }
}
