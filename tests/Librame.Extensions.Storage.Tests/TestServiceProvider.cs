using Microsoft.Extensions.DependencyInjection;
using System;

namespace Librame.Extensions.Storage.Tests
{
    internal static class TestServiceProvider
    {
        private static object _locker = new object();
        private static IServiceProvider _serviceProvider = null;

        public static IServiceProvider Current
        {
            get
            {
                if (_serviceProvider.IsNull())
                {
                    lock (_locker)
                    {
                        if (_serviceProvider.IsNull())
                        {
                            var services = new ServiceCollection();

                            services.AddLibrame()
                                .AddStorage();

                            _serviceProvider = services.BuildServiceProvider();
                        }
                    }
                }

                return _serviceProvider;
            }
        }
    }
}
