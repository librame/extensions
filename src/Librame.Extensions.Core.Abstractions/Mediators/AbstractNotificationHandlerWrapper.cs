#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 抽象通知处理程序封装。
    /// </summary>
    public abstract class AbstractNotificationHandlerWrapper
    {
        /// <summary>
        /// 异步处理。
        /// </summary>
        /// <param name="notification">给定的 <see cref="INotification"/>。</param>
        /// <param name="serviceFactory">给定的 <see cref="ServiceFactoryDelegate"/>。</param>
        /// <param name="publishFactory">给定的发布工厂方法。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个异步操作。</returns>
        public abstract Task HandleAsync(INotification notification, ServiceFactoryDelegate serviceFactory,
            Func<IEnumerable<Func<Task>>, Task> publishFactory, CancellationToken cancellationToken = default);
    }
}
