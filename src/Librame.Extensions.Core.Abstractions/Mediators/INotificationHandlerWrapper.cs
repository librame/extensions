#region License

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
    /// 通知处理程序封装接口。
    /// </summary>
    /// <typeparam name="TNotification">指定的通知类型。</typeparam>
    public interface INotificationHandlerWrapper<in TNotification> : INotificationHandlerWrapper
        where TNotification : INotification
    {
        /// <summary>
        /// 异步处理通知。
        /// </summary>
        /// <param name="notification">给定的 <see cref="INotification"/>。</param>
        /// <param name="serviceFactory">给定的 <see cref="ServiceFactoryDelegate"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个异步操作。</returns>
        Task HandleAsync(INotification notification, ServiceFactoryDelegate serviceFactory,
            CancellationToken cancellationToken = default);
    }


    /// <summary>
    /// 通知处理程序封装接口。
    /// </summary>
    public interface INotificationHandlerWrapper
    {
    }
}
