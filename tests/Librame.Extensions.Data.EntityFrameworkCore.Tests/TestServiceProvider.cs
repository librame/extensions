using Microsoft.EntityFrameworkCore;
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

                services.AddLibrame()
                    .AddData(options =>
                    {
                        options.Tenants.Default.DefaultConnectionString = "Data Source=.;Initial Catalog=librame_data_default;Integrated Security=True";
                        options.Tenants.Default.WritingConnectionString = "Data Source=.;Initial Catalog=librame_data_writing;Integrated Security=True";
                        options.Tenants.Default.WritingSeparation = true;
                    })
                    .AddAccessor<TestDbContextAccessor>((options, optionsBuilder) =>
                    {
                        var migrationsAssembly = typeof(TestServiceProvider).Assembly.GetName().Name;
                        optionsBuilder.UseSqlServer(options.Tenants.Default.DefaultConnectionString,
                            sql => sql.MigrationsAssembly(migrationsAssembly));
                    })
                    .AddStoreHub<TestStoreHub>() // IStoreHub<TestDbContextAccessor>
                    .AddInitializer<TestStoreInitializer>() // IStoreInitializer<TestDbContextAccessor>
                    .AddIdentifier<TestStoreIdentifier>(); // IStoreIdentifier

                return services.BuildServiceProvider();
            });
        }


        public static IServiceProvider Current { get; }
    }
}
