using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Xunit;

namespace Librame.Extensions.Core.Tests
{
    public class RequestPreProcessorBehaviorTests
    {
        [Fact]
        public async Task AllTest()
        {
            var mediator = TestServiceProvider.Current.GetRequiredService<IMediator>();

            var response = await mediator.Send(new Ping { Message = "Ping" });

            Assert.Contains("Ping Ping Pong", response.Message);
        }

    }
}
