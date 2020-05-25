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
using DotNetty.Codecs.Http;
using DotNetty.Common;
using DotNetty.Common.Utilities;
using DotNetty.Transport.Channels;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Librame.Extensions.Network.DotNetty.Demo
{
    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal class HttpServerHandler : ChannelHandlerAdapter
    {
        private static readonly ThreadLocalCache _cache = new ThreadLocalCache();

        private sealed class ThreadLocalCache : FastThreadLocal<AsciiString>
        {
            protected override AsciiString GetInitialValue()
            {
                DateTime dateTime = DateTime.UtcNow;
                return AsciiString.Cached($"{dateTime.DayOfWeek}, {dateTime:dd MMM yyyy HH:mm:ss z}");
            }
        }


        private static readonly byte[] _staticPlaintext = Encoding.UTF8.GetBytes("Hello, World!");
        private static readonly int _staticPlaintextLen = _staticPlaintext.Length;
        private static readonly IByteBuffer _plaintextContentBuffer = Unpooled.UnreleasableBuffer(Unpooled.DirectBuffer().WriteBytes(_staticPlaintext));
        private static readonly AsciiString _plaintextClheaderValue = AsciiString.Cached($"{_staticPlaintextLen}");
        private static readonly AsciiString _jsonClheaderValue = AsciiString.Cached($"{_jsonLen()}");

        private static readonly AsciiString _typePlain = AsciiString.Cached("text/plain");
        private static readonly AsciiString _typeJson = AsciiString.Cached("application/json");
        private static readonly AsciiString _serverName = AsciiString.Cached("Netty");
        private static readonly AsciiString _contentTypeEntity = HttpHeaderNames.ContentType;
        private static readonly AsciiString _dateEntity = HttpHeaderNames.Date;
        private static readonly AsciiString _contentLengthEntity = HttpHeaderNames.ContentLength;
        private static readonly AsciiString _serverEntity = HttpHeaderNames.Server;

        private volatile ICharSequence _date = _cache.Value;

        private static int _jsonLen() => Encoding.UTF8.GetBytes(_newMessage().ToJsonFormat()).Length;

        private static MessageBody _newMessage() => new MessageBody("Hello, Librame!");


        private readonly IHttpServer _server;
        private readonly ILogger _logger;


        public HttpServerHandler(IHttpServer server)
        {
            _server = server.NotNull(nameof(server));
            _logger = server.LoggerFactory.CreateLogger<HttpServerHandler>();
        }


        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            if (message is IHttpRequest request)
            {
                try
                {
                    Process(context, request);
                }
                finally
                {
                    ReferenceCountUtil.Release(message);
                }
            }
            else
            {
                context.FireChannelRead(message);
            }
        }

        private void Process(IChannelHandlerContext context, IHttpRequest request)
        {
            string uri = request.Uri;
            switch (uri)
            {
                case "/json":
                    var json = Encoding.UTF8.GetBytes(_newMessage().ToJsonFormat());
                    WriteResponse(context, Unpooled.WrappedBuffer(json), _typeJson, _jsonClheaderValue);
                    break;

                case "/404":
                    var response = new DefaultFullHttpResponse(HttpVersion.Http11, HttpResponseStatus.NotFound, Unpooled.Empty, false);
                    context.WriteAndFlushAsync(response);
                    context.CloseAsync();
                    break;

                default:
                    WriteResponse(context, _plaintextContentBuffer.Duplicate(), _typePlain, _plaintextClheaderValue);
                    break;
            }
        }

        private void WriteResponse(IChannelHandlerContext ctx, IByteBuffer buf, ICharSequence contentType, ICharSequence contentLength)
        {
            // Build the response object.
            var response = new DefaultFullHttpResponse(HttpVersion.Http11, HttpResponseStatus.OK, buf, false);
            HttpHeaders headers = response.Headers;
            headers.Set(_contentTypeEntity, contentType);
            headers.Set(_serverEntity, _serverName);
            headers.Set(_dateEntity, _date);
            headers.Set(_contentLengthEntity, contentLength);

            // Close the non-keep-alive connection after the write operation is done.
            ctx.WriteAsync(response);
        }

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
            => context.CloseAsync();

        public override void ChannelReadComplete(IChannelHandlerContext context)
            => context.Flush();
    }


    class MessageBody
    {
        public MessageBody(string message)
        {
            Message = message;
        }


        public string Message { get; }


        public virtual string ToJsonFormat()
        {
            return "{" + $"\"{nameof(MessageBody)}\" :" + "{" + $"\"{nameof(Message)}\"" + " :\"" + Message + "\"}" + "}";
        }


        public override string ToString()
        {
            return Message;
        }

    }
}
