using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Librame.Extensions.Data.Tests
{
    internal class TestServiceProvider
    {
        static TestServiceProvider()
        {
            Current = Current.EnsureSingleton(() =>
            {
                var services = new ServiceCollection();

                services.AddLibrame()
                    .AddData(options =>
                    {
                        options.DefaultTenant.DefaultConnectionString = "Data Source=.;Initial Catalog=librame_default;Integrated Security=True";
                        options.DefaultTenant.WriteConnectionString = "Data Source=.;Initial Catalog=librame_write;Integrated Security=True";
                        options.DefaultTenant.WriteConnectionSeparation = true;
                    })
                    .AddAccessor<TestDbContextAccessor>((options, optionsBuilder) =>
                    {
                        var migrationsAssembly = typeof(TestServiceProvider).Assembly.GetName().Name;
                        optionsBuilder.UseSqlServer(options.DefaultTenant.DefaultConnectionString,
                            sql => sql.MigrationsAssembly(migrationsAssembly));
                    });

                services.TryReplace<IIdentifierService, TestIdentifierService>();
                services.TryReplace(typeof(IInitializerService<>), typeof(TestInitializerService<>));
                services.AddScoped<ITestStoreHub, TestStoreHub>();

                return services.BuildServiceProvider();
            });
        }


        public static IServiceProvider Current { get; }
    }
}
