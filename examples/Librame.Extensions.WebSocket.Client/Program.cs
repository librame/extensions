using DotNetty.Buffers;
using DotNetty.Codecs.Http.WebSockets;
using DotNetty.Common.Internal.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Console;
using System;
using System.Security.Cryptography.X509Certificates;

namespace Librame.Extensions.WebSocket.Client
{
    using Builders;
    using Extensions.Network.DotNetty;

    class Program
    {
        static void Main(string[] args)
        {
            var loggerFactory = InternalLoggerFactory.DefaultFactory;
            loggerFactory.AddProvider(new ConsoleLoggerProvider((s, level) => true, false));

            var services = new ServiceCollection();

            services.AddLibrame(options => options.ReplaceLoggerFactory(loggerFactory))
                .AddNetwork()
                .AddDotNetty()
                .ConfigureEncryption(builder =>
                {
                    var locator = "dotnetty.com.pfx".AsFileLocator(AppContext.BaseDirectory.CombinePath(@"..\..\..\..\..\resources"));
                    builder.AddGlobalSigningCredentials(new X509Certificate2(locator.ToString(), "password"));
                });

            var serviceProvider = services.BuildServiceProvider();
            var client = serviceProvider.GetRequiredService<IWebSocketClient>();
            client.StartAsync(async channel =>
            {
                while (true)
                {
                    string msg = Console.ReadLine();
                    if (msg == null)
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
