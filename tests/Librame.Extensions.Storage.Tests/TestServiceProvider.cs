using Microsoft.Extensions.DependencyInjection;
using System;

namespace Librame.Extensions.Storage.Tests
{
    internal static class TestServiceProvider
    {
        static TestServiceProvider()
        {
            if (Current == null)
            {
                var services = new ServiceCollection();

                services.AddLibrame()
                    .AddStorage();

                Current = services.BuildServiceProvider();
            }
        }

        public static IServiceProvider Current { get; private set; }
    }
}
