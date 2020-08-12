Librame.Extensions 系列
=========================

[![License](https://img.shields.io/badge/License-Apache%202.0-blue.svg)](https://github.com/librame/extensions/blob/master/LICENSE)
[![Available on NuGet https://www.nuget.org/packages?q=Librame.Extensions](https://img.shields.io/nuget/v/Librame.Extensions.svg?style=flat-square)](https://www.nuget.org/packages?q=Librame.Extensions)

简体中文 | [English](/README-EN.md)

Librame.Extensions 是一个基于 .NET Standard/Framework 的基础工具系列库，主要包括核心、数据、图画、加密、网络、存储等方面。

| 库                                           | .NET Standard 2.1  | .NET Framework 4.8     |
|--------------------------------------------  |------------------  |----------------------  |
| Librame.Extensions.Core                      | ✓                  | ✓                      |
| Librame.Extensions.Data.EntityFrameworkCore  | ✓                  | ! compatible           |
| Librame.Extensions.Drawing.SkiaSharp         | ✓                  | ! compiled to x86/x64  |
| Librame.Extensions.Encryption                | ✓                  | ✓                      |
| Librame.Extensions.Network                   | ✓                  | ✓                      |
| Librame.Extensions.Network.DotNetty          | ✓                  | ✓                      |
| Librame.Extensions.Storage                   | ✓                  | ✓                      |

## 开始

Librame.Extensions APIs 可以使用 NuGet 包管理器添加到项目中。官方发布在 [NuGet](https://www.nuget.org/packages?q=Librame.Extensions) 上。

## 特点

Librame.Extensions.Core (and Core.Abstractions)

- [x] Identifiers: 有序唯一标识符, 雪花标识符 (Core.Abstractions)
- [x] Localizers (扩展支持 lambda 表达式)
- [x] Mediators (基于 MediatR)
- [x] Services: Clock, Humanization, Injection

Librame.Extensions.Data.EntityFrameworkCore (and Data.Abstractions)

- [x] Accessors (DbContextAccessor): 多租户, 写入分离, 迁移, 数据审计, 实体/表管理
- [x] Collections: IPageable, ITreeable (Data.Abstractions)
- [x] Stores: GuidStoreIdentifierGenerator, GuidStoreInitializer, StoreHub

Librame.Extensions.Drawing.SkiaSharp (and Drawing.Abstractions)

- [x] Services: Captcha, Scale, Watermark

Librame.Extensions.Encryption (and Encryption.Abstractions)

- [x] Services: Hash, KeyedHash, Rsa, Symmetric
- [x] SigningCredentials (支持由 IdentityServer4 生成的临时密钥文件)

Librame.Extensions.Network (and Network.Abstractions)

- [x] Requesters: HttpClient, HttpWebRequest
- [x] Services: Crawler, Email, Sms

Librame.Extensions.Network.DotNetty

- [x] Demo (Based on DotNetty): Discard, Echo, Factorial, HttpServer, QuoteOfTheMoment, SecureChat, Telnet, WebSocket

Librame.Extensions.Storage (and Storage.Abstractions)

- [x] Capacities: Binary, Decimal (Storage.Abstractions)
- [x] Services: PhysicalStorageFile (Read/Write), FileTransfer (Download/Upload), FilePermission (AccessToken/AuthorizationCode/CookieValue)

## 如何使用

以 Librame.Extensions.Data.EntityFrameworkCore 为例：

    PM> Install-Package: Librame.Extensions.Data.EntityFrameworkCore

### 配置

    // appsettings.json
    {
        // 测试1: SQLite
        "ConnectionStrings": {
            "DefaultConnectionString": "librame_data_default.db",
            "WritingConnectionString": "librame_data_writing.db"
        },
        // 测试2: MySQL
        "DefaultTenant": {
            "Name": "DefaultTenant",
            "Host": "localhost",
            "EncryptedConnectionStrings": false,
            "DefaultConnectionString": "server=localhost;port=3306;database=librame_data_default;user=root;password=123456",
            "WritingConnectionString": "server=localhost;port=3306;database=librame_data_writing;user=root;password=123456",
            "WritingSeparation": true,
            "DataSynchronization": true,
            "StructureSynchronization": true
        },
        // 测试3: SQL Server (默认支持)
        "DataBuilderDependency": {
            // DataBuilderOptions
            "Options": {
                "DefaultTenant": {
                    "Name": "DefaultTenant",
                    "Host": "localhost",
                    "DefaultConnectionString": "Data Source=.;Initial Catalog=librame_data_default;Integrated Security=True",
                    "WritingConnectionString": "Data Source=.;Initial Catalog=librame_data_writing;Integrated Security=True",
                    "WritingSeparation": true,
                    "DataSynchronization": true,
                    "StructureSynchronization": true
                }
            }
        }
    }

    //var root = new ConfigurationBuilder()
    //  .SetBasePath("BasePath")
    //  .AddJsonFile("appsettings.json")
    //  .Build();

    var builder = new ServiceCollection()
        .AddLibrame(dependency =>
        {
            // Default Support "appsettings.json"
            //dependency.ConfigurationRoot = root;
        });

### 使用 MySQL

    PM> Install-Package: Pomelo.EntityFrameworkCore.MySql

    var services = builder
        .AddLibrame(dependency =>
        {
            dependency.Options.Identifier.GuidIdentificationGenerator = CombIdentificationGenerator.MySQL;
        })
        .AddData(dependency =>
        {
            dependency.BindDefaultTenant(MySqlConnectionStringHelper.Validate);
        })
        .AddAccessor<TestDbContextAccessor>((tenant, optionsBuilder) =>
        {
            optionsBuilder.UseMySql(tenant.DefaultConnectionString, mySql =>
            {
                mySql.MigrationsAssembly(typeof(Program).GetAssemblyDisplayName());
                mySql.ServerVersion(new Version(5, 7, 28), ServerType.MySql);
            });
        })
        .AddDatabaseDesignTime<MySqlDesignTimeServices>()
        .AddStoreHub<TestStoreHub>()
        .AddStoreIdentifierGenerator<TestGuidStoreIdentifierGenerator>()
        .AddStoreInitializer<TestStoreInitializer>()
        .BuildServiceProvider();

### 使用 SQL Server

    PM> Install-Package: Microsoft.EntityFrameworkCore.SqlServer

    var services = builder
        .AddLibrame(dependency =>
        {
            // SQLServer (Default)
            //dependency.Options.Identifier.GuidIdentificationGenerator = CombIdentificationGenerator.SQLServer;
        })
        .AddData(dependency =>
        {
            dependency.Options.MigrationAssemblyReferences.Add(
                AssemblyReference.Load("Microsoft.EntityFrameworkCore.SqlServer"));
        })
        .AddAccessor<TestDbContextAccessor>((tenant, optionsBuilder) =>
        {
            optionsBuilder.UseSqlServer(tenant.DefaultConnectionString,
                sqlServer => sqlServer.MigrationsAssembly(typeof(Program).GetAssemblyDisplayName()));
        })
        .AddDatabaseDesignTime<SqlServerDesignTimeServices>()
        .AddStoreHub<TestStoreHub>()
        .AddStoreIdentifierGenerator<TestGuidStoreIdentifierGenerator>()
        .AddStoreInitializer<TestStoreInitializer>()
        .BuildServiceProvider();

### 使用 SQLite

    PM> Install-Package: Microsoft.EntityFrameworkCore.Sqlite

    var services = builder
        .AddLibrame(dependency =>
        {
            dependency.Options.Identifier.GuidIdentificationGenerator = CombIdentificationGenerator.SQLite;
        })
        .AddData(dependency =>
        {
            // ConnectionStrings Section is not support DefaultTenant.WritingSeparation
            dependency.Options.DefaultTenant.WritingSeparation = true;
            dependency.Options.DefaultTenant.DataSynchronization = true;
            dependency.Options.DefaultTenant.StructureSynchronization = true;
            
            dependency.BindConnectionStrings(dataFile => "Data Source=" + dependency.BaseDirectory.CombinePath(dataFile));
        })
        .AddAccessor<TestDbContextAccessor>((tenant, optionsBuilder) =>
        {
            optionsBuilder.UseSqlite(tenant.DefaultConnectionString,
                sqlite => sqlite.MigrationsAssembly(typeof(Program).GetAssemblyDisplayName()));
        })
        .AddDatabaseDesignTime<SqliteDesignTimeServices>()
        .AddStoreHub<TestStoreHub>()
        .AddStoreIdentifierGenerator<TestGuidStoreIdentifierGenerator>()
        .AddStoreInitializer<TestStoreInitializer>()
        .BuildServiceProvider();

### 创建模型与访问器

    [Description("文章")]
    [ShardingTable]
    public class Article : AbstractEntityCreation<string>
    {
        public string Title { get; set; }
        public string Descr { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }

    [Description("分类")]
    public class Category : AbstractEntityCreation<int>
    {
        public string Name { get; set; }

        public IList<Article> Articles { get; set; }
            = new List<Article>();
    }

    public class TestDbContextAccessor : DbContextAccessor
    {
        public TestDbContextAccessor(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Article> Articles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>(b =>
            {
                b.ToTable();
                
                b.HasKey(x => x.Id);
                
                b.Property(x => x.Id).ValueGeneratedOnAdd();
                b.Property(x => x.Name).HasMaxLength(256).IsRequired();

                b.HasMany(x => x.Articles).WithOne(x => x.Category)
                    .IsRequired().OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Article>(b =>
            {
                b.ToTable(table =>
                {
                    // 通过年份分表 (需要在 Article 中添加 [ShardingTable] 特性)
                    table.AppendYearSuffix(CurrentTimestamp);
                });

                b.HasKey(x => x.Id);

                b.Property(x => x.Id).HasMaxLength(256);
                b.Property(x => x.Title).HasMaxLength(256).IsRequired();
            });
        }
    }

### 创建存储

    public class TestGuidStoreIdentifierGenerator : GuidStoreIdentifierGenerator
    {
        public TestStoreIdentifier(IOptions<DataBuilderOptions> options,
            IClockService clock, ILoggerFactory loggerFactory)
            : base(options, clock, loggerFactory)
        {
        }
        
        public Task<Guid> GetArticleIdAsync(CancellationToken cancellationToken = default)
            => GenerateIdAsync("ArticleId", cancellationToken);
    }

    public class TestStoreInitializer : GuidStoreInitializer
    {
        // 参见 tests/Librame.Extensions.Data.EntityFrameworkCore.Tests/TestStoreInitializer.cs
    }

    public class TestStoreHub : StoreHub<TestDbContextAccessor>
    {
        public TestStore(IStoreInitializer initializer, IAccessor accessor)
            : base(initializer, accessor)
        {
        }

        public IList<Category> GetCategories()
            => Accessor.Categories.ToList();

        public IPageable<Article> GetArticles()
            => Accessor.Articles.AsDescendingPagingByIndex(1, 10);

        public TestStoreHub UseWriteDbConnection()
        {
            Accessor.SwitchTenant(t => t.WritingConnectionString);
            return this;
        }

        public TestStoreHub UseDefaultDbConnection()
        {
            Accessor.SwitchTenant(t => t.DefaultConnectionString);
            return this;
        }
    }

    var stores = services.GetRequiredService<TestStoreHub>()
    var categories = stores.GetCategories();
    Assert.Empty(categories);

    categories = stores.UseWriteDbConnection().GetCategories();
    Assert.NotEmpty(categories);

    var articles = stores.UseDefaultDbConnection().GetArticles();
    Assert.Empty(articles);

    articles = stores.UseWriteDbConnection().GetArticles();
    Assert.True(articles.Total >= 0); // 如果启用分片表，文章表可能为空。

### 已测试的数据库

| 数据库       | 多租户         | 写入分离           | 迁移       | 审计   | 实体/表管理              |
|------------  |--------------  |------------------  |----------  |------  |------------------------  |
| MySQL        | ✓              | ✓                  | ✓          | ✓      | ✓                        |
| SQL Server   | ✓              | ✓                  | ✓          | ✓      | ✓                        |
| SQLite       | ✓              | ✓                  | ✓          | ✓      | ✓                        |

**MySQL** [测试于 MySQL 5.7](/examples/Librame.Extensions.Examples/Tested_in_MySQL57.png).

**SQL Server** [测试于 SQL Server 2019](/examples/Librame.Extensions.Examples/Tested_in_SQLServer2019.png).

**SQLite** [测试于 SQLite](/examples/Librame.Extensions.Examples/Tested_in_SQLite.png).
