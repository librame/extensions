using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Librame.Extensions.Core.Tests
{
    public class NotificationHandlerTests
    {
        public class Ping : INotification
        {
            public string Message { get; set; }
        }

        public class PongChildHandler : INotificationHandler<Ping>
        {
            private readonly TextWriter _writer;

            public PongChildHandler(TextWriter writer)
            {
                _writer = writer;
            }

            public Task HandleAsync(Ping notification, CancellationToken cancellationToken = default)
            {
                _writer.WriteLine(notification.Message + " Pong");

                return Task.CompletedTask;
            }
        }


        [Fact]
        public async Task AllTest()
        {
            var builder = new StringBuilder();
            var writer = new StringWriter(builder);

            INotificationHandler<Ping> handler = new PongChildHandler(writer);

            await handler.HandleAsync(new Ping()
            {
                Message = "Ping"
            })
            .ConfigureAndWaitAsync();

            Assert.Contains("Ping Pong", builder.ToString());
        }

    }
}
