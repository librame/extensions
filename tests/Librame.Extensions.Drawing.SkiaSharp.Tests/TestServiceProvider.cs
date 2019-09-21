using Microsoft.Extensions.DependencyInjection;
using System;

namespace Librame.Extensions.Drawing.Tests
{
    internal static class TestServiceProvider
    {
        static TestServiceProvider()
        {
            Current = Current.EnsureSingleton(() =>
            {
                var services = new ServiceCollection();

                services.AddLibrame()
                    .AddDrawing(options =>
                    {
                        options.Captcha.Font.FilePath.ChangeBasePath(ResourcesPath);
                        options.Watermark.Font.FilePath.ChangeBasePath(ResourcesPath);
                        options.Watermark.ImagePath.ChangeBasePath(ResourcesPath);
                    });

                return services.BuildServiceProvider();
            });
        }


        public static IServiceProvider Current { get; }

        public static string ResourcesPath
        {
            get { return AppContext.BaseDirectory.CombinePath(@"..\..\..\..\..\resources"); }
        }
    }
}
