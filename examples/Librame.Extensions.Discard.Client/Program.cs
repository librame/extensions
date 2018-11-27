using DotNetty.Common.Internal.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Console;
using System;
using System.Security.Cryptography.X509Certificates;

namespace Librame.Extensions.Discard.Client
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
            var client = serviceProvider.GetRequiredService<IDiscardClient>();

            client.StartAsync(channel =>
            {
                Console.ReadLine();
            })
            .Wait();
        }
    }
}
