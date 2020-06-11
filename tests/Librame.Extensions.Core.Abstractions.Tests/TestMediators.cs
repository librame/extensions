using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Core.Tests
{
    using Mediators;

    public class PingNotification : INotificationIndication
    {
        public string Message { get; set; }
    }

    public class PingNotificationHandler : INotificationHandler<PingNotification>
    {
        private readonly TextWriter _writer;

        public PingNotificationHandler(TextWriter writer)
        {
            _writer = writer;
        }

        public Task HandleAsync(PingNotification notification, CancellationToken cancellationToken = default)
        {
            _writer.WriteLine(notification.Message + " Pong");

            return Task.CompletedTask;
        }
    }

    public class Pong
    {
        public string Message { get; set; }
    }

    public class PingRequest : IRequest<Pong>
    {
        public string Message { get; set; }
    }

    public class PingRequestHandler : IRequestHandler<PingRequest, Pong>
    {
        public Task<Pong> HandleAsync(PingRequest request, CancellationToken cancellationToken = default)
        {
            return Task.Run(() => new Pong
            {
                Message = request.Message + " Pong"
            });
        }
    }

}
