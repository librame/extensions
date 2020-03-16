using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Design.Internal;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Librame.Extensions.Data.Tests
{
    using Builders;

    internal static class TestServiceProvider
    {
        static TestServiceProvider()
        {
            Current = Current.EnsureSingleton(() =>
            {
                var services = new ServiceCollection();

                services.AddLibrame()
                    .AddData(dependency =>
                    {
                        dependency.Options.DefaultTenant.DefaultConnectionString = "Data Source=.;Initial Catalog=librame_data_default;Integrated Security=True";
                        dependency.Options.DefaultTenant.WritingConnectionString = "Data Source=.;Initial Catalog=librame_data_writing;Integrated Security=True";
                        dependency.Options.DefaultTenant.WritingSeparation = true;
                    })
                    .AddAccessor<TestDbContextAccessor>((options, optionsBuilder) =>
                    {
                        optionsBuilder.UseSqlServer(options.DefaultTenant.DefaultConnectionString,
                            sql => sql.MigrationsAssembly(typeof(TestServiceProvider).GetAssemblyDisplayName()));
                    })
                    .AddDbDesignTime<SqlServerDesignTimeServices>()
                    .AddStoreIdentifier<TestStoreIdentifier>()
                    .AddStoreInitializer<TestStoreInitializer>()
                    .AddStoreHub<TestStoreHub>();

                return services.BuildServiceProvider();
            });
        }


        public static IServiceProvider Current { get; }
    }
}
