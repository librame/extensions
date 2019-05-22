using Microsoft.Extensions.DependencyInjection;
using System;

namespace Librame.Extensions.Network.Tests
{
    using Encryption;

    internal static class TestServiceProvider
    {
        static TestServiceProvider()
        {
            if (Current == null)
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
