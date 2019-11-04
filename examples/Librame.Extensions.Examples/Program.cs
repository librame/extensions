using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite.Design.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pomelo.EntityFrameworkCore.MySql.Design.Internal;
using System;

namespace Librame.Extensions.Examples
{
    using Core;
    using Data;
    using Encryption;

    class Program
    {
        static void Main(string[] args)
        {
            // Add NLog Configuration
            NLog.LogManager.LoadConfiguration("../../../nlog.config");

            var basePath = AppContext.BaseDirectory.WithoutDevelopmentRelativePath();
            var root = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json")
                .Build();

            var services = new ServiceCollection();

            services.AddLibrame<ExampleCoreBuilderDependencyOptions>(dependency =>
            {
                dependency.Configuration = root.GetSection(dependency.Name);

                dependency.LoggingAction = logging =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(LogLevel.Trace);

                    logging.AddConsole(logger => logger.IncludeScopes = false);
                    logging.AddFilter((str, level) => true);
                };
            })
            .AddData(options =>
            {
                // SQLite
                options.DefaultTenant.DefaultConnectionString = "Data Source=" + basePath.CombinePath("librame_data_default.db");
                options.DefaultTenant.WritingConnectionString = "Data Source=" + basePath.CombinePath("librame_data_writing.db");

                // MySQL
                //options.DefaultTenant.DefaultConnectionString = "Server=localhost;Database=librame_data_default;User=root;Password=123456;";
                //options.DefaultTenant.DefaultConnectionString = "Server=localhost;Database=librame_data_writing;User=root;Password=123456;";

                options.DefaultTenant.WritingSeparation = true;
            })
            .AddAccessor<ExampleDbContextAccessor>((options, optionsBuilder) =>
            {
                // SQLite
                optionsBuilder.UseSqlite(options.DefaultTenant.DefaultConnectionString,
                    sqlite => sqlite.MigrationsAssembly(typeof(Program).GetSimpleAssemblyName()));

                // MySQL
                //optionsBuilder.UseMySql(options.DefaultTenant.DefaultConnectionString,
                //    mysql => mysql.MigrationsAssembly(typeof(Program).GetSimpleAssemblyName()));
            })
            .AddDbDesignTime<SqliteDesignTimeServices>()
            //.AddDbDesignTime<MySqlDesignTimeServices>()
            .AddIdentifier<ExampleStoreIdentifier>()
            .AddInitializer<ExampleStoreInitializer>()
            .AddStoreHub<ExampleStoreHub>()
            .AddEncryption()
            .AddDeveloperGlobalSigningCredentials();
            
            var provider = services.BuildServiceProvider();

            RunHello(provider);

            // SQLite
            RunSqlite(provider);

            // MySQL
            //RunMySql(provider);

            RunEncryption(provider);

            // Close NLog
            NLog.LogManager.Shutdown();
        }

        private static void RunHello(IServiceProvider provider)
        {
            var options = provider.GetRequiredService<IOptions<ExampleOptions>>().Value;
            Console.WriteLine(options.Message);

            var dataOptions = provider.GetRequiredService<IOptions<DataBuilderOptions>>().Value;
            dataOptions.MigrationAssemblyReferences.ForEach((refer, i) =>
            {
                Console.WriteLine($"AssemblyReference {i + 1}: {refer.Location}");
            });

            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
        }

        private static void RunSqlite(IServiceProvider provider)
        {
            Console.WriteLine("Run sqlite database test:");

            using (var stores = provider.GetRequiredService<ExampleStoreHub>())
            {
                var categories = stores.GetCategories();
                Console.WriteLine($"Default database categories is empty: {categories.IsEmpty()}.");

                categories = stores.UseWriteDbConnection().GetCategories();
                Console.WriteLine($"Writing database categories is empty: {categories.IsEmpty()}.");
                categories.ForEach(category => Console.WriteLine(category));

                var articles = stores.UseDefaultDbConnection().GetArticles();
                Console.WriteLine($"Default database articles is empty: {articles.IsEmpty()}.");

                articles = stores.UseWriteDbConnection().GetArticles();
                Console.WriteLine($"Writing database articles is empty: {articles.IsEmpty()}."); // 如果已分表，则此表内容可能为空
                if (articles.IsNotEmpty())
                    articles.ForEach(article => Console.WriteLine(article));
            }

            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
        }

        private static void RunMySql(IServiceProvider provider)
        {
            Console.WriteLine("Run mysql database test:");

            using (var stores = provider.GetRequiredService<ExampleStoreHub>())
            {
                var categories = stores.GetCategories();
                Console.WriteLine($"Default database categories is empty: {categories.IsEmpty()}.");

                categories = stores.UseWriteDbConnection().GetCategories();
                Console.WriteLine($"Writing database categories is empty: {categories.IsEmpty()}.");
                categories.ForEach(category => Console.WriteLine(category));

                var articles = stores.UseDefaultDbConnection().GetArticles();
                Console.WriteLine($"Default database articles is empty: {articles.IsEmpty()}.");

                articles = stores.UseWriteDbConnection().GetArticles();
                Console.WriteLine($"Writing database articles is empty: {articles.IsEmpty()}."); // 如果已分表，则此表内容可能为空
                if (articles.IsNotEmpty())
                    articles.ForEach(article => Console.WriteLine(article));
            }

            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
        }

        private static void RunEncryption(IServiceProvider provider)
        {
            Console.WriteLine("Run encryption test:");

            Console.WriteLine("Please input some content:");

            var content = Console.ReadLine();
            if (content.IsWhiteSpace())
            {
                Console.WriteLine("Content is null, empty or white space.");
                RunEncryption(provider);
            }
            
            var hash = provider.GetRequiredService<IHashService>();
            var plaintextBuffer = content.AsPlaintextBuffer(provider);

            Console.WriteLine($"Content MD5: {hash.Md5(plaintextBuffer).AsBase64String()}");

            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
        }

    }

}
