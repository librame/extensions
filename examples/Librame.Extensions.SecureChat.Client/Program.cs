using DotNetty.Common.Internal.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Console;
using System;
using System.Security.Cryptography.X509Certificates;

namespace Librame.Extensions.SecureChat.Client
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

            var locator = "dotnetty.com.pfx".AsFileLocator(AppContext.BaseDirectory.CombinePath(@"..\..\..\..\..\resources"));

            services.AddLibrame(options => options.ReplaceLoggerFactory(loggerFactory))
                .AddEncryption().AddGlobalSigningCredentials(new X509Certificate2(locator.ToString(), "password"))
                .AddNetwork().AddDotNetty();

            var serviceProvider = services.BuildServiceProvider();
            var client = serviceProvider.GetRequiredService<ISecureChatClient>();

            client.StartAsync(async channel =>
            {
                for (; ; )
                {
                    string line = Console.ReadLine();
                    if (string.IsNullOrEmpty(line))
                    {
                        continue;
                    }

                    try
                    {
                        await channel.WriteAndFlushAsync(line + "\r\n");
                    }
                    catch
                    {
                    }

                    var clientOptions = client.Options.SecureChatClient;
                    if (string.Equals(line, clientOptions.ExitCommand, StringComparison.OrdinalIgnoreCase))
                    {
                        await channel.CloseAsync();
                        break;
                    }
                }
            })
            .Wait();
        }
    }
}
