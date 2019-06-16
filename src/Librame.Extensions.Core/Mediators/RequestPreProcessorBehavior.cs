#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 请求前置处理器行为。
    /// </summary>
    public class RequestPreProcessorBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IEnumerable<IRequestPreProcessor<TRequest>> _preProcessors;


        /// <summary>
        /// 构造一个 <see cref="RequestPostProcessorBehavior{TRequest, TResponse}"/> 实例。
        /// </summary>
        /// <param name="preProcessors">给定的请求前置处理器可枚举集合。</param>
        public RequestPreProcessorBehavior(IEnumerable<IRequestPreProcessor<TRequest>> preProcessors)
        {
            _preProcessors = preProcessors.NotNullOrEmpty(nameof(preProcessors));
        }


        /// <summary>
        /// 异步管道处理程序。执行任何附加行为，并根据需要等待响应动作。
        /// </summary>
        /// <param name="request">给定传入的请求。</param>
        /// <param name="next">用于管道中的下一个操作的可等待委托。最终，这个委托表示处理程序。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <typeparamref name="TResponse"/> 的异步操作。</returns>
        public async Task<TResponse> HandleAsync(TRequest request, RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken = default)
        {
            foreach (var processor in _preProcessors)
            {
                await processor.ProcessAsync(request, cancellationToken).ConfigureAwait(false);
            }

            return await next().ConfigureAwait(false);
        }

    }
}
