﻿using Microsoft.EntityFrameworkCore;
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
                        options.LocalTenant.DefaultConnectionString = "Data Source=PC-CLOUD\\SQLEXPRESS;Initial Catalog=librame_default;Integrated Security=True";
                        options.LocalTenant.WriteConnectionString = "Data Source=PC-CLOUD\\SQLEXPRESS;Initial Catalog=librame_write;Integrated Security=True";
                        options.LocalTenant.WriteConnectionSeparation = true;
                    })
                    .AddAccessor<TestDbContextAccessor>((options, optionsBuilder) =>
                    {
                        var migrationsAssembly = typeof(TestServiceProvider).Assembly.GetName().Name;
                        optionsBuilder.UseSqlServer(options.LocalTenant.DefaultConnectionString,
                            sql => sql.MigrationsAssembly(migrationsAssembly));
                    });

                services.AddTransient<ITestStore, TestStore>();

                return services.BuildServiceProvider();
            });
        }


        public static IServiceProvider Current { get; }
    }
}
