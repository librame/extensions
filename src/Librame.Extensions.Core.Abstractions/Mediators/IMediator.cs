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
    /// 中介者接口。
    /// </summary>
    public interface IMediator
    {
        /// <summary>
        /// 异步地向单个处理程序发送请求。
        /// </summary>
        /// <typeparam name="TResponse">指定的响应类型。</typeparam>
        /// <param name="request">给定的 <see cref="IRequest{TResponse}"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <typeparamref name="TResponse"/> 的异步操作。</returns>
        Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);


        /// <summary>
        /// 异步向多个处理程序发送通知。
        /// </summary>
        /// <param name="notification">给定的通知对象。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个异步操作。</returns>
        Task Publish(object notification, CancellationToken cancellationToken = default);

        /// <summary>
        /// 异步向多个处理程序发送通知。
        /// </summary>
        /// <param name="notification">给定的 <see cref="INotification"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个异步操作。</returns>
        Task Publish(INotification notification, CancellationToken cancellationToken = default);
    }
}
