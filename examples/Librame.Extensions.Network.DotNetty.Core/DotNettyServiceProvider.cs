using DotNetty.Common.Internal.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Cryptography.X509Certificates;

namespace Librame.Extensions.Network.DotNetty
{
    using Core.Combiners;
    using Encryption.Builders;

    public class DotNettyServiceProvider
    {
        static DotNettyServiceProvider()
        {
            Current = Current.EnsureSingleton(() =>
            {
                var combiner = "dotnetty.com.pfx".AsFilePathCombiner(ResourcesPath);

                var services = new ServiceCollection();

                services.AddLibrame(logging =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(LogLevel.Trace);

                    logging.AddConsole(logger => logger.IncludeScopes = false);
                    logging.AddFilter((str, level) => true);
                })
                .AddEncryption().AddGlobalSigningCredentials(new X509Certificate2(combiner, "password"))
                .AddNetwork().AddDotNetty();

                // Use DotNetty LoggerFactory
                //InternalLoggerFactory.DefaultFactory.AddProvider(new ConsoleLoggerProvider((s, level) => true, false));
                services.TryReplaceAll(InternalLoggerFactory.DefaultFactory);

                return services.BuildServiceProvider();
            });
        }


        public static IServiceProvider Current { get; }

        public static string ResourcesPath
        {
            get { return AppContext.BaseDirectory.CombinePath(@"..\..\..\..\..\resources"); }
        }

    }
}
