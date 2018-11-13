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
using System.Numerics;

namespace Librame.Extensions.Network.DotNetty
{
    internal class InternalFactorialServerHandler : SimpleChannelInboundHandler<BigInteger>
    {
        private readonly IFactorialServer _server;
        private readonly ILogger _logger;

        private BigInteger _lastMultiplier = new BigInteger(1);
        private BigInteger _factorial = new BigInteger(1);


        public InternalFactorialServerHandler(IFactorialServer server)
        {
            _server = server.NotDefault(nameof(server));
            _logger = server.LoggerFactory.CreateLogger<InternalFactorialServerHandler>();
        }


        protected override void ChannelRead0(IChannelHandlerContext context, BigInteger message)
        {
            _lastMultiplier = message;
            _factorial *= message;
            context.WriteAndFlushAsync(_factorial);
        }

        public override void ChannelReadComplete(IChannelHandlerContext context)
            => _logger.LogInformation($"Factorial of {_lastMultiplier} is: {_factorial}");


        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            _logger.LogError(exception, exception.AsInnerMessage());
            context.CloseAsync();
        }

    }
}
