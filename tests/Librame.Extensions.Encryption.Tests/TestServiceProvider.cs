using Microsoft.Extensions.DependencyInjection;
using System;

namespace Librame.Extensions.Encryption.Tests
{
    using Core;

    internal static class TestServiceProvider
    {
        static TestServiceProvider()
        {
            if (Current == null)
            {
                var services = new ServiceCollection();

                services.AddLibrame()
                    .AddEncryption()
                    .AddDeveloperGlobalSigningCredentials();

                Current = services.BuildServiceProvider();
            }
        }

        public static IServiceProvider Current { get; private set; }
    }
}
