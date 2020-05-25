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
    internal class DiscardClientHandler : SimpleChannelInboundHandler<object>
    {
        private readonly IDiscardClient _client;
        private readonly ILogger _logger;

        private IChannelHandlerContext _context;
        private byte[] _array;


        public DiscardClientHandler(IDiscardClient client)
            : base()
        {
            _client = client.NotNull(nameof(client));
            _logger = client.LoggerFactory.CreateLogger<DiscardClientHandler>();
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

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private async void GenerateTraffic()
        {
            try
            {
                var buffer = Unpooled.WrappedBuffer(_array);
                // Flush the outbound buffer to the socket.
                // Once flushed, generate the same amount of traffic again.
                await _context.WriteAndFlushAsync(buffer).ConfigureAndWaitAsync();

                //GenerateTraffic();
            }
            catch
            {
                await _context.CloseAsync().ConfigureAndWaitAsync();
            }
        }

    }
}
