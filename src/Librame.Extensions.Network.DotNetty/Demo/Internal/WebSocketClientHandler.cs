#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using DotNetty.Codecs.Http;
using DotNetty.Codecs.Http.WebSockets;
using DotNetty.Common.Concurrency;
using DotNetty.Common.Utilities;
using DotNetty.Transport.Channels;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Librame.Extensions.Network.DotNetty.Demo
{
    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal class WebSocketClientHandler : SimpleChannelInboundHandler<object>
    {
        private readonly IWebSocketClient _client;
        private readonly ILogger _logger;

        private readonly WebSocketClientHandshaker _handshaker;
        private readonly TaskCompletionSource _completionSource;


        public WebSocketClientHandler(IWebSocketClient client,
            WebSocketClientHandshaker handshaker)
        {
            _client = client;
            _logger = client.LoggerFactory.CreateLogger<WebSocketClientHandler>();

            _handshaker = handshaker;
            _completionSource = new TaskCompletionSource();
        }


        public Task HandshakeCompletion => _completionSource.Task;


        public override void ChannelActive(IChannelHandlerContext context)
            => _handshaker.HandshakeAsync(context.Channel).LinkOutcome(_completionSource);

        public override void ChannelInactive(IChannelHandlerContext context)
        {
            _logger.LogInformation("WebSocket Client disconnected!");
        }

        protected override void ChannelRead0(IChannelHandlerContext context, object message)
        {
            var channel = context.Channel;
            if (!_handshaker.IsHandshakeComplete)
            {
                try
                {
                    _handshaker.FinishHandshake(channel, (IFullHttpResponse)message);
                    _logger.LogInformation("WebSocket Client connected!");
                    _completionSource.TryComplete();
                }
                catch (WebSocketHandshakeException e)
                {
                    _logger.LogInformation("WebSocket Client failed to connect");
                    _completionSource.TrySetException(e);
                }

                return;
            }


            if (message is IFullHttpResponse response)
            {
                throw new InvalidOperationException(
                    $"Unexpected FullHttpResponse (getStatus={response.Status}, content={response.Content.ToString(_client.CoreOptions.Encoding)})");
            }

            if (message is TextWebSocketFrame textFrame)
            {
                _logger.LogInformation($"WebSocket Client received message: {textFrame.Text()}");
            }
            else if (message is PongWebSocketFrame)
            {
                _logger.LogInformation("WebSocket Client received pong");
            }
            else if (message is CloseWebSocketFrame)
            {
                _logger.LogInformation("WebSocket Client received closing");
                channel.CloseAsync();
            }
        }


        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            _logger.LogError(exception, exception.AsInnerMessage());
            _completionSource.TrySetException(exception);
            context.CloseAsync();
        }

    }
}
