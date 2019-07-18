using DotNetty.Buffers;
using DotNetty.Codecs.Http.WebSockets;
using DotNetty.Common.Internal.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Cryptography.X509Certificates;

namespace Librame.Extensions.WebSocket.Client
{
    using Core;
    using Encryption;
    using Network;
    using Network.DotNetty;

    class Program
    {
        static void Main(string[] args)
        {
            var locator = "dotnetty.com.pfx".AsFileLocator(AppContext.BaseDirectory.CombinePath(@"..\..\..\..\..\resources"));

            var services = new ServiceCollection();

            services.AddLibrame(setupLoggingAction: loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.SetMinimumLevel(LogLevel.Trace);

                loggingBuilder.AddConsole(logger => logger.IncludeScopes = false);
                loggingBuilder.AddFilter((str, level) => true);
            })
                .AddEncryption().AddGlobalSigningCredentials(new X509Certificate2(locator.ToString(), "password"))
                .AddNetwork().AddDotNetty();

            // Use DotNetty LoggerFactory
            services.TryReplace(InternalLoggerFactory.DefaultFactory);

            var serviceProvider = services.BuildServiceProvider();
            var client = serviceProvider.GetRequiredService<IWebSocketClient>();
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
