using Microsoft.Extensions.DependencyInjection;
using System;

namespace Librame.Extensions.Storage.Tests
{
    internal static class TestServiceProvider
    {
        static TestServiceProvider()
        {
            Current = Current.EnsureSingleton(() =>
            {
                var services = new ServiceCollection();

                services.AddLibrame()
                    .AddStorage();

                return services.BuildServiceProvider();
            });
        }


        public static IServiceProvider Current { get; }
    }
}
