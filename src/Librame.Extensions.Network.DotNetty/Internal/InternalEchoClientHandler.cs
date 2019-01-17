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
    internal class InternalEchoClientHandler : ChannelHandlerAdapter
    {
        private readonly IEchoClient _client;
        private readonly ILogger _logger;

        private readonly IByteBuffer _buffer;


        public InternalEchoClientHandler(IEchoClient client)
        {
            _client = client;
            _logger = client.LoggerFactory.CreateLogger<InternalEchoClientHandler>();

            _buffer = Unpooled.Buffer(_client.Options.EchoClient.BufferSize);

            var initMessage = "Welcome to Librame";
            _buffer.WriteBytes(_client.Options.Encoding.GetBytes(initMessage));
            _logger.LogInformation($"Write message: {initMessage}");
        }


        public override void ChannelActive(IChannelHandlerContext context)
            => context.WriteAndFlushAsync(_buffer);

        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            var buffer = message as IByteBuffer;
            if (buffer != null)
                _logger.LogInformation($"Received from server: {buffer.ToString(_client.Options.Encoding)}");

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
