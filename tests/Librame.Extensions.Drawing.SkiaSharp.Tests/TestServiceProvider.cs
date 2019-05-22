using Microsoft.Extensions.DependencyInjection;
using System;

namespace Librame.Extensions.Drawing.Tests
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
                    .AddDrawing();

                Current = services.BuildServiceProvider();
            }
        }

        public static IServiceProvider Current { get; private set; }


        public static string ResourcesPath
        {
            get { return AppContext.BaseDirectory.CombinePath(@"..\..\..\..\..\resources"); }
        }

    }
}
