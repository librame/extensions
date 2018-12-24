using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Librame.Extensions.Data.Tests
{
    using Builders;
    using Models;

    internal static class TestServiceProvider
    {
        static TestServiceProvider()
        {
            if (Current.IsDefault())
            {
                var services = new ServiceCollection();

                //services.AddEntityFrameworkSqlServer();

                services.AddLibrame()
                    .AddData<TestBuilderOptions>(options =>
                    {
                        options.CategoryTable = new TableSchema<Category>();
                        //options.ArticleTable = TableSchema<Article>.BuildEveryYear(DateTime.Now);
                        //options.ArticleTable = new TableSchema<Article>(typeNames => $"{typeNames}_19");
                        options.ArticleTable = new TableSchema<Article>();

                        options.LocalTenant.DefaultConnectionString = "Data Source=PC-CLOUD\\SQLEXPRESS;Initial Catalog=librame_default;Integrated Security=True";
                        options.LocalTenant.WriteConnectionString = "Data Source=PC-CLOUD\\SQLEXPRESS;Initial Catalog=librame_write;Integrated Security=True";
                        options.LocalTenant.WriteConnectionSeparation = true;
                    })
                    .AddDbContext<ITestDbContext, TestDbContext, TestBuilderOptions>((options, optionsBuilder) =>
                    {
                        var migrationsAssembly = typeof(TestServiceProvider).Assembly.GetName().Name;
                        optionsBuilder.UseSqlServer(options.LocalTenant.DefaultConnectionString,
                            sql => sql.MigrationsAssembly(migrationsAssembly));
                    });

                services.AddTransient<ITestStore, TestStore>();

                Current = services.BuildServiceProvider();
            }
        }

        public static IServiceProvider Current { get; private set; }
    }
}
