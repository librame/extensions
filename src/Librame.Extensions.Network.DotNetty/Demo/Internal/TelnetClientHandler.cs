#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using DotNetty.Transport.Channels;
using Microsoft.Extensions.Logging;
using System;

namespace Librame.Extensions.Network.DotNetty
{
    class TelnetClientHandler : SimpleChannelInboundHandler<string>
    {
        private readonly ITelnetClient _client;
        private readonly ILogger _logger;


        public TelnetClientHandler(ITelnetClient client)
            : base()
        {
            _client = client;
            _logger = client.LoggerFactory.CreateLogger<TelnetClientHandler>();
        }


        protected override void ChannelRead0(IChannelHandlerContext context, string message)
        {
            _logger.LogInformation($"Received from server: {message}");
        }

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            _logger.LogInformation($"Now: {DateTime.Now.Millisecond}");
            _logger.LogInformation($"StackTrace: {exception.StackTrace}");
            context.CloseAsync();
        }

    }
}
