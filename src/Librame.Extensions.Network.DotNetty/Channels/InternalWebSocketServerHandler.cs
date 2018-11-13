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
using DotNetty.Codecs.Http;
using DotNetty.Codecs.Http.WebSockets;
using DotNetty.Common.Utilities;
using DotNetty.Transport.Channels;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

using static DotNetty.Codecs.Http.HttpVersion;
using static DotNetty.Codecs.Http.HttpResponseStatus;

namespace Librame.Extensions.Network.DotNetty
{
    internal class InternalWebSocketServerHandler : SimpleChannelInboundHandler<object>
    {
        private readonly IWebSocketServer _server;
        private readonly ILogger _logger;

        //private const string WebsocketPath = "/websocket";

        WebSocketServerHandshaker handshaker;


        public InternalWebSocketServerHandler(IWebSocketServer server)
        {
            _server = server.NotDefault(nameof(server));
            _logger = server.LoggerFactory.CreateLogger<InternalWebSocketServerHandler>();
        }


        protected override void ChannelRead0(IChannelHandlerContext context, object message)
        {
            if (message is IFullHttpRequest request)
            {
                HandleHttpRequest(context, request);
            }
            else if (message is WebSocketFrame frame)
            {
                HandleWebSocketFrame(context, frame);
            }
        }

        public override void ChannelReadComplete(IChannelHandlerContext context)
            => context.Flush();


        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            _logger.LogError(exception, exception.AsInnerMessage());
            context.CloseAsync();
        }


        private void HandleHttpRequest(IChannelHandlerContext context, IFullHttpRequest request)
        {
            // Handle a bad request.
            if (!request.Result.IsSuccess)
            {
                SendHttpResponse(context, request, new DefaultFullHttpResponse(Http11, BadRequest));
                return;
            }

            // Allow only GET methods.
            if (!Equals(request.Method, HttpMethod.Get))
            {
                SendHttpResponse(context, request, new DefaultFullHttpResponse(Http11, Forbidden));
                return;
            }

            // Send the demo page and favicon.ico
            if ("/".Equals(request.Uri))
            {
                var content = WebSocketServerBenchmarkPage.GetContent(GetWebSocketLocation(request));
                var response = new DefaultFullHttpResponse(Http11, OK, content);

                response.Headers.Set(HttpHeaderNames.ContentType, "text/html; charset=UTF-8");
                HttpUtil.SetContentLength(response, content.ReadableBytes);

                SendHttpResponse(context, request, response);
                return;
            }
            if ("/favicon.ico".Equals(request.Uri))
            {
                var res = new DefaultFullHttpResponse(Http11, NotFound);
                SendHttpResponse(context, request, res);
                return;
            }

            // Handshake
            var wsFactory = new WebSocketServerHandshakerFactory(
                GetWebSocketLocation(request), null, true, 5 * 1024 * 1024);

            handshaker = wsFactory.NewHandshaker(request);
            if (handshaker == null)
            {
                WebSocketServerHandshakerFactory.SendUnsupportedVersionResponse(context.Channel);
            }
            else
            {
                handshaker.HandshakeAsync(context.Channel, request);
            }
        }

        private void HandleWebSocketFrame(IChannelHandlerContext context, WebSocketFrame frame)
        {
            // Check for closing frame
            if (frame is CloseWebSocketFrame)
            {
                handshaker.CloseAsync(context.Channel, (CloseWebSocketFrame)frame.Retain());
                return;
            }

            if (frame is PingWebSocketFrame)
            {
                context.WriteAsync(new PongWebSocketFrame((IByteBuffer)frame.Content.Retain()));
                return;
            }

            if (frame is TextWebSocketFrame)
            {
                // Echo the frame
                context.WriteAsync(frame.Retain());
                return;
            }

            if (frame is BinaryWebSocketFrame)
            {
                // Echo the frame
                context.WriteAsync(frame.Retain());
            }
        }

        private void SendHttpResponse(IChannelHandlerContext context, IFullHttpRequest request, IFullHttpResponse response)
        {
            // Generate an error page if response getStatus code is not OK (200).
            if (response.Status.Code != 200)
            {
                var buffer = Unpooled.CopiedBuffer(_server.Options.Encoding.GetBytes(response.Status.ToString()));
                response.Content.WriteBytes(buffer);
                buffer.Release();
                HttpUtil.SetContentLength(response, response.Content.ReadableBytes);
            }

            // Send the response and close the connection if necessary.
            var task = context.Channel.WriteAndFlushAsync(response);
            if (!HttpUtil.IsKeepAlive(request) || response.Status.Code != 200)
            {
                task.ContinueWith((t, c) => ((IChannelHandlerContext)c).CloseAsync(),
                    context, TaskContinuationOptions.ExecuteSynchronously);
            }
        }
        
        private string GetWebSocketLocation(IFullHttpRequest request)
        {
            var result = request.Headers.TryGet(HttpHeaderNames.Host, out ICharSequence value);
            Debug.Assert(result, "Host header does not exist.");

            var location = value.ToString() + _server.Options.WebSocketClient.Path;

            if (_server.Options.WebSocketServer.UseSSL)
                return "wss://" + location;
            else
                return "ws://" + location;
        }

    }
}
