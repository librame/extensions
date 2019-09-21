## Librame Project

[![License](https://img.shields.io/badge/License-Apache%202.0-blue.svg)](https://github.com/librame/Librame/blob/master/LICENSE)
[![Available on NuGet https://www.nuget.org/packages?q=Librame.Extensions.Core](https://img.shields.io/nuget/v/Librame.Extensions.Core.svg?style=flat-square)](https://www.nuget.org/packages?q=Librame.Extensions.Core)

## Open Source

* Official releases are on [NuGet](https://www.nuget.org/packages?q=Librame).

## Use

    // Install-Package Microsoft.Extensions.DependencyInjection
    var services = new ServiceCollection();
    
    // Install-Package Librame.Extensions.Core
    // Register Librame Entry
    services.AddLibrame();
    
    // Build ServiceProvider
    var serviceProvider = services.BuildServiceProvider();
    ......

### Librame.Extensions.Core

## Install Extension

    PM> Install-Package Librame.Extensions.Core

## Register Extension

    services.AddLibrame();

## Test Extension

    // MediatR
    var mediator = serviceProvider.GetRequiredService<IMediator>();
    var response = await mediator.Send(new Ping { Message = "Ping" });
    Assert.Contains("Ping Pong Ping", response.Message);
    // Librame.Extensions.Core.Tests.RequestPostProcessorBehaviorTests
    
    // Localizers
    var localizer = serviceProvider.GetRequiredService<IExpressionStringLocalizer<TestResource>>();
    Assert.True(localizer[r => r.Name].ResourceNotFound);
    // Librame.Extensions.Core.Tests.ExpressionStringLocalizerTests
    
    // Combiners
    var urlString = "https://1.2.3.4.developer.microsoft.com/en-us/fabric#/get-started";
    var uriCombiner = uriString.AsUriCombiner();
    Assert.Equal("https", uriCombiner.Scheme);
    Assert.Equal("developer.microsoft.com", uriCombiner.Host);
    Assert.Equal("/en-us/fabric", uriCombiner.Path);
    ......
    // Librame.Extensions.Core.Tests.UriCombinerTests
    
    var domainCombiner = uriString.GetHost().AsDomainNameCombiner(); // uriCombiner.Host
    Assert.Equal("com", domainCombiner.Root);
    Assert.Equal("microsoft.com", domainCombiner.TopLevel);
    Assert.Equal("developer.microsoft.com", domainCombiner.SecondLevel);
    ......
    // Librame.Extensions.Core.Tests.DomainNameCombinerTests
    
    var path = @"c:\test\file.ext";
    var filePathCombiner = path.AsFilePathCombiner();
    Assert.Equal(@"c:\test", filePathCombiner.BasePath);
    Assert.Equal("file.ext", filePathCombiner.FileNameString);
    ......
    // Librame.Extensions.Core.Tests.FilePathCombinerTests
    
    var fileNameCombiner = path.AsFileNameCombiner();
    Assert.Equal("file", fileNameCombiner.BaseName);
    Assert.Equal(".ext", fileNameCombiner.Extension);
    ......
    // Librame.Extensions.Core.Tests.FileNameCombinerTests
    
    // ServicesManager
    // public interface ILetterService : IService {}
    // services.AddSingleton<ILetterService, AService>();
    // services.AddSingleton<ILetterService, BService>();
    // services.AddSingleton<ILetterService, CService>();
    
    var letters = serviceProvider.GetRequiredService<IServiceManager<ILetterService>>();
    Assert.NotEmpty(letters.Services) // IEnumerable<ILetterService>
    Assert.NotNull(letters.Default) // default letters.Services.First()
    
    letters = serviceProvider.GetRequiredService<IServiceManager<ILetterService, CService>>();
    Assert.NotEmpty(letters.Services) // IEnumerable<ILetterService>
    Assert.NotNull(letters.Default) // default CService

### Librame.Extensions.Data.EntityFrameworkCore

## Install Extension

    PM> Install-Package Librame.Extensions.Data.EntityFrameworkCore

## Register Extension

    services.AddLibrame()
        .AddData(options =>
        {
            // Multi-Tenancy, Reading and writing separation
            options.DefaultTenant.DefaultConnectionString = "default database connection string";
            options.DefaultTenant.WritingConnectionString = "write database connection string";
            options.DefaultTenant.WritingSeparation = true;
        })
        .AddAccessor<TestDbContextAccessor>((options, optionsBuilder) =>
        {
            // Install-Package Microsoft.EntityFrameworkCore.SqlServer
            optionsBuilder.UseSqlServer(options.DefaultTenant.DefaultConnectionString,
                sql => sql.MigrationsAssembly("AssemblyName"));
        })
        .AddStoreHubWithAccessor<TestStoreHub>()
        .AddInitializerWithAccessor<TestStoreInitializer>()
        .AddIdentifier<TestStoreIdentifier>();

## Test Extension

    // StoreHub
    public class TestStoreHub : StoreHubBase<TestDbContextAccessor>
    {
        public TestStore(IAccessor accessor, IStoreInitializer<TestDbContextAccessor> initializer)
            : base(accessor, initializer)
        {
        }
        
        
        public IList<Category> GetCategories()
        {
            return Accessor.Categories.ToList();
        }

        public IPageable<Article> GetArticles()
        {
            return Accessor.Articles.AsDescendingPagingByIndex(1, 10);
        }


        public TestStoreHub UseWriteDbConnection()
        {
            Accessor.TryChangeDbConnection(t => t.WritingConnectionString);
            return this;
        }

        public TestStoreHub UseDefaultDbConnection()
        {
            Accessor.TryChangeDbConnection(t => t.DefaultConnectionString);
            return this;
        }
    }
    
    // DbContextAccessor
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

            ......
        }
    }
    
    // Test
    using (var stores = serviceProvider.GetRequiredService<TestStoreHub>())
    {
        var categories = stores.GetCategories();
        Assert.Empty(categories);

        categories = stores.UseWriteDbConnection().GetCategories();
        Assert.NotEmpty(categories);

        var articles = stores.UseDefaultDbConnection().GetArticles();
        Assert.Empty(articles);

        articles = stores.UseWriteDbConnection().GetArticles();
        Assert.NotEmpty(articles);
    }
    ......
    // Librame.Extensions.Data.Tests.DbContextAccessorTests

### Librame.Extensions.Drawing.SkiaSharp

## Install Extension

    PM> Install-Package Librame.Extensions.Drawing.SkiaSharp

## Register Extension

    services.AddLibrame()
        .AddDrawing(options =>
        {
            options.Captcha.Font.FilePath.ChangeBasePath("FontFilePath");
            options.Watermark.Font.FilePath.ChangeBasePath("FontFilePath");
            options.Watermark.ImagePath.ChangeBasePath("WatermarkFilePath");
        });

## Test Extension

    // Captcha
    var captchaService = serviceProvider.GetRequiredService<ICaptchaService>();
    var buffer = await captchaService.DrawBytesAsync("1234");
    Assert.NotNull(buffer);
    
    var saveFile = "captcha.png".AsFilePathCombiner("SavePath");
    var result = await captchaService.DrawFileAsync("1234", saveFile);
    Assert.True(result);
    // Librame.Extensions.Drawing.Tests.CaptchaServiceTests
    
    // Scale
    var scaleService = serviceProvider.GetRequiredService<IScaleService>();
    var imageFile = "test.jpg".AsFilePathCombiner("ImagePath");
    var result = scaleService.DrawFile(imageFile);
    Assert.True(result);
    
    var count = await scaleService.DrawFilesByDirectoryAsync("FolderPath");
    Assert.True(count > 0);
    // Librame.Extensions.Drawing.Tests.ScaleServiceTests
    
    // Watermark
    var watermarkService = serviceProvider.GetRequiredService<IWatermarkService>();
    var imageFile = "test.jpg".AsFilePathCombiner("ImagePath");
    var saveFile = imageFile.NewFileName("test-watermark.png");
    var result = await watermarkService.DrawFileAsync(imageFile, saveFile);
    Assert.True(result);
    // Librame.Extensions.Drawing.Tests.WatermarkServiceTests

### Librame.Extensions.Encryption

## Install Extension

    PM> Install-Package Librame.Extensions.Encryption

## Register Extension

    services.AddLibrame()
        .AddEncryption()
        .AddDeveloperGlobalSigningCredentials(); // AddSigningCredentials();

## Test Extension

    var str = "TestString";
    var plaintextBuffer = str.AsPlaintextBuffer(serviceProvider);
    
    var hashString = plaintextBuffer
        .Md5()
        .Sha1()
        .Sha256()
        .Sha384()
        .Sha512()
        .HmacMd5()
        .HmacSha1()
        .HmacSha256()
        .HmacSha384()
        .HmacSha512()
        .AsCiphertextString();

    var plaintextBufferCopy = plaintextBuffer.Copy();
    var ciphertextString = plaintextBufferCopy
        .AsDes()
        .AsTripleDes()
        .AsAes()
        .AsRsa()
        .AsCiphertextString();
    Assert.NotEmpty(ciphertextString);

    var ciphertextBuffer = ciphertextString.AsCiphertextBuffer(serviceProvider)
        .FromRsa()
        .FromAes()
        .FromTripleDes()
        .FromDes();
    
    Assert.True(plaintextBuffer.Equals(ciphertextBuffer));
    Assert.Equal(hashString, ciphertextBuffer.AsCiphertextString());
    // Librame.Extensions.Encryption.Tests.EncryptionBufferExtensionsTests

### Librame.Extensions.Network

## Install Extension

    PM> Install-Package Librame.Extensions.Network

## Register Extension

    services.AddLibrame()
        .AddNetwork();

## Test Extension

    // Crawler
    var crawler = serviceProvider.GetRequiredService<ICrawlerService>();
    var content = await crawler.GetContentAsync("https://www.cnblogs.com");
    Assert.NotEmpty(content);
    
    var links = await crawler.GetHyperLinksAsync("https://www.baidu.com");
    Assert.NotEmpty(links);
    
    var images = await crawler.GetImageHyperLinks("https://www.baidu.com");
    Assert.NotEmpty(images);
    // Librame.Extensions.Network.Tests.CrawlerServiceTests

    // Email
    var email = serviceProvider.GetRequiredService<IEmailService>();
    async email.SendAsync("receiver@domain.com",
        "Email Subject",
        "Email Body");
    // Librame.Extensions.Network.Tests.EmailServiceTests
    
    // Message
    var sms = serviceProvider.GetRequiredService<ISmsService>();
    var result = async sms.SendAsync("13012345678", "TestData: 123456");
    Assert.Empty(result);

### Librame.Extensions.Network.DotNetty

## Install Extension

    PM> Install-Package Librame.Extensions.Network.DotNetty

## Register Extension

    var combiner = "dotnetty.com.pfx".AsFilePathCombiner("BasePath");
    services.AddLibrame(logging =>
    {
        logging.ClearProviders();
        logging.SetMinimumLevel(LogLevel.Trace);

        logging.AddConsole(logger => logger.IncludeScopes = false);
        logging.AddFilter((str, level) => true);
    })
    .AddEncryption().AddGlobalSigningCredentials(new X509Certificate2(combiner.ToString(), "password"))
    .AddNetwork().AddDotNetty();
    
    // Use DotNetty LoggerFactory
    InternalLoggerFactory.DefaultFactory.AddProvider(new ConsoleLoggerProvider((s, level) => true, false));
    //services.TryReplace(InternalLoggerFactory.DefaultFactory);

## Test Extension

    // WebSocketServer Console
    var server = serviceProvider.GetRequiredService<IWebSocketServer>();
    server.StartAsync(async channel =>
    {
        Console.ReadLine();
        await channel.CloseAsync();
    })
    .Wait();
    // Librame.Extensions.Network.DotNetty.WebSocket.Server
    
    // WebSocketClient Console
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
    // Librame.Extensions.Network.DotNetty.WebSocket.Client

### Librame.Extensions.Storage

## Install Extension

    PM> Install-Package Librame.Extensions.Storage

## Register Extension

    services.AddLibrame()
        .AddStorage(options =>
        {
            options.FileProviders.Add(new PhysicalStorageFileProvider("Root"));
        });

## Test Extension

    // FileService
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
    // Librame.Extensions.Storage.Tests.FileServiceTests
    
    // FileTransferService
    var url = "https://www.domain.com/test.txt";
    var filePath = @"d:\test.txt";
    var fileTransfer = serviceProvider.GetRequiredService<IFileTransferService>();
    
    var combiner = await fileTransfer.DownloadFileAsync(url, filePath);
    Assert.True(combiner.Exists());
    
    combiner = fileTransfer.UploadFileAsync(url, filePath);
    // Librame.Extensions.Storage.Tests.FileTransferServiceTests
    
    // FilePermissionService
    var permission = TestServiceProvider.Current.GetRequiredService<IFilePermissionService>();

    var accessToken = await service.GeAccessTokenAsync();
    Assert.NotEmpty(accessToken);

    var authorizationCode = await service.GetAuthorizationCodeAsync();
    Assert.NotEmpty(authorizationCode);

    var cookieValue = await service.GetCookieValueAsync();
    Assert.NotEmpty(cookieValue);
    // Librame.Extensions.Storage.Tests.FilePermissionServiceTests
