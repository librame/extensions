using Microsoft.Extensions.DependencyInjection;
using System;

namespace Librame.Extensions.Network.Tests
{
    internal static class TestServiceProvider
    {
        static TestServiceProvider()
        {
            Current = Current.EnsureSingleton(() =>
            {
                var services = new ServiceCollection();

                services.AddLibrame()
                    .AddNetwork();

                return services.BuildServiceProvider();
            });
        }


        public static IServiceProvider Current { get; }
    }
}
