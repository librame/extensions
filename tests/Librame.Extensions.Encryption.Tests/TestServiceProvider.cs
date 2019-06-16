using Microsoft.Extensions.DependencyInjection;
using System;

namespace Librame.Extensions.Encryption.Tests
{
    internal class TestServiceProvider
    {
        static TestServiceProvider()
        {
            Current = Current.EnsureSingleton(() =>
            {
                var services = new ServiceCollection();

                services.AddLibrame()
                    .AddEncryption()
                    .AddDeveloperGlobalSigningCredentials();

                return services.BuildServiceProvider();
            });
        }


        public static IServiceProvider Current { get; }
    }
}
