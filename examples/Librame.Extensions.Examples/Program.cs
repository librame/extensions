using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite.Design.Internal;
using Microsoft.EntityFrameworkCore.SqlServer.Design.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pomelo.EntityFrameworkCore.MySql.Design.Internal;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using System;

namespace Librame.Extensions.Examples
{
    using Core.Builders;
    using Core.Identifiers;
    using Core.Options;
    using Data.Builders;

    class Program
    {
        static void Main(string[] args)
        {
            // Add NLog Configuration
            NLog.LogManager.LoadConfiguration("../../../nlog.config");

            Console.WriteLine("Hello, Librame Pong!");
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();

            RunMySql();

            RunSqlServer();

            RunSqlite();

            // Close NLog
            NLog.LogManager.Shutdown();
        }

        static void RunMySql()
        {
            var builder = CreateBuilder(options => options.GuidIdentificationGenerator = CombIdentificationGenerator.MySQL)
                .AddData(dependency =>
                {
                    // for MySQL
                    dependency.BindDefaultTenant(MySqlConnectionStringHelper.Validate);
                })
                .AddAccessor<ExampleDbContextAccessor>((tenant, optionsBuilder) =>
                {
                    // for MySQL
                    optionsBuilder.UseMySql(tenant.DefaultConnectionString, mySql =>
                    {
                        mySql.MigrationsAssembly(typeof(Program).GetAssemblyDisplayName());
                        mySql.ServerVersion(new Version(5, 7, 28), ServerType.MySql);
                    });
                })
                // for MySQL
                .AddDatabaseDesignTime<MySqlDesignTimeServices>()
                .AddStoreHub<ExampleStoreHub>()
                .AddStoreIdentifierGenerator<ExampleStoreIdentifierGenerator>()
                .AddStoreInitializer<ExampleStoreInitializer>();

            var provider = builder.Services.BuildServiceProvider();
            DisplayData(provider, "MySql");
        }

        static void RunSqlServer()
        {
            var builder = CreateBuilder()
                .AddData(dependency =>
                {
                    //dependency.Options.MigrationAssemblyReferences.Add(
                    //    AssemblyDescriptor.Create("Microsoft.EntityFrameworkCore.SqlServer"));
                })
                .AddAccessor<ExampleDbContextAccessor>((tenant, optionsBuilder) =>
                {
                    // for SqlServer
                    optionsBuilder.UseSqlServer(tenant.DefaultConnectionString,
                        sqlServer => sqlServer.MigrationsAssembly(typeof(Program).GetAssemblyDisplayName()));
                })
                // for SqlServer
                .AddDatabaseDesignTime<SqlServerDesignTimeServices>()
                .AddStoreHub<ExampleStoreHub>()
                .AddStoreIdentifierGenerator<ExampleStoreIdentifierGenerator>()
                .AddStoreInitializer<ExampleStoreInitializer>();

            var provider = builder.Services.BuildServiceProvider();
            DisplayData(provider, "SqlServer");
        }

        static void RunSqlite()
        {
            var builder = CreateBuilder(options => options.GuidIdentificationGenerator = CombIdentificationGenerator.SQLite)
                .AddData(dependency =>
                {
                    // for SQLite
                    dependency.BindConnectionStrings(dataFile => "Data Source=" + dependency.DatabasesConfigDierctory.CombinePath(dataFile));

                    // ConnectionStrings 配置节点不支持 DefaultTenant 配置，须手动启用读写分离
                    dependency.Options.DefaultTenant.WritingSeparation = true;
                    dependency.Options.DefaultTenant.DataSynchronization = true;
                    dependency.Options.DefaultTenant.StructureSynchronization = true;
                })
                .AddAccessor<ExampleDbContextAccessor>((tenant, optionsBuilder) =>
                {
                    // for SQLite
                    optionsBuilder.UseSqlite(tenant.DefaultConnectionString,
                        sqlite => sqlite.MigrationsAssembly(typeof(Program).GetAssemblyDisplayName()));
                })
                // for SQLite
                .AddDatabaseDesignTime<SqliteDesignTimeServices>()
                .AddStoreHub<ExampleStoreHub>()
                .AddStoreIdentifierGenerator<ExampleStoreIdentifierGenerator>()
                .AddStoreInitializer<ExampleStoreInitializer>();

            var provider = builder.Services.BuildServiceProvider();
            DisplayData(provider, "Sqlite");
        }

        static void DisplayData(IServiceProvider provider, string databaseName)
        {
            Console.WriteLine($"Run {databaseName} database test:");

            // Write Tenant
            var tenant = provider.GetRequiredService<IOptions<DataBuilderOptions>>().Value.DefaultTenant;

            Console.WriteLine($"Current tenant name: {tenant.Name}.");
            Console.WriteLine($"Current tenant host: {tenant.Host}.");
            Console.WriteLine($"Current tenant EncryptedConnectionStrings: {tenant.EncryptedConnectionStrings}.");
            Console.WriteLine($"Current tenant DefaultConnectionString: {tenant.DefaultConnectionString}.");
            Console.WriteLine($"Current tenant WritingConnectionString: {tenant.WritingConnectionString}.");
            Console.WriteLine($"Current tenant WritingSeparation: {tenant.WritingSeparation}.");

            // Write Data
            var stores = provider.GetRequiredService<ExampleStoreHub>();

            var categories = stores.GetCategories();
            Console.WriteLine($"Default database categories is empty: {categories.IsEmpty()}.");

            categories = stores.UseWriteDbConnection().GetCategories();
            Console.WriteLine($"Writing database categories is empty: {categories.IsEmpty()}.");
            categories.ForEach(category => Console.WriteLine(category));

            var articles = stores.UseDefaultDbConnection().GetArticles();
            Console.WriteLine($"Default database articles is empty: {articles.IsEmpty()}.");

            articles = stores.UseWriteDbConnection().GetArticles();
            Console.WriteLine($"Writing database articles is empty: {articles.IsEmpty()}."); // 如果已分表，则此表内容可能为空

            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
        }

        static ICoreBuilder CreateBuilder(Action<IdentifierOptions> configureOptions = null)
        {
            //var basePath = AppContext.BaseDirectory.WithoutDevelopmentRelativePath();
            //var root = new ConfigurationBuilder()
            //    .SetBasePath(basePath)
            //    .AddJsonFile("appsettings.json")
            //    .Build();

            var services = new ServiceCollection();

            return services.AddLibrame<ExampleCoreBuilderDependency>(dependency =>
            {
                // appsettings.json (default)
                //dependency.ConfigurationRoot = root;

                dependency.ConfigureLoggingBuilder = logging =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(LogLevel.Trace);

                    logging.AddConsole(logger => logger.IncludeScopes = false);
                    logging.AddFilter((str, level) => true);
                };

                configureOptions?.Invoke(dependency.Options.Identifier);
            });
        }

    }

}
