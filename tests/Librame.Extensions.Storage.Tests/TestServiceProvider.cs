using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

namespace Librame.Extensions.Storage.Tests
{
    internal static class TestServiceProvider
    {
        static TestServiceProvider()
        {
            Current = Current.EnsureSingleton(() =>
            {
                var services = new ServiceCollection();

                services.AddLibrame()
                    .AddStorage(options =>
                    {
                        options.FileProviders.Add(new PhysicalStorageFileProvider(Path.GetTempPath()));
                    });

                return services.BuildServiceProvider();
            });
        }


        public static IServiceProvider Current { get; }
    }
}
