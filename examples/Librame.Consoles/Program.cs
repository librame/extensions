using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using System;

namespace Librame.Consoles
{
    using Builders;
    using Extensions;
    using Extensions.Encryption;

    class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection();

            services.AddLibrame()
                .AddEncryption()
                .AddDeveloperGlobalSigningCredentials();

            var serviceProvider = services.BuildServiceProvider();

            // Configure NLog
            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
            loggerFactory.AddNLog(new NLogProviderOptions { CaptureMessageTemplates = true, CaptureMessageProperties = true });

            // Add NLog Configuration
            NLog.LogManager.LoadConfiguration("../../../configs/nlog.config");

            RunEncryption(serviceProvider);

            // Close NLog
            NLog.LogManager.Shutdown();
        }

        private static void RunEncryption(IServiceProvider serviceProvider)
        {
            Console.WriteLine("Please input some content:");

            var content = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(content))
            {
                Console.WriteLine("Content is null, empty or white space.");
                RunEncryption(serviceProvider);
            }

            var hash = serviceProvider.GetRequiredService<IHashAlgorithmService>();
            var plaintextBuffer = content.AsPlaintextBuffer(serviceProvider);

            Console.WriteLine($"Content MD5: {hash.Md5(plaintextBuffer).AsBase64String()}");
        }

    }

}
