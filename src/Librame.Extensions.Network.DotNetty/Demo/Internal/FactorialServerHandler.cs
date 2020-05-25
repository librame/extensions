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
using System.Numerics;

namespace Librame.Extensions.Network.DotNetty.Demo
{
    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal class FactorialServerHandler : SimpleChannelInboundHandler<BigInteger>
    {
        private readonly ILogger _logger;

        private BigInteger _lastMultiplier = new BigInteger(1);
        private BigInteger _factorial = new BigInteger(1);


        public FactorialServerHandler(IFactorialServer server)
        {
            _logger = server.LoggerFactory.CreateLogger<FactorialServerHandler>();
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
