using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Librame.Extensions.Core.Tests
{
    public class AbstractRequestHandlerTests
    {
        public class Ping : IRequest<Pong>
        {
            public string Message { get; set; }
        }

        public class Pong
        {
            public string Message { get; set; }
        }

        public class PingHandler : AbstractRequestHandler<Ping, Pong>
        {
            public override Task<Pong> HandleAsync(Ping request, CancellationToken cancellationToken = default)
            {
                return Task.Factory.StartNew(() => new Pong { Message = request.Message + " Pong" });
            }
        }


        [Fact]
        public async Task AllTest()
        {
            IRequestHandler<Ping, Pong> handler = new PingHandler();

            var response = await handler.HandleAsync(new Ping() { Message = "Ping" });

            Assert.Contains("Ping Pong", response.Message);
        }

    }
}
