using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Core.Tests
{
    using Mediators;

    public class Ping : IRequest<Pong>
    {
        public string Message { get; set; }
    }
    public class Pong
    {
        public string Message { get; set; }
    }

    public class PingHandler : IRequestHandler<Ping, Pong>
    {
        public Task<Pong> HandleAsync(Ping request, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(new Pong { Message = request.Message + " Pong" });
        }
    }

    public class PingPreProcessor : IRequestPreProcessor<Ping>
    {
        public Task ProcessAsync(Ping request, CancellationToken cancellationToken = default)
        {
            request.Message = request.Message + " Ping";

            return Task.CompletedTask;
        }
    }
    public class PingPongPostProcessor : IRequestPostProcessor<Ping, Pong>
    {
        public Task Process(Ping request, Pong response, CancellationToken cancellationToken)
        {
            response.Message = response.Message + " " + request.Message;

            return Task.CompletedTask;
        }
    }
}
