using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Design;
using Microsoft.EntityFrameworkCore.Sqlite.Design.Internal;
using Microsoft.EntityFrameworkCore.SqlServer.Design.Internal;
using Microsoft.Extensions.DependencyInjection;
using Pomelo.EntityFrameworkCore.MySql.Design.Internal;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using System;
using Xunit;

namespace Librame.Extensions.Data.Tests
{
    using Core.Identifiers;
    using Core.Services;
    using Data.Builders;

    public class TestStoreHubTests
    {
        [Fact]
        public void MySqlTest()
        {
            var services = new ServiceCollection();

            services.AddLibrame()
                .AddData(dependency =>
                {
                    dependency.Options.SUIDGenerator = SequentialUniqueIdentifierGenerator.MySQL;

                    dependency.Options.DefaultTenant.DefaultConnectionString
                        = MySqlConnectionStringHelper.Validate("server=localhost;port=3306;database=librame_data_default;user=root;password=123456");
                    dependency.Options.DefaultTenant.WritingConnectionString
                        = MySqlConnectionStringHelper.Validate("server=localhost;port=3306;database=librame_data_writing;user=root;password=123456");
                    dependency.Options.DefaultTenant.WritingSeparation = true;
                })
                .AddAccessor<TestDbContextAccessor>((tenant, optionsBuilder) =>
                {
                    optionsBuilder.UseMySql(tenant.DefaultConnectionString, mySql =>
                    {
                        mySql.MigrationsAssembly(typeof(TestStoreHubTests).GetAssemblyDisplayName());
                        mySql.ServerVersion(new Version(5, 7, 28), ServerType.MySql);
                    });
                })
                .AddDatabaseDesignTime<MySqlDesignTimeServices>()
                .AddStoreHub<TestStoreHub>()
                .AddStoreIdentifier<TestStoreIdentifier>()
                .AddStoreInitializer<TestStoreInitializer>();

            var serviceProvider = services.BuildServiceProvider();
            var stores = serviceProvider.GetRequiredService<TestStoreHub>();

            var dependencies = stores.ServiceFactory.GetService<MigrationsScaffolderDependencies>();
            Assert.NotNull(dependencies);

            var categories = stores.GetCategories();
            Assert.Empty(categories);

            categories = stores.UseWriteDbConnection().GetCategories();
            Assert.NotEmpty(categories);

            var articles = stores.UseDefaultDbConnection().GetArticles();
            Assert.Empty(articles);

            articles = stores.UseWriteDbConnection().GetArticles();
            Assert.True(articles.Total >= 0); // 如果已分表，则此表内容可能为空
        }


        [Fact]
        public void SqlServerTest()
        {
            var services = new ServiceCollection();

            services.AddLibrame()
                .AddData(dependency =>
                {
                    // SqlServer (Default)
                    //dependency.Options.SUIDGenerator = SequentialUniqueIdentifierGenerator.SqlServer;

                    dependency.Options.DefaultTenant.DefaultConnectionString
                        = "Data Source=.;Initial Catalog=librame_data_default;Integrated Security=True";
                    dependency.Options.DefaultTenant.WritingConnectionString
                        = "Data Source=.;Initial Catalog=librame_data_writing;Integrated Security=True";
                    dependency.Options.DefaultTenant.WritingSeparation = true;
                })
                .AddAccessor<TestDbContextAccessor>((tenant, optionsBuilder) =>
                {
                    optionsBuilder.UseSqlServer(tenant.DefaultConnectionString,
                        sqlServer => sqlServer.MigrationsAssembly(typeof(TestStoreHubTests).GetAssemblyDisplayName()));
                })
                .AddDatabaseDesignTime<SqlServerDesignTimeServices>()
                .AddStoreHub<TestStoreHub>()
                .AddStoreIdentifier<TestStoreIdentifier>()
                .AddStoreInitializer<TestStoreInitializer>();

            var serviceProvider = services.BuildServiceProvider();
            var stores = serviceProvider.GetRequiredService<TestStoreHub>();

            var dependencies = stores.ServiceFactory.GetService<MigrationsScaffolderDependencies>();
            Assert.NotNull(dependencies);

            var categories = stores.GetCategories();
            Assert.Empty(categories);

            categories = stores.UseWriteDbConnection().GetCategories();
            Assert.NotEmpty(categories);

            var articles = stores.UseDefaultDbConnection().GetArticles();
            Assert.Empty(articles);

            articles = stores.UseWriteDbConnection().GetArticles();
            Assert.True(articles.Total >= 0); // 如果已分表，则此表内容可能为空
        }


        [Fact]
        public void SqliteTest()
        {
            var services = new ServiceCollection();

            services.AddLibrame()
                .AddData(dependency =>
                {
                    dependency.Options.SUIDGenerator = new SequentialUniqueIdentifierGenerator(SequentialUniqueIdentifierType.AsString);

                    dependency.Options.DefaultTenant.DefaultConnectionString
                        = "Data Source=" + dependency.BaseDirectory.CombinePath("librame_data_default.db");
                    dependency.Options.DefaultTenant.WritingConnectionString
                        = "Data Source=" + dependency.BaseDirectory.CombinePath("librame_data_writing.db");
                    dependency.Options.DefaultTenant.WritingSeparation = true;
                })
                .AddAccessor<TestDbContextAccessor>((tenant, optionsBuilder) =>
                {
                    optionsBuilder.UseSqlite(tenant.DefaultConnectionString,
                        sqlite => sqlite.MigrationsAssembly(typeof(TestStoreHubTests).GetAssemblyDisplayName()));
                })
                .AddDatabaseDesignTime<SqliteDesignTimeServices>()
                .AddStoreHub<TestStoreHub>()
                .AddStoreIdentifier<TestStoreIdentifier>()
                .AddStoreInitializer<TestStoreInitializer>();

            var serviceProvider = services.BuildServiceProvider();
            var stores = serviceProvider.GetRequiredService<TestStoreHub>();

            var dependencies = stores.ServiceFactory.GetService<MigrationsScaffolderDependencies>();
            Assert.NotNull(dependencies);

            var categories = stores.GetCategories();
            Assert.Empty(categories);

            categories = stores.UseWriteDbConnection().GetCategories();
            Assert.NotEmpty(categories);

            var articles = stores.UseDefaultDbConnection().GetArticles();
            Assert.Empty(articles);

            articles = stores.UseWriteDbConnection().GetArticles();
            Assert.True(articles.Total >= 0); // 如果已分表，则此表内容可能为空
        }

    }
}
