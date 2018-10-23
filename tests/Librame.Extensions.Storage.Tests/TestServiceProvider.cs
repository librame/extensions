using Microsoft.Extensions.DependencyInjection;
using System;

namespace Librame.Extensions.Storage.Tests
{
    using Builders;

    internal static class TestServiceProvider
    {
        static TestServiceProvider()
        {
            if (Current.IsDefault())
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
