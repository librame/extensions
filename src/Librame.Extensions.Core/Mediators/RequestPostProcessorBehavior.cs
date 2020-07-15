#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Core.Mediators
{
    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal class RequestPostProcessorBehavior<TRequest, TResponse> : IRequestPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IRequestPostProcessor<TRequest, TResponse>> _postProcessors;


        public RequestPostProcessorBehavior(IEnumerable<IRequestPostProcessor<TRequest, TResponse>> postProcessors)
        {
            _postProcessors = postProcessors.NotEmpty(nameof(postProcessors));
        }


        public async Task<TResponse> HandleAsync(TRequest request, RequestHandler<TResponse> nextHandler,
            CancellationToken cancellationToken = default)
        {
            nextHandler.NotNull(nameof(nextHandler));

            var response = await nextHandler.Invoke().ConfigureAwait();

            foreach (var post in _postProcessors)
                await post.Process(request, response, cancellationToken).ConfigureAwait();

            return response;
        }

    }
}
