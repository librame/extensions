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

namespace Librame.Extensions.Core.Mediators
{
    /// <summary>
    /// 请求前置处理程序接口。
    /// </summary>
    /// <typeparam name="TRequest">指定的请求类型。</typeparam>
    public interface IRequestPreProcessor<in TRequest>
        where TRequest : IRequest
    {
        /// <summary>
        /// 异步执行。
        /// </summary>
        /// <param name="request">给定的 <typeparamref name="TRequest"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个异步操作。</returns>
        Task ProcessAsync(TRequest request, CancellationToken cancellationToken = default);
    }
}
