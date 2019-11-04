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
    /// 请求管道行为接口。
    /// </summary>
    /// <typeparam name="TRequest">指定的请求类型。</typeparam>
    /// <typeparam name="TResponse">指定的响应类型。</typeparam>
    public interface IRequestPipelineBehavior<in TRequest, TResponse>
        where TRequest : IRequest
    {
        /// <summary>
        /// 异步管道处理程序。执行任何附加行为，并根据需要等待响应动作。
        /// </summary>
        /// <param name="request">给定传入的请求。</param>
        /// <param name="nextHandler">用于管道中的下一个操作的可等待委托。最终，这个委托表示处理程序。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <typeparamref name="TResponse"/> 的异步操作。</returns>
        Task<TResponse> HandleAsync(TRequest request, RequestHandler<TResponse> nextHandler, CancellationToken cancellationToken = default);
    }
}
