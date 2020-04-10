Librame.Extensions
==================

[![License](https://img.shields.io/badge/License-Apache%202.0-blue.svg)](https://github.com/librame/extensions/blob/master/LICENSE)
[![Available on NuGet https://www.nuget.org/packages?q=Librame.Extensions](https://img.shields.io/nuget/v/Librame.Extensions.svg?style=flat-square)](https://www.nuget.org/packages?q=Librame.Extensions)

## Get Started

Librame.Extensions APIs can then be added to the project using the NuGet Package Manager. Official releases are on [NuGet](https://www.nuget.org/packages?q=Librame.Extensions).

## How to use

    // PM> Install-Package: Microsoft.Extensions.DependencyInjection
    var services = new ServiceCollection();
    
    // PM> Install-Package: Librame.Extensions.Core
    services.AddLibrame();
    
    // Build ServiceProvider
    var serviceProvider = services.BuildServiceProvider();

| Extensions Series                                                                                         | .NET Standard 2.1  | .NET Framework 4.8     |
|---------------------------------------------------------------------------------------------------------  |------------------  |----------------------  |
| [Core](https://www.nuget.org/packages?q=Librame.Extensions.Core)                                          | ✓                  | ✓                      |
| [Data.EntityFrameworkCore](https://www.nuget.org/packages?q=Librame.Extensions.Data.EntityFrameworkCore)  | ✓                  | !                      |
| [Drawing.SkiaSharp](https://www.nuget.org/packages?q=Librame.Extensions.Drawing.SkiaSharp)                | ✓                  | ! Compiled to x86/x64  |
| [Encryption](https://www.nuget.org/packages?q=Librame.Extensions.Encryption)                              | ✓                  | ✓                      |
| [Network](https://www.nuget.org/packages?q=Librame.Extensions.Network)                                    | ✓                  | ✓                      |
| [Network.DotNetty](https://www.nuget.org/packages?q=Librame.Extensions.Network.DotNetty)                  | ✓                  | ✓                      |
| [Storage](https://www.nuget.org/packages?q=Librame.Extensions.Storage)                                    | ✓                  | ✓                      |

**✓ Native, ! Compliant**

## Librame.Extensions.Core

    // PM> Install-Package: [Librame.Extensions.Core](https://www.nuget.org/packages?q=Librame.Extensions.Core)
    services.AddLibrame();

### Test Combiners

    // Librame.Extensions.Core.Tests.UriCombinerTests
    var urlString = "https://1.2.3.4.developer.microsoft.com/en-us/fabric#/get-started";
    var uriCombiner = uriString.AsUriCombiner();
    Assert.Equal("https", uriCombiner.Scheme);
    Assert.Equal("developer.microsoft.com", uriCombiner.Host);
    Assert.Equal("/en-us/fabric", uriCombiner.Path);
    
    // Librame.Extensions.Core.Tests.DomainNameCombinerTests
    var domainCombiner = uriString.GetHost().AsDomainNameCombiner(); // uriCombiner.Host
    Assert.Equal("com", domainCombiner.Root);
    Assert.Equal("microsoft.com", domainCombiner.TopLevel);
    Assert.Equal("developer.microsoft.com", domainCombiner.SecondLevel);

### Test Localizers

    // Librame.Extensions.Core.Tests.StringLocalizerTests
    var localizer = serviceProvider.GetRequiredService<IStringLocalizer<TestResource>>();
    Assert.True(localizer.GetString(r => r.Name).ResourceNotFound);

### Test Mediators (MediatR)

    // Librame.Extensions.Core.Tests.RequestPostProcessorBehaviorTests
    var mediator = serviceProvider.GetRequiredService<IMediator>();
    var response = await mediator.Send(new Ping { Message = "Ping" });
    Assert.Contains("Ping Pong Ping", response.Message);

## Librame.Extensions.Data.EntityFrameworkCore

    // appsettings.json
    {
        // ConnectionStrings 1
        "ConnectionStrings": {
            // ex. SQLite
            "DefaultConnectionString": "librame_data_default.db",
            "WritingConnectionString": "librame_data_writing.db"
        },
        // ConnectionStrings 2
        "DefaultTenant": {
            // ex. MySQL
            "Name": "DefaultTenant",
            "Host": "localhost",
            "DefaultConnectionString": "Server=localhost;Database=librame_data_default;User=root;Password=123456;",
            "WritingConnectionString": "Server=localhost;Database=librame_data_writing;User=root;Password=123456;",
            "WritingSeparation": true
        },
        "DataBuilderDependency": {
            // DataBuilderOptions
            "Options": {
                // ConnectionStrings 3 // default
                "DefaultTenant": {
                    // ex. SQL Server
                    "Name": "DefaultTenant",
                    "Host": "localhost",
                    "DefaultConnectionString": "Server=localhost;Database=librame_data_default;User=root;Password=123456;",
                    "WritingConnectionString": "Server=localhost;Database=librame_data_writing;User=root;Password=123456;",
                    "WritingSeparation": true
                }
            }
        }
    }
    
    // Configuration
    //var root = new ConfigurationBuilder()
    //.SetBasePath("BasePath")
    //.AddJsonFile("appsettings.json")
    //.Build();

    // PM> Install-Package: [Librame.Extensions.Data.EntityFrameworkCore](https://www.nuget.org/packages?q=Librame.Extensions.Data.EntityFrameworkCore)
    services.AddLibrame(dependency =>
        {
            // Default Support "appsettings.json"
            //dependency.ConfigurationRoot = root;
        })
        .AddData(options =>
        {
            // Default Support DataBuilderDependency Section (ex. SQL Server)
            
            // Support ConnectionStrings Section (ex. SQLite)
            //dependency.BindConnectionStrings(dataFile => dependency.BaseDirectory.CombinePath(dataFile));
            
            // Support DefaultTenant Section (ex. MySQL)
            //dependency.BindDefaultTenant(MySqlConnectionStringHelper.Validate);
        })
        .AddAccessor<TestDbContextAccessor>((tenant, optionsBuilder) =>
        {
            // for SQL Server
            // PM> Install-Package Microsoft.EntityFrameworkCore.SqlServer
            optionsBuilder.UseSqlServer(tenant.DefaultConnectionString,
                sqlServer => sqlServer.MigrationsAssembly("AssemblyName"));
            
            // for SQLite
            // PM> Install-Package Microsoft.EntityFrameworkCore.Sqlite
            //optionsBuilder.UseSqlite(tenant.DefaultConnectionString,
                //sqlite => sqlite.MigrationsAssembly("AssemblyName"));
            
            // for MySQL
            // PM> Install-Package Pomelo.EntityFrameworkCore.MySql
            //optionsBuilder.UseMySql(tenant.DefaultConnectionString, mySql =>
            //{
                //mySql.MigrationsAssembly("AssemblyName");
                //mySql.ServerVersion(new Version(5, 7, 28), ServerType.MySql);
            //});
        })
        // for SQL Server
        .AddDatabaseDesignTime<SqlServerDesignTimeServices>()
        // for SQLite
        //.AddDatabaseDesignTime<SqliteDesignTimeServices>()
        // for MySQL
        //.AddDatabaseDesignTime<MySqlDesignTimeServices>()
        .AddStoreIdentifier<TestStoreIdentifier>()
        //.AddStoreInitializer<TestStoreInitializer>()
        .AddStoreHub<TestStoreHub>();

| Database     | Multi-Tenancy  | WritingSeparation  | Migration  | Audit  | Entity/Table Management  |
|------------  |--------------  |------------------  |----------  |------  |------------------------  |
| SQL Server   | ✓              | ✓                  | ✓          | ✓      | ✓                        |
| SQLite       | ✓              | ✓                  | ✓          | ✓      | ✓                        |
| MySQL        | ✓              | ✓                  | ✓          | ✓      | ✓                        |

**SQLServer** [test in SQL Server 2019](https://raw.githubusercontent.com/librame/Librame/dev/examples/Librame.Extensions.Examples/Tested_in_SQLServer2016.png).

**SQLite** [test in SQLite](https://raw.githubusercontent.com/librame/Librame/dev/examples/Librame.Extensions.Examples/Tested_in_SQLite.png).

**MySQL** [test in MySQL 5.7](https://raw.githubusercontent.com/librame/Librame/dev/examples/Librame.Extensions.Examples/Tested_in_MySQL57.png).

## Test StoreHub

    // Article
    //[Description("Article")]
    [ShardingTable]
    public class Article : AbstractEntityCreation<string>
    {
        public string Title { get; set; }
        public string Descr { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
    
    // Category
    //[Description("Category")]
    public class Category : AbstractEntityCreation<int>
    {
        public string Name { get; set; }

        public IList<Article> Articles { get; set; }
            = new List<Article>();
    }
    
    // TestDbContextAccessor
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
                // Sharding table by year (use [ShardingTable])
                b.ToTable(descr => descr.ChangeDateOffsetSuffixByYear());

                b.HasKey(x => x.Id);

                b.Property(x => x.Id).HasMaxLength(256);
                b.Property(x => x.Title).HasMaxLength(256).IsRequired();
            });
        }
    }
    
    // TestStoreHub
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
    
    // Librame.Extensions.Data.Tests.DbContextAccessorTests
    var stores = serviceProvider.GetRequiredService<TestStoreHub>()
    var categories = stores.GetCategories();
    Assert.Empty(categories);

    categories = stores.UseWriteDbConnection().GetCategories();
    Assert.NotEmpty(categories);

    var articles = stores.UseDefaultDbConnection().GetArticles();
    Assert.Empty(articles);

    articles = stores.UseWriteDbConnection().GetArticles();
    Assert.True(articles.Total >= 0); // If enable sharding table, the articles of this table may be empty

## Librame.Extensions.Drawing.SkiaSharp

    // PM> Install-Package: [Librame.Extensions.Drawing.SkiaSharp](https://www.nuget.org/packages?q=Librame.Extensions.Drawing.SkiaSharp)
    services.AddLibrame()
        .AddDrawing();

### Test CaptchaService

    // Librame.Extensions.Drawing.Tests.CaptchaServiceTests
    var captchaService = serviceProvider.GetRequiredService<ICaptchaService>();
    var buffer = await captchaService.DrawBytesAsync("1234");
    Assert.NotNull(buffer);
    
    var saveFile = "captcha.png".AsFilePathCombiner("SavePath");
    var result = await captchaService.DrawFileAsync("1234", saveFile);
    Assert.True(result);

### Test ScaleService

    // Librame.Extensions.Drawing.Tests.ScaleServiceTests
    var scaleService = serviceProvider.GetRequiredService<IScaleService>();
    var imageFile = "test.jpg".AsFilePathCombiner("ImagePath");
    var result = scaleService.DrawFile(imageFile);
    Assert.True(result);
    
    var count = await scaleService.DrawFilesByDirectoryAsync("FolderPath");
    Assert.True(count > 0);

### Test WatermarkService

    // Librame.Extensions.Drawing.Tests.WatermarkServiceTests
    var watermarkService = serviceProvider.GetRequiredService<IWatermarkService>();
    var imageFile = "test.jpg".AsFilePathCombiner("ImagePath");
    var saveFile = imageFile.NewFileName("test-watermark.png");
    var result = await watermarkService.DrawFileAsync(imageFile, saveFile);
    Assert.True(result);

## Librame.Extensions.Encryption

    // PM> Install-Package: [Librame.Extensions.Encryption](https://www.nuget.org/packages?q=Librame.Extensions.Encryption)
    services.AddLibrame()
        .AddEncryption()
        .AddDeveloperGlobalSigningCredentials(); // AddSigningCredentials();

## Test Extension

    // Librame.Extensions.Encryption.Tests.BufferExtensionsTests
    var str = "TestString";
    var plaintextBuffer = str.AsPlaintextBuffer(serviceProvider);
    
    plaintextBuffer
        .UseHash((hash, buffer) =>
        {
            buffer = hash.Md5(buffer);
            buffer = hash.Sha1(buffer);
            buffer = hash.Sha256(buffer);
            buffer = hash.Sha384(buffer);
            return hash.Sha512(buffer);
        })
        .UseKeyedHash((keyedHash, buffer) =>
        {
            buffer = keyedHash.HmacMd5(buffer);
            buffer = keyedHash.HmacSha1(buffer);
            buffer = keyedHash.HmacSha256(buffer);
            buffer = keyedHash.HmacSha384(buffer);
            return keyedHash.HmacSha512(buffer);
        });
    
    var hashString = plaintextBuffer.AsBase64String();

    var algorithmBuffer = hashString.FromBase64StringAsAlgorithmBuffer(serviceProvider);
    Assert.True(plaintextBuffer.Equals(algorithmBuffer));

    algorithmBuffer
        .UseSymmetric((symmetric, buffer) =>
        {
            buffer = symmetric.EncryptDes(buffer);
            buffer = symmetric.EncryptTripleDes(buffer);
            return symmetric.EncryptAes(buffer);
        })
        .UseRsa((rsa, buffer) =>
        {
            return rsa.Encrypt(buffer);
        });

    Assert.False(plaintextBuffer.Equals(algorithmBuffer));

    algorithmBuffer
        .UseRsa((rsa, buffer) =>
        {
            return rsa.Decrypt(buffer);
        })
        .UseSymmetric((symmetric, buffer) =>
        {
            buffer = symmetric.DecryptAes(buffer);
            buffer = symmetric.DecryptTripleDes(buffer);
            return symmetric.DecryptDes(buffer);
        });

    Assert.True(plaintextBuffer.Equals(algorithmBuffer));

## Librame.Extensions.Network

    // PM> Install-Package: [Librame.Extensions.Network](https://www.nuget.org/packages?q=Librame.Extensions.Network)
    services.AddLibrame()
        .AddNetwork();

## Test Extension

    // Librame.Extensions.Network.Tests.CrawlerServiceTests
    var crawler = serviceProvider.GetRequiredService<ICrawlerService>();
    var content = await crawler.GetContentAsync("https://www.cnblogs.com");
    Assert.NotEmpty(content);
    
    var links = await crawler.GetHyperLinksAsync("https://www.baidu.com");
    Assert.NotEmpty(links);
    
    var images = await crawler.GetImageHyperLinks("https://www.baidu.com");
    Assert.NotEmpty(images);

    // Librame.Extensions.Network.Tests.EmailServiceTests
    var email = serviceProvider.GetRequiredService<IEmailService>();
    async email.SendAsync("receiver@domain.com",
        "Email Subject",
        "Email Body");
    
    // Librame.Extensions.Network.Tests.SmsServiceTests
    var sms = serviceProvider.GetRequiredService<ISmsService>();
    var result = async sms.SendAsync("13012345678", "TestData: 123456");
    Assert.Empty(result);

## Librame.Extensions.Network.DotNetty

    // PM> Install-Package: [Librame.Extensions.Network.DotNetty](https://www.nuget.org/packages?q=Librame.Extensions.Network.DotNetty)
    var combiner = "dotnetty.com.pfx".AsFilePathCombiner("BasePath");
    services.AddLibrame(logging =>
    {
        logging.ClearProviders();
        logging.SetMinimumLevel(LogLevel.Trace);

        logging.AddConsole(logger => logger.IncludeScopes = false);
        logging.AddFilter((str, level) => true);
    })
    .AddEncryption().AddGlobalSigningCredentials(new X509Certificate2(combiner, "password"))
    .AddNetwork().AddDotNetty();
    
    // Use DotNetty LoggerFactory
    //InternalLoggerFactory.DefaultFactory.AddProvider(new ConsoleLoggerProvider((s, level) => true, false));
    services.TryReplace(InternalLoggerFactory.DefaultFactory);

## Test Extension

    // Librame.Extensions.Network.DotNetty.WebSocket.Server
    var server = serviceProvider.GetRequiredService<IWebSocketServer>();
    server.StartAsync(async channel =>
    {
        Console.ReadLine();
        await channel.CloseAsync();
    })
    .Wait();
    
    // Librame.Extensions.Network.DotNetty.WebSocket.Client
    var client = serviceProvider.GetRequiredService<IWebSocketClient>();
    client.StartAsync(async channel =>
    {
        while (true)
        {
            string msg = Console.ReadLine();
            if (msg.IsNull())
            {
                break;
            }
            else if ("bye".Equals(msg, StringComparison.OrdinalIgnoreCase))
            {
                await channel.WriteAndFlushAsync(new CloseWebSocketFrame());
                break;
            }
            else if ("ping".Equals(msg, StringComparison.OrdinalIgnoreCase))
            {
                var frame = new PingWebSocketFrame(Unpooled.WrappedBuffer(new byte[] { 8, 1, 8, 1 }));
                await channel.WriteAndFlushAsync(frame);
            }
            else
            {
                var frame = new TextWebSocketFrame(msg);
                await channel.WriteAndFlushAsync(frame);
            }
        }

        await channel.CloseAsync();
    })
    .Wait();

## Librame.Extensions.Storage

    // PM> Install-Package: [Librame.Extensions.Storage](https://www.nuget.org/packages?q=Librame.Extensions.Storage)
    services.AddLibrame()
        .AddStorage(options =>
        {
            options.FileProviders.Add(new PhysicalStorageFileProvider("Root"));
        });

## Test Extension

    // Librame.Extensions.Storage.Tests.FileServiceTests
    var service = serviceProvider.GetRequiredService<IFileService>();
    var fileInfo = await service.GetFileInfoAsync("Subpath");
    
    // fileInfo save as WriteFile
    using (var writeStream = new FileStream("WriteFile", FileMode.Create))
    {
        await service.ReadAsync(fileInfo, writeStream);
    }
    Assert.Equal(File.ReadAllText(fileInfo.PhysicalPath), File.ReadAllText("WriteFile"));
    
    using (var readStream = new FileStream("ReadFile", FileMode.Open))
    {
        await service.WriteAsync(fileInfo, readStream);
    }
    Assert.Equal(File.ReadAllText("ReadFile"), File.ReadAllText(fileInfo.PhysicalPath));
    
    // Librame.Extensions.Storage.Tests.FileTransferServiceTests
    var url = "https://www.domain.com/test.txt";
    var filePath = @"d:\test.txt";
    var fileTransfer = serviceProvider.GetRequiredService<IFileTransferService>();
    
    var combiner = await fileTransfer.DownloadFileAsync(url, filePath);
    Assert.True(combiner.Exists());
    
    combiner = fileTransfer.UploadFileAsync(url, filePath);
    
    // Librame.Extensions.Storage.Tests.FilePermissionServiceTests
    var permission = TestServiceProvider.Current.GetRequiredService<IFilePermissionService>();

    var accessToken = await service.GeAccessTokenAsync();
    Assert.NotEmpty(accessToken);

    var authorizationCode = await service.GetAuthorizationCodeAsync();
    Assert.NotEmpty(authorizationCode);

    var cookieValue = await service.GetCookieValueAsync();
    Assert.NotEmpty(cookieValue);
