using Microsoft.Extensions.DependencyInjection;
using System;

namespace Librame.Extensions.Drawing.Tests
{
    internal static class TestServiceProvider
    {
        static TestServiceProvider()
        {
            Current = Current.EnsureSingleton(() =>
            {
                var services = new ServiceCollection();

                services.AddLibrame()
                    .AddDrawing(dependency =>
                    {
                        dependency.ResourceDirectory = dependency.BaseDirectory.CombinePath(@"..\..\resources");
                    });

                return services.BuildServiceProvider();
            });
        }


        public static IServiceProvider Current { get; }
    }
}
