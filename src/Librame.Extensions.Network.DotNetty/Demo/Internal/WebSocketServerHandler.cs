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
using System.Text;
using System.Threading.Tasks;

using static DotNetty.Codecs.Http.HttpVersion;
using static DotNetty.Codecs.Http.HttpResponseStatus;

namespace Librame.Extensions.Network.DotNetty
{
    class WebSocketServerHandler : SimpleChannelInboundHandler<object>
    {
        private readonly IWebSocketServer _server;
        private readonly ILogger _logger;

        //private const string WebsocketPath = "/websocket";

        WebSocketServerHandshaker handshaker;


        public WebSocketServerHandler(IWebSocketServer server)
        {
            _server = server.NotNull(nameof(server));
            _logger = server.LoggerFactory.CreateLogger<WebSocketServerHandler>();
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
            if (handshaker.IsNull())
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

            if (_server.Options.WebSocketServer.IsSsl)
                return "wss://" + location;
            else
                return "ws://" + location;
        }

    }


    class WebSocketServerBenchmarkPage
    {
        const string Newline = "\r\n";

        public static IByteBuffer GetContent(string webSocketLocation) =>
            Unpooled.WrappedBuffer(
                Encoding.ASCII.GetBytes(
                    "<html><head><title>Web Socket Performance Test</title></head>" + Newline +
                    "<body>" + Newline +
                    "<h2>WebSocket Performance Test</h2>" + Newline +
                    "<label>Connection Status:</label>" + Newline +
                    "<label id=\"connectionLabel\"></label><br />" + Newline +

                    "<form onsubmit=\"return false;\">" + Newline +
                    "Message size:" +
                    "<input type=\"text\" id=\"messageSize\" value=\"1024\"/><br>" + Newline +
                    "Number of messages:" +
                    "<input type=\"text\" id=\"nrMessages\" value=\"100000\"/><br>" + Newline +
                    "Data Type:" +
                    "<input type=\"radio\" name=\"type\" id=\"typeText\" value=\"text\" checked>text" +
                    "<input type=\"radio\" name=\"type\" id=\"typeBinary\" value=\"binary\">binary<br>" + Newline +
                    "Mode:<br>" + Newline +
                    "<input type=\"radio\" name=\"mode\" id=\"modeSingle\" value=\"single\" checked>" +
                    "Wait for response after each messages<br>" + Newline +
                    "<input type=\"radio\" name=\"mode\" id=\"modeAll\" value=\"all\">" +
                    "Send all messages and then wait for all responses<br>" + Newline +
                    "<input type=\"checkbox\" id=\"verifiyResponses\">Verify responded messages<br>" + Newline +
                    "<input type=\"button\" value=\"Start Benchmark\"" + Newline +
                    "       onclick=\"startBenchmark()\" />" + Newline +
                    "<h3>Output</h3>" + Newline +
                    "<textarea id=\"output\" style=\"width:500px;height:300px;\"></textarea>" + Newline +
                    "<br>" + Newline +
                    "<input type=\"button\" value=\"Clear\" onclick=\"clearText()\">" + Newline +
                    "</form>" + Newline +

                    "<script type=\"text/javascript\">" + Newline +
                    "var benchRunning = false;" + Newline +
                    "var messageSize = 0;" + Newline +
                    "var totalMessages = 0;" + Newline +
                    "var rcvdMessages = 0;" + Newline +
                    "var isBinary = true;" + Newline +
                    "var isSingle = true;" + Newline +
                    "var verifiyResponses = false;" + Newline +
                    "var benchData = null;" + Newline +
                    "var startTime;" + Newline +
                    "var endTime;" + Newline +
                    "var socket;" + Newline +
                    "var output = document.getElementById('output');" + Newline +
                    "var connectionLabel = document.getElementById('connectionLabel');" + Newline +
                    "if (!window.WebSocket) {" + Newline +
                    "  window.WebSocket = window.MozWebSocket;" + Newline +
                    '}' + Newline +
                    "if (window.WebSocket) {" + Newline +
                    "  socket = new WebSocket(\"" + webSocketLocation + "\");" + Newline +
                    "  socket.binaryType = 'arraybuffer';" + Newline +
                    "  socket.onmessage = function(event) {" + Newline +
                    "    if (verifiyResponses) {" + Newline +
                    "        if (isBinary) {" + Newline +
                    "            if (!(event.data instanceof ArrayBuffer) || " + Newline +
                    "                  event.data.byteLength != benchData.byteLength) {" + Newline +
                    "                onInvalidResponse(benchData, event.data);" + Newline +
                    "                return;" + Newline +
                    "            } else {" + Newline +
                    "                var v = new Uint8Array(event.data);" + Newline +
                    "                for (var j = 0; j < benchData.byteLength; j++) {" + Newline +
                    "                    if (v[j] != benchData[j]) {" + Newline +
                    "                        onInvalidResponse(benchData, event.data);" + Newline +
                    "                        return;" + Newline +
                    "                    }" + Newline +
                    "                }" + Newline +
                    "            }" + Newline +
                    "        } else {" + Newline +
                    "            if (event.data != benchData) {" + Newline +
                    "                onInvalidResponse(benchData, event.data);" + Newline +
                    "                return;" + Newline +
                    "            }" + Newline +
                    "        }" + Newline +
                    "    }" + Newline +
                    "    rcvdMessages++;" + Newline +
                    "    if (rcvdMessages == totalMessages) {" + Newline +
                    "        onFinished();" + Newline +
                    "    } else if (isSingle) {" + Newline +
                    "        socket.send(benchData);" + Newline +
                    "    }" + Newline +
                    "  };" + Newline +
                    "  socket.onopen = function(event) {" + Newline +
                    "    connectionLabel.innerHTML = \"Connected\";" + Newline +
                    "  };" + Newline +
                    "  socket.onclose = function(event) {" + Newline +
                    "    benchRunning = false;" + Newline +
                    "    connectionLabel.innerHTML = \"Disconnected\";" + Newline +
                    "  };" + Newline +
                    "} else {" + Newline +
                    "  alert(\"Your browser does not support Web Socket.\");" + Newline +
                    '}' + Newline +
                    Newline +
                    "function onInvalidResponse(sent,recvd) {" + Newline +
                    "    socket.close();" + Newline +
                    "    alert(\"Error: Sent data did not match the received data!\");" + Newline +
                    "}" + Newline +
                    Newline +
                    "function clearText() {" + Newline +
                    "    output.value=\"\";" + Newline +
                    "}" + Newline +
                    Newline +
                    "function createBenchData() {" + Newline +
                    "    if (isBinary) {" + Newline +
                    "        benchData = new Uint8Array(messageSize);" + Newline +
                    "        for (var i=0; i < messageSize; i++) {" + Newline +
                    "            benchData[i] += Math.floor(Math.random() * 255);" + Newline +
                    "        }" + Newline +
                    "    } else { " + Newline +
                    "        benchData = \"\";" + Newline +
                    "        for (var i=0; i < messageSize; i++) {" + Newline +
                    "            benchData += String.fromCharCode(Math.floor(Math.random() * (123 - 65) + 65));" + Newline +
                    "        }" + Newline +
                    "    }" + Newline +
                    "}" + Newline +
                    Newline +
                    "function startBenchmark(message) {" + Newline +
                    "  if (!window.WebSocket || benchRunning) { return; }" + Newline +
                    "  if (socket.readyState == WebSocket.OPEN) {" + Newline +
                    "    isBinary = document.getElementById('typeBinary').checked;" + Newline +
                    "    isSingle = document.getElementById('modeSingle').checked;" + Newline +
                    "    verifiyResponses = document.getElementById('verifiyResponses').checked;" + Newline +
                    "    messageSize = parseInt(document.getElementById('messageSize').value);" + Newline +
                    "    totalMessages = parseInt(document.getElementById('nrMessages').value);" + Newline +
                    "    if (isNaN(messageSize) || isNaN(totalMessages)) return;" + Newline +
                    "    createBenchData();" + Newline +
                    "    output.value = output.value + '\\nStarting Benchmark';" + Newline +
                    "    rcvdMessages = 0;" + Newline +
                    "    benchRunning = true;" + Newline +
                    "    startTime = new Date();" + Newline +
                    "    if (isSingle) {" + Newline +
                    "        socket.send(benchData);" + Newline +
                    "    } else {" + Newline +
                    "        for (var i = 0; i < totalMessages; i++) socket.send(benchData);" + Newline +
                    "    }" + Newline +
                    "  } else {" + Newline +
                    "    alert(\"The socket is not open.\");" + Newline +
                    "  }" + Newline +
                    '}' + Newline +
                    Newline +
                    "function onFinished() {" + Newline +
                    "    endTime = new Date();" + Newline +
                    "    var duration = (endTime - startTime) / 1000.0;" + Newline +
                    "    output.value = output.value + '\\nTest took: ' + duration + 's';" + Newline +
                    "    var messagesPerS = totalMessages / duration;" + Newline +
                    "    output.value = output.value + '\\nPerformance: ' + messagesPerS + ' Messages/s';" + Newline +
                    "    output.value = output.value + ' in each direction';" + Newline +
                    "    output.value = output.value + '\\nRound trip: ' + 1000.0/messagesPerS + 'ms';" + Newline +
                    "    var throughput = messageSize * totalMessages / duration;" + Newline +
                    "    var throughputText;" + Newline +
                    "    if (isBinary) throughputText = throughput / (1024*1024) + ' MB/s';" + Newline +
                    "    else throughputText = throughput / (1000*1000) + ' MChars/s';" + Newline +
                    "    output.value = output.value + '\\nThroughput: ' + throughputText;" + Newline +
                    "    output.value = output.value + ' in each direction';" + Newline +
                    "    benchRunning = false;" + Newline +
                    "}" + Newline +
                    "</script>" + Newline +
                    "</body>" + Newline +
                    "</html>" + Newline));
    }
}
