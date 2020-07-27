using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite.Design.Internal;
using Microsoft.EntityFrameworkCore.SqlServer.Design.Internal;
using Microsoft.Extensions.DependencyInjection;
using Pomelo.EntityFrameworkCore.MySql.Design.Internal;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Librame.Extensions.Data.Tests
{
    using Extensions.Core.Identifiers;
    using Extensions.Core.Services;
    using Extensions.Data.Builders;
    using Extensions.Data.Stores;
    using Librame.Extensions.Core;
    using Models;

    public class TestStoreHubTests
    {
        [Fact]
        public void MySqlTest()
        {
            // Initialize Database Test: 6s
            // Default Sharding Test: 5s
            var services = new ServiceCollection();

            services
                .AddLibrame(dependency =>
                {
                    dependency.Options.Identifier.GuidIdentifierGenerator = CombIdentityGenerator.MySQL;
                })
                .AddData(dependency =>
                {
                    dependency.Options.DefaultTenant.DefaultConnectionString
                        = MySqlConnectionStringHelper.Validate("server=localhost;port=3306;database=librame_data_default;user=root;password=123456;");
                    dependency.Options.DefaultTenant.WritingConnectionString
                        = MySqlConnectionStringHelper.Validate("server=localhost;port=3306;database=librame_data_writing;user=root;password=123456;");
                    
                    dependency.Options.DefaultTenant.WritingSeparation = true;
                    dependency.Options.DefaultTenant.DataSynchronization = true;
                    dependency.Options.DefaultTenant.StructureSynchronization = true;
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
                .AddStoreIdentifierGenerator<TestGuidStoreIdentityGenerator>()
                .AddStoreInitializer<TestStoreInitializer>();

            TestStores(services.BuildServiceProvider());
        }


        [Fact]
        public void SqlServerTest()
        {
            // Initialize Database Test: 27s
            // Default Sharding Test: 5s
            var services = new ServiceCollection();

            services
                .AddLibrame(dependency =>
                {
                    // SQLServer (Default)
                    //dependency.Options.Identifier.GuidIdentifierGenerator = CombIdentityGenerator.SQLServer;
                })
                .AddData(dependency =>
                {
                    dependency.Options.DefaultTenant.DefaultConnectionString
                        = "Data Source=.;Initial Catalog=librame_data_default;Integrated Security=True";
                    dependency.Options.DefaultTenant.WritingConnectionString
                        = "Data Source=.;Initial Catalog=librame_data_writing;Integrated Security=True";

                    dependency.Options.DefaultTenant.WritingSeparation = true;
                    dependency.Options.DefaultTenant.DataSynchronization = true;
                    dependency.Options.DefaultTenant.StructureSynchronization = true;

                    dependency.Options.MigrationAssemblyReferences.Add(
                        AssemblyReference.Load("Microsoft.EntityFrameworkCore.SqlServer"));
                })
                .AddAccessor<TestDbContextAccessor>((tenant, optionsBuilder) =>
                {
                    optionsBuilder.UseSqlServer(tenant.DefaultConnectionString,
                        sqlServer => sqlServer.MigrationsAssembly(typeof(TestStoreHubTests).GetAssemblyDisplayName()));
                })
                .AddDatabaseDesignTime<SqlServerDesignTimeServices>()
                .AddStoreHub<TestStoreHub>()
                .AddStoreIdentifierGenerator<TestGuidStoreIdentityGenerator>()
                .AddStoreInitializer<TestStoreInitializer>();

            TestStores(services.BuildServiceProvider());
        }


        [Fact]
        public void SqliteTest()
        {
            // Initialize Database Test: 5s
            // Default Sharding Test: 5s
            var services = new ServiceCollection();

            services
                .AddLibrame(dependency =>
                {
                    dependency.Options.Identifier.GuidIdentifierGenerator = CombIdentityGenerator.SQLite;
                })
                .AddData(dependency =>
                {
                    dependency.Options.DefaultTenant.DefaultConnectionString
                        = "Data Source=" + dependency.DatabasesConfigDierctory.CombinePath("librame_data_default.db");
                    dependency.Options.DefaultTenant.WritingConnectionString
                        = "Data Source=" + dependency.DatabasesConfigDierctory.CombinePath("librame_data_writing.db");

                    dependency.Options.DefaultTenant.WritingSeparation = true;
                    dependency.Options.DefaultTenant.DataSynchronization = true;
                    dependency.Options.DefaultTenant.StructureSynchronization = true;
                })
                .AddAccessor<TestDbContextAccessor>((tenant, optionsBuilder) =>
                {
                    optionsBuilder.UseSqlite(tenant.DefaultConnectionString,
                        sqlite => sqlite.MigrationsAssembly(typeof(TestStoreHubTests).GetAssemblyDisplayName()));
                })
                .AddDatabaseDesignTime<SqliteDesignTimeServices>()
                .AddStoreHub<TestStoreHub>()
                .AddStoreIdentifierGenerator<TestGuidStoreIdentityGenerator>()
                .AddStoreInitializer<TestStoreInitializer>();

            TestStores(services.BuildServiceProvider());
        }


        private void TestStores(ServiceProvider serviceProvider)
        {
            var stores = serviceProvider.GetRequiredService<TestStoreHub>();
            var dependency = serviceProvider.GetRequiredService<DataBuilderDependency>();

            // DataStores
            var audits = stores.GetAudits();
            VerifyDefaultData(audits);

            audits = stores.UseWriteDbConnection().GetAudits();
            Assert.NotEmpty(audits);

            var auditProperties = stores.UseDefaultDbConnection().GetAuditProperties();
            VerifyDefaultData(auditProperties);

            auditProperties = stores.UseWriteDbConnection().GetAuditProperties();
            Assert.NotEmpty(auditProperties);

            var entities = stores.UseDefaultDbConnection().GetEntities();
            VerifyDefaultData(entities);

            entities = stores.UseWriteDbConnection().GetEntities();
            Assert.NotEmpty(entities);

            var migrations = stores.UseDefaultDbConnection().GetMigrations();
            VerifyDefaultData(migrations);

            migrations = stores.UseWriteDbConnection().GetMigrations();
            Assert.NotEmpty(migrations);

            var tenants = stores.UseDefaultDbConnection().GetTenants();
            VerifyDefaultData(tenants);

            tenants = stores.UseWriteDbConnection().GetTenants();
            Assert.NotEmpty(tenants);

            // TestStores
            var categories = stores.UseDefaultDbConnection().GetCategories();
            VerifyDefaultData(categories);

            categories = stores.UseWriteDbConnection().GetCategories();
            Assert.NotEmpty(categories);

            var articles = stores.UseDefaultDbConnection().GetArticles();
            if (dependency.Options.DefaultTenant.DataSynchronization)
                Assert.True(articles.Total >= 0); // 如果已分表，则此表内容可能为空
            else
                Assert.True(articles.IsEmpty());

            articles = stores.UseWriteDbConnection().GetArticles();
            Assert.True(articles.Total >= 0); // 如果已分表，则此表内容可能为空


            // 独立测试分表文章（需先自行更改 TestDbContextAccessor 文章分表的年份再进行测试）
            //AddShardingArticle(serviceProvider, stores, categories);


            void VerifyDefaultData<TEntity>(IEnumerable<TEntity> items)
                where TEntity : class
            {
                Assert.True(dependency.Options.DefaultTenant.DataSynchronization
                    ? items.IsNotEmpty()
                    : items.IsEmpty());
            }
        }


        private void AddShardingArticle(ServiceProvider serviceProvider, TestStoreHub stores,
            IList<Category<int, Guid>> categories)
        {
            var clock = serviceProvider.GetRequiredService<IClockService>();
            var identifierGenerator = serviceProvider.GetRequiredService<TestGuidStoreIdentityGenerator>();

            var categoryId = categories.First().Id;
            var shardingArticle = "Sharding Article";

            stores.Accessor.ArticlesManager.TryAdd(p => p.CategoryId == categoryId && p.Title == shardingArticle,
                () =>
                {
                    var article = new Article<Guid, int, Guid>
                    {
                        Id = identifierGenerator.GenerateArticleId(),
                        Title = "Sharding Article",
                        Descr = "Descr Sharding Article",
                        CategoryId = categories.First().Id
                    };

                    article.PopulateCreation(clock);

                    return article;
                },
                addedPost =>
                {
                    if (!stores.Accessor.RequiredSaveChanges)
                        stores.Accessor.RequiredSaveChanges = true;
                });

            if (stores.Accessor.RequiredSaveChanges)
                stores.Accessor.SaveChanges();
        }

    }
}
