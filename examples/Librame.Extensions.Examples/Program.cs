using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;

namespace Librame.Extensions.Examples
{
    using Core;
    using Encryption;

    class Program
    {
        static void Main(string[] args)
        {
            // Add NLog Configuration
            NLog.LogManager.LoadConfiguration("../../../configs/nlog.config");

            var root = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory.CombinePath(@"..\..\..\"))
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            var services = new ServiceCollection();

            services.AddLibrame<ExampleCoreBuilderDependencyOptions>(dependency =>
            {
                dependency.Configuration = root.GetSection(dependency.Name);

                dependency.LoggingAction = logging =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(LogLevel.Trace);

                    logging.AddConsole(logger => logger.IncludeScopes = false);
                    logging.AddFilter((str, level) => true);
                };
            })
            .AddEncryption()
            .AddDeveloperGlobalSigningCredentials();

            var serviceProvider = services.BuildServiceProvider();

            RunEncryption(serviceProvider);

            // Close NLog
            NLog.LogManager.Shutdown();
        }

        private static void RunEncryption(IServiceProvider serviceProvider)
        {
            var options = serviceProvider.GetRequiredService<IOptions<ExampleOptions>>().Value;
            Console.WriteLine(options.Message);
            Console.WriteLine("Please input some content:");

            var content = Console.ReadLine();
            if (content.IsWhiteSpace())
            {
                Console.WriteLine("Content is null, empty or white space.");
                RunEncryption(serviceProvider);
            }
            
            var hash = serviceProvider.GetRequiredService<IHashService>();
            var plaintextBuffer = content.AsPlaintextBuffer(serviceProvider);

            Console.WriteLine($"Content MD5: {hash.Md5(plaintextBuffer).AsBase64String()}");
        }

    }

}
