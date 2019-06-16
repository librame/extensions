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
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 内部通知处理程序封装。
    /// </summary>
    /// <typeparam name="TNotification">指定的通知类型。</typeparam>
    internal class InternalNotificationHandlerWrapper<TNotification> : AbstractNotificationHandlerWrapper
        where TNotification : INotification
    {
        /// <summary>
        /// 异步处理。
        /// </summary>
        /// <param name="notification">给定的 <see cref="INotification"/>。</param>
        /// <param name="serviceFactory">给定的 <see cref="ServiceFactoryDelegate"/>。</param>
        /// <param name="publishFactory">给定的发布工厂方法。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个异步操作。</returns>
        public override Task HandleAsync(INotification notification, ServiceFactoryDelegate serviceFactory,
            Func<IEnumerable<Func<Task>>, Task> publishFactory, CancellationToken cancellationToken = default)
        {
            var handlers = serviceFactory
                .Invokes<INotificationHandler<TNotification>>()
                .Select(x => new Func<Task>(() => x.HandleAsync((TNotification)notification, cancellationToken)));

            return publishFactory.Invoke(handlers);
        }

    }
}
