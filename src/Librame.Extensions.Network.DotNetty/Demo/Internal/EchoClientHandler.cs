#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Librame.Extensions.Network.DotNetty.Demo
{
    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal class EchoClientHandler : ChannelHandlerAdapter
    {
        private readonly IEchoClient _client;
        private readonly ILogger _logger;

        private readonly IByteBuffer _buffer;


        public EchoClientHandler(IEchoClient client)
        {
            _client = client;
            _logger = client.LoggerFactory.CreateLogger<EchoClientHandler>();

            _buffer = Unpooled.Buffer(_client.Options.EchoClient.BufferSize);

            var initMessage = "Welcome to Librame";
            _buffer.WriteBytes(_client.CoreOptions.Encoding.Source.GetBytes(initMessage));
            _logger.LogInformation($"Write message: {initMessage}");
        }


        public override void ChannelActive(IChannelHandlerContext context)
            => context.WriteAndFlushAsync(_buffer);

        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            var buffer = message as IByteBuffer;
            if (buffer.IsNotNull())
                _logger.LogInformation($"Received from server: {buffer.ToString(_client.CoreOptions.Encoding)}");

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
