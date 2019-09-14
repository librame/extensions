using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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

            var config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory.CombinePath(@"..\..\..\"))
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            var services = new ServiceCollection();

            services.AddLibrame(logging =>
            {
                logging.ClearProviders();
                logging.SetMinimumLevel(LogLevel.Trace);

                logging.AddConsole(logger => logger.IncludeScopes = false);
                logging.AddFilter((str, level) => true);
            })
            .AddEncryption()
            .AddDeveloperGlobalSigningCredentials();

            var serviceProvider = services.BuildServiceProvider();

            RunEncryption(serviceProvider);

            RunThreadPool();

            // Close NLog
            NLog.LogManager.Shutdown();
        }

        private static void RunEncryption(IServiceProvider serviceProvider)
        {
            Console.WriteLine("Please input some content:");

            var content = Console.ReadLine();
            if (content.IsNullOrWhiteSpace())
            {
                Console.WriteLine("Content is null, empty or white space.");
                RunEncryption(serviceProvider);
            }

            var hash = serviceProvider.GetRequiredService<IHashService>();
            var plaintextBuffer = content.AsPlaintextBuffer(serviceProvider);

            Console.WriteLine($"Content MD5: {hash.Md5(plaintextBuffer).AsBase64String()}");
        }

        private static void RunThreadPool()
        {
            using (var pool = new JobThreadPool())
            {
                for (int i = 0; i < 10; i++)
                {
                    var job = new JobDescriptor(i);
                    
                    job.Execution = (t, args) => Console.WriteLine($"add {args[0]}_{t.ManagedThreadId}.");
                    job.FinishCallback = (t, args) => Console.WriteLine($"{args[0]}_{t.ManagedThreadId}_finished.");
                    job.ErrorCallback = (t, args, ex) => Console.WriteLine(ex.AsInnerMessage());

                    pool.Add(job);
                }

                pool.Execute();
            }
        }

    }

}
