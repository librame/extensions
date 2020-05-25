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
    /// <summary>
    /// 请求后置处理程序接口。
    /// </summary>
    /// <typeparam name="TRequest">指定的请求类型。</typeparam>
    /// <typeparam name="TResponse">指定的响应类型。</typeparam>
    public interface IRequestPostProcessor<in TRequest, in TResponse>
        where TRequest : IRequest<TResponse>
    {
        /// <summary>
        /// 进程方法在处理程序上的句柄方法之后执行。
        /// </summary>
        /// <param name="request">给定的 <typeparamref name="TRequest"/>。</param>
        /// <param name="response">给定的 <typeparamref name="TResponse"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个异步操作。</returns>
        Task Process(TRequest request, TResponse response, CancellationToken cancellationToken = default);
    }
}
