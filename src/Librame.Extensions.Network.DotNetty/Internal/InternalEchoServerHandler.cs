#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using Microsoft.Extensions.Logging;
using System;

namespace Librame.Extensions.Network.DotNetty.Internal
{
    internal class InternalEchoServerHandler : ChannelHandlerAdapter
    {
        private readonly IEchoServer _server;
        private readonly ILogger _logger;


        public InternalEchoServerHandler(IEchoServer server)
        {
            _server = server.NotNull(nameof(server));
            _logger = server.LoggerFactory.CreateLogger<InternalEchoServerHandler>();
        }


        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            var buffer = message as IByteBuffer;
            if (buffer.IsNotNull())
                _logger.LogInformation($"Received from client: {buffer.ToString(_server.Options.Encoding)}");

            context.WriteAsync(message);
        }

        public override void ChannelReadComplete(IChannelHandlerContext context)
            => context.Flush();


        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            _logger.LogError(exception, exception.AsInnerMessage());
            context.CloseAsync();
        }

    }
}
