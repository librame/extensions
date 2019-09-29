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
    class RequestPreProcessorBehavior<TRequest, TResponse> : IRequestPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest
    {
        private readonly IEnumerable<IRequestPreProcessor<TRequest>> _preProcessors;


        public RequestPreProcessorBehavior(IEnumerable<IRequestPreProcessor<TRequest>> preProcessors)
        {
            _preProcessors = preProcessors.NotEmpty(nameof(preProcessors));
        }


        public async Task<TResponse> HandleAsync(TRequest request, RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken = default)
        {
            next.NotNull(nameof(next));

            foreach (var pre in _preProcessors)
                await pre.ProcessAsync(request, cancellationToken).ConfigureAndWaitAsync();

            return await next.Invoke().ConfigureAndResultAsync();
        }

    }
}
