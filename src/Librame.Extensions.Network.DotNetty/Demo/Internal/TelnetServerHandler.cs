#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using DotNetty.Transport.Channels;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Threading.Tasks;

namespace Librame.Extensions.Network.DotNetty.Demo
{
    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal class TelnetServerHandler : SimpleChannelInboundHandler<string>
    {
        private readonly ITelnetServer _server;
        private readonly ILogger _logger;


        public TelnetServerHandler(ITelnetServer server)
        {
            _server = server.NotNull(nameof(server));
            _logger = server.LoggerFactory.CreateLogger<TelnetServerHandler>();
        }


        public override void ChannelActive(IChannelHandlerContext context)
        {
            var welcomeMessage = $"Welcome to {Dns.GetHostName()}!\r\n";
            var nowMessage = $"It is {DateTime.Now} now !\r\n";

            context.WriteAsync(welcomeMessage);
            context.WriteAndFlushAsync(nowMessage);

            _logger.LogInformation(welcomeMessage);
            _logger.LogInformation(nowMessage);
        }

        protected override void ChannelRead0(IChannelHandlerContext context, string message)
        {
            // Generate and write a response.
            string response;
            bool close = false;

            if (string.IsNullOrEmpty(message))
            {
                response = "Please type something.\r\n";
            }
            else if (string.Equals(_server.Options.TelnetServer.ExitCommand, message, StringComparison.OrdinalIgnoreCase))
            {
                response = "Have a good day!\r\n";
                close = true;
            }
            else
            {
                response = "Did you say '" + message + "'?\r\n";
            }

            Task wait_close = context.WriteAndFlushAsync(response);
            if (close)
            {
                Task.WaitAll(wait_close);
                context.CloseAsync();
            }
        }

        public override void ChannelReadComplete(IChannelHandlerContext context) => context.Flush();

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            context.CloseAsync();
            _logger.LogInformation(exception.StackTrace);
        }

        public override bool IsSharable => true;

    }
}
