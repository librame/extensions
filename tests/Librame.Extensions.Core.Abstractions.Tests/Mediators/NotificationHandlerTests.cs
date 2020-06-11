using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Librame.Extensions.Core.Tests
{
    using Mediators;

    public class NotificationHandlerTests
    {
        [Fact]
        public async Task AllTest()
        {
            var builder = new StringBuilder();
            var writer = new StringWriter(builder);

            INotificationHandler<PingNotification> handler = new PingNotificationHandler(writer);

            await handler.HandleAsync(new PingNotification()
            {
                Message = "Ping"
            })
            .ConfigureAndWaitAsync();

            Assert.Contains("Ping Pong", builder.ToString());
        }

    }
}
