using Microsoft.Extensions.DependencyInjection;
using System;

namespace Librame.Extensions.Encryption.Tests
{
    using Core;

    internal static class TestServiceProvider
    {
        private static readonly UniqueIdentifier _defaultIdentifier
            = UniqueIdentifier.New();


        static TestServiceProvider()
        {
            Current = Current.EnsureSingleton(() =>
            {
                var services = new ServiceCollection();

                services.AddLibrame()
                    .AddEncryption(options =>
                    {
                        options.Identifier = _defaultIdentifier;
                        options.IdentifierConverter = _defaultIdentifier.Converter;
                    })
                    .AddDeveloperGlobalSigningCredentials();

                return services.BuildServiceProvider();
            });
        }


        public static IServiceProvider Current { get; }
    }
}
