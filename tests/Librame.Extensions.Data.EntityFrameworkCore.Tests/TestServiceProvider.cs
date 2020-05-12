using Microsoft.Extensions.DependencyInjection;
using System;

namespace Librame.Extensions.Data.Tests
{
    internal static class TestServiceProvider
    {
        static TestServiceProvider()
        {
            Current = Current.EnsureSingleton(() =>
            {
                var services = new ServiceCollection();

                // 此处不配置 EntityFrameworkCore，EFCore 由 TestStoreHubTests 单独配置
                services.AddLibrame()
                    .AddData(depend => depend.SupportsEntityFrameworkDesignTimeServices = false);

                return services.BuildServiceProvider();
            });
        }


        public static IServiceProvider Current { get; }
    }
}
