using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Xunit;

namespace Librame.Extensions.Core.Tests
{
    using Mediators;

    public class RequestPostProcessorBehaviorTests
    {
        [Fact]
        public async Task AllTest()
        {
            var mediator = TestServiceProvider.Current.GetRequiredService<IMediator>();
            var response = await mediator.Send(new Ping { Message = "Ping" }).ConfigureAwait();
            Assert.Contains("Ping Ping Pong", response.Message);
        }

    }
}
