using DotNetty.Buffers;
using DotNetty.Codecs.Http.WebSockets;
using DotNetty.Common.Internal.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Librame.Extensions.Network.DotNetty.WebSocket.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = DotNettyServiceProvider.Current.GetRequiredService<IWebSocketClient>();
            client.StartAsync(async channel =>
            {
                while (true)
                {
                    string msg = Console.ReadLine();
                    if (msg.IsNull())
                    {
                        break;
                    }
                    else if ("bye".Equals(msg.ToLower()))
                    {
                        await channel.WriteAndFlushAsync(new CloseWebSocketFrame());
                        break;
                    }
                    else if ("ping".Equals(msg.ToLower()))
                    {
                        var frame = new PingWebSocketFrame(Unpooled.WrappedBuffer(new byte[] { 8, 1, 8, 1 }));
                        await channel.WriteAndFlushAsync(frame);
                    }
                    else
                    {
                        WebSocketFrame frame = new TextWebSocketFrame(msg);
                        await channel.WriteAndFlushAsync(frame);
                    }
                }
            })
            .Wait();
        }
    }
}
