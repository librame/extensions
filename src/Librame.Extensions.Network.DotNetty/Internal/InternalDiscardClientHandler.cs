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
    internal class InternalDiscardClientHandler : SimpleChannelInboundHandler<object>
    {
        private readonly IDiscardClient _client;
        private readonly ILogger _logger;

        private IChannelHandlerContext _context;
        private byte[] _array;


        public InternalDiscardClientHandler(IDiscardClient client)
            : base()
        {
            _client = client;
            _logger = client.LoggerFactory.CreateLogger<InternalDiscardClientHandler>();
        }


        public override void ChannelActive(IChannelHandlerContext context)
        {
            _array = new byte[_client.Options.DiscardClient.BufferSize];
            _context = context;

            // Send the initial messages.
            GenerateTraffic();
        }

        protected override void ChannelRead0(IChannelHandlerContext context, object message)
        {
            // Server is supposed to send nothing, but if it sends something, discard it.
        }

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            _logger.LogError(exception, exception.AsInnerMessage());
            _context.CloseAsync();
        }

        private async void GenerateTraffic()
        {
            try
            {
                var buffer = Unpooled.WrappedBuffer(_array);
                // Flush the outbound buffer to the socket.
                // Once flushed, generate the same amount of traffic again.
                await _context.WriteAndFlushAsync(buffer);

                GenerateTraffic();
            }
            catch
            {
                await _context.CloseAsync();
            }
        }

    }
}
