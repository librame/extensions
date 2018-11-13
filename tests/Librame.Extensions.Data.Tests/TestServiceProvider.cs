using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Librame.Extensions.Data.Tests
{
    using Builders;

    internal static class TestServiceProvider
    {
        static TestServiceProvider()
        {
            if (Current.IsDefault())
            {
                var services = new ServiceCollection();

                services.AddLibrame()
                .AddData(configureOptions: options =>
                {
                    options.Connection.DefaultString = "Data Source=PC-CLOUD\\SQLEXPRESS;Initial Catalog=librame_default;Integrated Security=True";
                    options.Connection.WriteString = "Data Source=PC-CLOUD\\SQLEXPRESS;Initial Catalog=librame_write;Integrated Security=True";
                    options.Connection.WriteSeparation = true;
                })
                .AddDbContext<ITestDbContext, TestDbContext>((options, builder) =>
                {
                    var migrationsAssembly = typeof(TestServiceProvider).Assembly.GetName().Name;
                    builder.UseSqlServer(options.Connection.DefaultString,
                        sql => sql.MigrationsAssembly(migrationsAssembly));
                });

                services.AddTransient<ITestStore, TestStore>();

                Current = services.BuildServiceProvider();
            }
        }

        public static IServiceProvider Current { get; private set; }
    }
}
