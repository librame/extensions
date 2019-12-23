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
using DotNetty.Transport.Channels.Sockets;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Librame.Extensions.Network.DotNetty.Demo
{
    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal class QuoteOfTheMomentServerHandler : SimpleChannelInboundHandler<DatagramPacket>
    {
        static readonly Random _random = new Random();

        private readonly IQuoteOfTheMomentServer _server;
        private readonly ILogger _logger;


        public QuoteOfTheMomentServerHandler(IQuoteOfTheMomentServer server)
            : base()
        {
            _server = server;
            _logger = server.LoggerFactory.CreateLogger<QuoteOfTheMomentServerHandler>();
        }


        // Quotes from Mohandas K. Gandhi:
        static readonly string[] Quotes =
        {
            "Where there is love there is life.",
            "First they ignore you, then they laugh at you, then they fight you, then you win.",
            "Be the change you want to see in the world.",
            "The weak can never forgive. Forgiveness is the attribute of the strong.",
        };

        static string NextQuote()
        {
            int quoteId = _random.Next(Quotes.Length);
            return Quotes[quoteId];
        }

        protected override void ChannelRead0(IChannelHandlerContext ctx, DatagramPacket packet)
        {
            _logger.LogInformation($"Server Received => {packet}");

            if (!packet.Content.IsReadable())
            {
                return;
            }

            var message = packet.Content.ToString(_server.CoreOptions.Encoding.Source);
            if (message != "QOTM?")
            {
                return;
            }

            var bytes = _server.CoreOptions.Encoding.Source.GetBytes("QOTM: " + NextQuote());
            var buffer = Unpooled.WrappedBuffer(bytes);
            ctx.WriteAsync(new DatagramPacket(buffer, packet.Sender));
        }

        public override void ChannelReadComplete(IChannelHandlerContext context) => context.Flush();

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            _logger.LogError(exception, exception.AsInnerMessage());
            context.CloseAsync();
        }

    }
}
