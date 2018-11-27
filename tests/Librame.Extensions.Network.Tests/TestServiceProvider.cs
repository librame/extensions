using Microsoft.Extensions.DependencyInjection;
using System;

namespace Librame.Extensions.Network.Tests
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
                    .AddEncryption().AddDeveloperGlobalSigningCredentials()
                    .AddNetwork();

                Current = services.BuildServiceProvider();
            }
        }

        public static IServiceProvider Current { get; private set; }

    }
}
