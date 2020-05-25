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

namespace Librame.Extensions.Network.DotNetty.Demo
{
    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal class SecureChatClientHandler : SimpleChannelInboundHandler<string>
    {
        private readonly ILogger _logger;


        public SecureChatClientHandler(ISecureChatClient client)
            : base()
        {
            _logger = client.LoggerFactory.CreateLogger<SecureChatClientHandler>();
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
