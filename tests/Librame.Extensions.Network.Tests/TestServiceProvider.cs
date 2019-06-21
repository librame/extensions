using Microsoft.Extensions.DependencyInjection;
using System;

namespace Librame.Extensions.Network.Tests
{
    using Encryption;

    internal class TestServiceProvider
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
                    .AddDeveloperGlobalSigningCredentials()
                    .AddNetwork();

                return services.BuildServiceProvider();
            });
        }


        public static IServiceProvider Current { get; }
    }
}
