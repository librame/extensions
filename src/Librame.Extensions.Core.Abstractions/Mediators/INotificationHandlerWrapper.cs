#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Core.Mediators
{
    using Services;

    /// <summary>
    /// 通知处理程序封装接口。
    /// </summary>
    /// <typeparam name="TNotification">指定的通知类型。</typeparam>
    public interface INotificationHandlerWrapper<in TNotification> : INotificationHandlerWrapperIndication
        where TNotification : INotificationIndication
    {
        /// <summary>
        /// 异步处理通知。
        /// </summary>
        /// <param name="notification">给定的 <see cref="INotificationIndication"/>。</param>
        /// <param name="serviceFactory">给定的 <see cref="ServiceFactory"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个异步操作。</returns>
        Task HandleAsync(INotificationIndication notification, ServiceFactory serviceFactory,
            CancellationToken cancellationToken = default);
    }
}
