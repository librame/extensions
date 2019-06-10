using Microsoft.Extensions.DependencyInjection;
using System;

namespace Librame.Extensions.Drawing.Tests
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
                                .AddDrawing(options =>
                                {
                                    options.Captcha.Font.FileLocator.ChangeBasePath(ResourcesPath);
                                    options.Watermark.Font.FileLocator.ChangeBasePath(ResourcesPath);
                                    options.Watermark.ImageFileLocator.ChangeBasePath(ResourcesPath);
                                });

                            _serviceProvider = services.BuildServiceProvider();
                        }
                    }
                }

                return _serviceProvider;
            }
        }

        public static string ResourcesPath
        {
            get { return AppContext.BaseDirectory.CombinePath(@"..\..\..\..\..\resources"); }
        }

    }
}
