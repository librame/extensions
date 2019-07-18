using Microsoft.Extensions.DependencyInjection;
using System;

namespace Librame.Extensions.Encryption.Tests
{
    internal static class TestServiceProvider
    {
        private static readonly AlgorithmIdentifier _defaultIdentifier
            = AlgorithmIdentifier.New();


        static TestServiceProvider()
        {
            Current = Current.EnsureSingleton(() =>
            {
                var services = new ServiceCollection();

                services.AddLibrame()
                    .AddEncryption(options =>
                    {
                        options.Identifier = _defaultIdentifier;
                    })
                    .AddDeveloperGlobalSigningCredentials();

                return services.BuildServiceProvider();
            });
        }


        public static IServiceProvider Current { get; }
    }
}
