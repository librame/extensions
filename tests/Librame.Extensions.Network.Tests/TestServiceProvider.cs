using Microsoft.Extensions.DependencyInjection;
using System;

namespace Librame.Extensions.Network.Tests
{
    using Encryption;

    internal class TestServiceProvider
    {
        static TestServiceProvider()
        {
            Current = Current.EnsureSingleton(() =>
            {
                var services = new ServiceCollection();

                services.AddLibrame()
                    .AddEncryption().AddDeveloperGlobalSigningCredentials()
                    .AddNetwork();

                return services.BuildServiceProvider();
            });
        }


        public static IServiceProvider Current { get; }
    }
}
