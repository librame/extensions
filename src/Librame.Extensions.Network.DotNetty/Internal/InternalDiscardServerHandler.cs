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

namespace Librame.Extensions.Network.DotNetty.Internal
{
    internal class InternalDiscardServerHandler : SimpleChannelInboundHandler<object>
    {
        private readonly IDiscardServer _server;
        private readonly ILogger _logger;


        public InternalDiscardServerHandler(IDiscardServer server)
        {
            _server = server.NotDefault(nameof(server));
            _logger = server.LoggerFactory.CreateLogger<InternalDiscardServerHandler>();
        }

        
        protected override void ChannelRead0(IChannelHandlerContext context, object message)
        {
        }


        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            _logger.LogError(exception, exception.AsInnerMessage());
            context.CloseAsync();
        }

    }
}
