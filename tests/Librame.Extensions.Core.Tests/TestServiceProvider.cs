using Microsoft.Extensions.DependencyInjection;
using System;

namespace Librame.Extensions.Core.Tests
{
    internal static class TestServiceProvider
    {
        static TestServiceProvider()
        {
            Current = Current.EnsureSingleton(() =>
            {
                var services = new ServiceCollection();

                services.AddLibrame();

                services.AddSingleton<IRequestHandler<Ping, Pong>, PingHandler>();
                services.AddSingleton<IRequestPreProcessor<Ping>, PingPreProcessor>();
                services.AddSingleton<IRequestPostProcessor<Ping, Pong>, PingPongPostProcessor>();

                services.AddScoped<TestInjectionService>();

                return services.BuildServiceProvider();
            });
        }


        public static IServiceProvider Current { get; }
    }
}
