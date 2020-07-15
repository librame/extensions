using System.Threading.Tasks;
using Xunit;

namespace Librame.Extensions.Core.Tests
{
    using Mediators;

    public class RequestHandlerTests
    {
        [Fact]
        public async Task AllTest()
        {
            IRequestHandler<PingRequest, Pong> handler = new PingRequestHandler();

            var response = await handler.HandleAsync(new PingRequest()
            {
                Message = "Ping"
            })
            .ConfigureAwait();

            Assert.Contains("Ping Pong", response.Message);
        }

    }
}
