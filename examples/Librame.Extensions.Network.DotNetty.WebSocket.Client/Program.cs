using DotNetty.Buffers;
using DotNetty.Codecs.Http.WebSockets;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Librame.Extensions.Network.DotNetty.WebSocket.Client
{
    using Demo;

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
                    else if ("bye".Equals(msg, StringComparison.OrdinalIgnoreCase))
                    {
                        await channel.WriteAndFlushAsync(new CloseWebSocketFrame());
                        break;
                    }
                    else if ("ping".Equals(msg, StringComparison.OrdinalIgnoreCase))
                    {
                        var frame = new PingWebSocketFrame(Unpooled.WrappedBuffer(new byte[] { 8, 1, 8, 1 }));
                        await channel.WriteAndFlushAsync(frame);
                    }
                    else
                    {
                        var frame = new TextWebSocketFrame(msg);
                        await channel.WriteAndFlushAsync(frame);
                    }
                }

                await channel.CloseAsync();
            })
            .Wait();
        }
    }
}
