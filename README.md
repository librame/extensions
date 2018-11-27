## Librame Project

[![License](https://img.shields.io/badge/License-Apache%202.0-blue.svg)](https://github.com/librame/Librame/blob/master/LICENSE)
[![Available on NuGet https://www.nuget.org/packages?q=Librame](https://img.shields.io/nuget/v/Librame.svg?style=flat-square)](https://www.nuget.org/packages?q=Librame)

## Use

* Official releases are on [NuGet](https://www.nuget.org/packages?q=Librame).

### Librame

## Install Extension

    PM> Install-Package Librame

## Register Extension

    // Use DependencyInjection
    var services = new ServiceCollection();
    
    // Register Librame
    services.AddLibrame();
    
    // Build ServiceProvider
    var serviceProvider = services.BuildServiceProvider();
    ......

### Librame.Extensions.Data (based EntityFrameworkCore)

## Install Extension

    PM> Install-Package Librame.Extensions.Data

## Register Extension

    // Use DependencyInjection
    var services = new ServiceCollection();
    
    // Register Librame
    services.AddLibrame()
        .AddData(options =>
        {
            options.LocalTenant.DefaultConnectionString = "default database connection string";
            options.LocalTenant.WriteConnectionString = "write database connection string";
            options.LocalTenant.WriteConnectionSeparation = true;
        })
        .AddDbContext<ITestDbContext, TestDbContext>((options, optionsBuilder) =>
        {
            optionsBuilder.UseSqlServer(options.LocalTenant.DefaultConnectionString,
                sql => sql.MigrationsAssembly("AssemblyName"));
        });
    
    // Use Store
    services.AddTransient<ITestStore, TestStore>();
    
    // Build ServiceProvider
    var serviceProvider = services.BuildServiceProvider();

## Test Extension

    // DbContext
    public interface ITestDbContext : IDbContext
    {
        DbSet<Category> Categories { get; set; }

        DbSet<Article> Articles { get; set; }
    }
    public class TestDbContext : AbstractDbContext<TestDbContext>, ITestDbContext
    {
        public DbSet<Category> Categories { get; set; }

        public DbSet<Article> Articles { get; set; }
    }
    
    // Store
    public interface ITestStore : IStore
    {
        IList<Category> GetCategories();

        IPagingList<Article> GetArticles();


        ITestStore UseDefaultStore();

        ITestStore UseWriteStore();
    }
    public class TestStore : AbstractStore<ITestDbContext>, ITestStore
    {
        public TestStore(ITestDbContext dbContext)
            : base(dbContext)
        {
        }
        
        public IList<Category> GetCategories()
        {
            return DbContext.Categories.ToList();
        }

        public IPagingList<Article> GetArticles()
        {
            return DbContext.Articles.AsPagingByIndex(order => order.OrderBy(a => a.Id), 1, 10);
        }


        public ITestStore UseDefaultStore()
        {
            DbContext.TrySwitchConnection(options => options.DefaultString);
            return this;
        }

        public ITestStore UseWriteStore()
        {
            DbContext.TrySwitchConnection(options => options.WriteString);
            return this;
        }
    }
    
    // Test
    public class UnifiedTests
    {
        [Fact]
        public void UnifiedTest()
        {
            var store = TestServiceProvider.Current.GetRequiredService<ITestStore>();

            var categories = store.GetCategories();
            Assert.Empty(categories);

            // Use Write Database
            categories = store.UseWriteStore().GetCategories();
            Assert.NotEmpty(categories);

            // Restore
            store.UseDefaultStore();

            var articles = store.GetArticles();
            Assert.Empty(articles);

            // Use Write Database
            articles = store.UseWriteStore().GetArticles();
            Assert.NotEmpty(articles);

            // Restore
            store.UseDefaultStore();
        }
    }

### Librame.Extensions.Drawing (based SkiaSharp)

## Install Extension

    PM> Install-Package Librame.Extensions.Drawing

## Register Extension

    // Use DependencyInjection
    var services = new ServiceCollection();
    
    // Register Librame
    services.AddLibrame()
        .AddDrawing();
    
    // Build ServiceProvider
    var serviceProvider = services.BuildServiceProvider();

## Test Extension

    // Captcha
    public class InternalCaptchaServiceTests
    {
        private ICaptchaService _drawing = null;

        public InternalCaptchaServiceTests()
        {
            _drawing = TestServiceProvider.Current.GetRequiredService<ICaptchaService>();
        }

        [Fact]
        public async void DrawCaptchaBytesTest()
        {
            var buffer = await _drawing.DrawBytes("1234");
            Assert.NotNull(buffer);
        }

        [Fact]
        public async void DrawCaptchaFileTest()
        {
            var saveFile = "captcha.png".AsDefaultFileLocator(TestServiceProvider.ResourcesPath);

            var succeed = await _drawing.DrawFile("1234", saveFile.ToString());
            Assert.True(succeed);
        }
    }
    
    // Scale
    public class InternalScaleServiceTests
    {
        private IScaleService _drawing = null;

        public InternalScaleServiceTests()
        {
            _drawing = TestServiceProvider.Current.GetRequiredService<IScaleService>();
        }

        [Fact]
        public async void DrawScaleTest()
        {
            // 5K 2.21MB
            var imageFile = "eso1004a.jpg".AsDefaultFileLocator(TestServiceProvider.ResourcesPath);
            
            var succeed = await _drawing.DrawFile(imageFile.ToString());
            Assert.True(succeed);
        }

        [Fact]
        public async void DrawScalesByDirectoryTest()
        {
            // 5K 2.21MB
            var directory = TestServiceProvider.ResourcesPath.CombinePath(@"pictures");
            
            // Clear
            var count = _drawing.DeleteScalesByDirectory(directory);

            count = await _drawing.DrawFilesByDirectory(directory);
            Assert.True(count > 0);
        }
    }

    // Watermark
    public class InternalWatermarkServiceTests
    {
        private IWatermarkService _drawing = null;

        public InternalWatermarkServiceTests()
        {
            _drawing = TestServiceProvider.Current.GetRequiredService<IWatermarkService>();
        }

        [Fact]
        public async void DrawWatermarkTest()
        {
            // 5K 2.21MB
            var imageFile = "eso1004a.jpg".AsDefaultFileLocator(TestServiceProvider.ResourcesPath);
            var saveFile = imageFile.NewFileName("eso1004a-watermark.png");
            
            var succeed = await _drawing.DrawFile(imageFile.ToString(), saveFile.ToString());
            Assert.True(succeed);
        }
    }

### Librame.Extensions.Encryption

## Install Extension

    PM> Install-Package Librame.Extensions.Encryption

## Register Extension

    // Use DependencyInjection
    var services = new ServiceCollection();
    
    // Register Librame
    services.AddLibrame()
        .AddEncryption()
        .AddDeveloperGlobalSigningCredentials(); //.AddSigningCredentials();
    
    // Build ServiceProvider
    var serviceProvider = services.BuildServiceProvider();

## Test Extension

    public class EncryptionBuilderExtensionsTests
    {
        [Fact]
        public void JointTest()
        {
            var str = nameof(EncryptionBuilderExtensionsTests);
            var plaintextBuffer = str.AsPlaintextBuffer(TestServiceProvider.Current);
            
            var hashString = plaintextBuffer.ApplyServiceProvider(TestServiceProvider.Current)
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

            var ciphertextBuffer = ciphertextString.AsCiphertextBuffer(TestServiceProvider.Current)
                .FromRsa()
                .FromAes()
                .FromTripleDes()
                .FromDes();
            Assert.Equal(hashString, ciphertextBuffer.AsCiphertextString());
        }
    }

### Librame.Extensions.Network

## Install Extension

    PM> Install-Package Librame.Extensions.Network

## Register Extension

    // Use DependencyInjection
    var services = new ServiceCollection();
    
    // Register Librame
    services.AddLibrame()
        .AddEncryption().AddDeveloperGlobalSigningCredentials()
        .AddNetwork();
    
    // Build ServiceProvider
    var serviceProvider = services.BuildServiceProvider();

## Test Extension

    // Crawler
    public class InternalCrawlerServiceTests
    {
        private ICrawlerService _crawler;

        public InternalCrawlerServiceTests()
        {
            _crawler = TestServiceProvider.Current.GetRequiredService<ICrawlerService>();
        }

        [Fact]
        public async void GetStringTest()
        {
            var result = await _crawler.GetString("https://www.cnblogs.com");
            Assert.NotEmpty(result);
        }

        [Fact]
        public async void GetHyperLinksTest()
        {
            var result = await _crawler.GetHyperLinks("https://www.baidu.com");
            Assert.True(result.Count > 0);
        }

        [Fact]
        public async void GetImageHyperLinksTest()
        {
            var result = await _crawler.GetImageHyperLinks("https://www.baidu.com");
            Assert.True(result.Count > 0);
        }
    }

    // Email
    public class InternalEmailSenderTests
    {
        private IEmailSender _sender;

        public InternalEmailSenderTests()
        {
            _sender = TestServiceProvider.Current.GetRequiredService<IEmailSender>();
        }
        
        [Fact]
        public async void SendAsyncTest()
        {
            async _sender.SendAsync("receiver@domain.com",
                "Email Subject",
                "Email Body");
        }
    }
    
    // Message
    public class InternalSmsSenderTests
    {
        private ISmsSender _sender;

        public InternalSmsSenderTests()
        {
            _sender = TestServiceProvider.Current.GetRequiredService<ISmsSender>();
        }

        [Fact]
        public async void SendAsyncTest()
        {
            var result = async _sender.SendAsync("TestData: 123456");
            Assert.Empty(result);
        }
    }

### Librame.Extensions.Network.DotNetty

## Install Extension

    PM> Install-Package Librame.Extensions.Network.DotNetty

## Register Extension

    // Use DependencyInjection
    var services = new ServiceCollection();
    
    var locator = "dotnetty.com.pfx".AsDefaultFileLocator("BasePath");
    
    // Register Librame
    services.AddLibrame()
        .AddEncryption().AddGlobalSigningCredentials(new X509Certificate2(locator.ToString(), "password"))
        .AddNetwork().AddDotNetty();
    
    // Build ServiceProvider
    var serviceProvider = services.BuildServiceProvider();

## Test Extension

    // WebSocketServer Console
    var server = serviceProvider.GetRequiredService<IWebSocketServer>();
    server.StartAsync(channel =>
    {
        Console.ReadLine();
    })
    .Wait();
    
    // WebSocketClient Console
    var client = serviceProvider.GetRequiredService<IWebSocketClient>();
    client.StartAsync(async channel =>
    {
        while (true)
        {
            string msg = Console.ReadLine();
            if (msg == null)
            {
                break;
            }
            else if ("bye".Equals(msg.ToLower()))
            {
                await channel.WriteAndFlushAsync(new CloseWebSocketFrame());
                break;
            }
            else if ("ping".Equals(msg.ToLower()))
            {
                var frame = new PingWebSocketFrame(Unpooled.WrappedBuffer(new byte[] { 8, 1, 8, 1 }));
                await channel.WriteAndFlushAsync(frame);
            }
            else
            {
                WebSocketFrame frame = new TextWebSocketFrame(msg);
                await channel.WriteAndFlushAsync(frame);
            }
        }
    })
    .Wait();

### Librame.Extensions.Storage

## Install Extension

    PM> Install-Package Librame.Extensions.Storage

## Register Extension

    // Use DependencyInjection
    var services = new ServiceCollection();
    
    // Register Librame
    services.AddLibrame()
        .AddStorage();
    
    // Build ServiceProvider
    var serviceProvider = services.BuildServiceProvider();

## Test Extension

    // FileSystem
    public class InternalFileSystemServiceTests
    {
        private IFileSystemService _fileSystem;

        public InternalFileSystemServiceTests()
        {
            _fileSystem = TestServiceProvider.Current.GetRequiredService<IFileSystemService>();
        }

        [Fact]
        public void LoadDriversTest()
        {
            var drivers = _fileSystem.LoadDrivers();
            Assert.NotEmpty(drivers);
        }

        [Fact]
        public void LoadDirectoriesTest()
        {
            var dirs = _fileSystem.LoadDirectories("*.*");
            Assert.NotEmpty(dirs);
        }
    }
