using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Librame.Extensions.Core.Tests
{
    using Mediators;

    public class RequestHandlerTests
    {
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
                return Task.Run(() => new Pong { Message = request.Message + " Pong" });
            }
        }


        [Fact]
        public async Task AllTest()
        {
            IRequestHandler<Ping, Pong> handler = new PingHandler();

            var response = await handler.HandleAsync(new Ping() { Message = "Ping" }).ConfigureAndResultAsync();

            Assert.Contains("Ping Pong", response.Message);
        }

    }
}
