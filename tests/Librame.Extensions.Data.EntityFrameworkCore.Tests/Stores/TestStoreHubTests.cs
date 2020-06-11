using Microsoft.EntityFrameworkCore;
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

    public class TestStoreHubTests
    {
        [Fact]
        public void MySqlTest()
        {
            // Initialize Database: 7s
            var services = new ServiceCollection();

            services.AddLibrame()
                .AddData(dependency =>
                {
                    dependency.Options.IdentifierGenerator = CombIdentifierGenerator.MySQL;

                    dependency.Options.DefaultTenant.DefaultConnectionString
                        = MySqlConnectionStringHelper.Validate("server=localhost;port=3306;database=librame_data_default;user=root;password=123456;");
                    dependency.Options.DefaultTenant.WritingConnectionString
                        = MySqlConnectionStringHelper.Validate("server=localhost;port=3306;database=librame_data_writing;user=root;password=123456;");
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
                .AddStoreIdentifierGenerator<TestGuidStoreIdentifierGenerator>()
                .AddStoreInitializer<TestStoreInitializer>();

            var serviceProvider = services.BuildServiceProvider();
            var stores = serviceProvider.GetRequiredService<TestStoreHub>();

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
            // Initialize Database: 27s
            var services = new ServiceCollection();

            services.AddLibrame()
                .AddData(dependency =>
                {
                    // SQLServer (Default)
                    //dependency.Options.IdentifierGenerator = CombIdentifierGenerator.SQLServer;

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
                .AddStoreIdentifierGenerator<TestGuidStoreIdentifierGenerator>()
                .AddStoreInitializer<TestStoreInitializer>();

            var serviceProvider = services.BuildServiceProvider();
            var stores = serviceProvider.GetRequiredService<TestStoreHub>();

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
            // Initialize Database: 3s
            var services = new ServiceCollection();

            services.AddLibrame()
                .AddData(dependency =>
                {
                    dependency.Options.IdentifierGenerator = CombIdentifierGenerator.SQLite;

                    dependency.Options.DefaultTenant.DefaultConnectionString
                        = "Data Source=" + dependency.DatabasesConfigDierctory.CombinePath("librame_data_default.db");
                    dependency.Options.DefaultTenant.WritingConnectionString
                        = "Data Source=" + dependency.DatabasesConfigDierctory.CombinePath("librame_data_writing.db");
                    dependency.Options.DefaultTenant.WritingSeparation = true;
                })
                .AddAccessor<TestDbContextAccessor>((tenant, optionsBuilder) =>
                {
                    optionsBuilder.UseSqlite(tenant.DefaultConnectionString,
                        sqlite => sqlite.MigrationsAssembly(typeof(TestStoreHubTests).GetAssemblyDisplayName()));
                })
                .AddDatabaseDesignTime<SqliteDesignTimeServices>()
                .AddStoreHub<TestStoreHub>()
                .AddStoreIdentifierGenerator<TestGuidStoreIdentifierGenerator>()
                .AddStoreInitializer<TestStoreInitializer>();

            var serviceProvider = services.BuildServiceProvider();
            var stores = serviceProvider.GetRequiredService<TestStoreHub>();

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
