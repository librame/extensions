using DotNetty.Common.Internal.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Cryptography.X509Certificates;

namespace Librame.Extensions.WebSocket.Server
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

            services.AddLibrame(options => options.ConfigureLogging = loggingBuilder =>
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
            var server = serviceProvider.GetRequiredService<IWebSocketServer>();

            server.StartAsync(channel =>
            {
                Console.ReadLine();
            })
            .Wait();
        }
    }
}
