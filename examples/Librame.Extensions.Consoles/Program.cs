using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using System;

namespace Librame.Extensions.Consoles
{
    using Core;
    using Encryption;

    class Program
    {
        static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory.CombinePath(@"..\..\..\"))
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            var services = new ServiceCollection();

            services.AddLibrame(options => options.ConfigureLogging = loggingBuilder =>
            {
                // configure Logging with NLog
                loggingBuilder.ClearProviders();
                loggingBuilder.SetMinimumLevel(LogLevel.Trace);
                loggingBuilder.AddNLog(config);
            })
                .AddEncryption()
                .AddDeveloperGlobalSigningCredentials();

            var serviceProvider = services.BuildServiceProvider();

            // Configure NLog
            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
            loggerFactory.AddNLog(new NLogProviderOptions { CaptureMessageTemplates = true, CaptureMessageProperties = true });

            // Add NLog Configuration
            NLog.LogManager.LoadConfiguration("../../../configs/nlog.config");

            RunEncryption(serviceProvider);

            RunThreadPoolTest();

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

            var hash = serviceProvider.GetRequiredService<IHashService>();
            var plaintextBuffer = content.AsPlaintextBuffer(serviceProvider);

            Console.WriteLine($"Content MD5: {hash.Md5(plaintextBuffer).AsBase64String()}");
        }

        private static void RunThreadPoolTest()
        {
            using (var pool = new JobThreadPool())
            {
                for (int i = 0; i < 10; i++)
                {
                    var job = new JobDescriptor(i);
                    
                    job.Execution = (t, args) => Console.WriteLine($"add {args[0]}_{t.ManagedThreadId}.");
                    job.FinishCallback = (t, args) => Console.WriteLine($"{args[0]}_{t.ManagedThreadId}_finished.");
                    job.ErrorCallback = (t, args, ex) => Console.WriteLine(ex.AsInnerMessage());

                    pool.AddJob(job);
                }
            }
        }

    }

}
