using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Xunit;

namespace Librame.Extensions.Core.Tests
{
    public class RequestPostProcessorTests
    {
        [Fact]
        public async Task AllTest()
        {
            var mediator = TestServiceProvider.Current.GetRequiredService<IMediator>();

            var response = await mediator.Send(new Ping { Message = "Ping" });
            
            Assert.Contains("Ping Pong Ping", response.Message);
        }

    }
}
