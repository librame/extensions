using Microsoft.Extensions.DependencyInjection;
using System;

namespace Librame.Extensions.Core.Tests
{
    internal static class TestServiceProvider
    {
        static TestServiceProvider()
        {
            if (Current == null)
            {
                var services = new ServiceCollection();

                services.AddLibrame();
                services.AddScoped<InjectionServiceTest>();

                Current = services.BuildServiceProvider();
            }
        }

        public static IServiceProvider Current { get; private set; }
    }
}
