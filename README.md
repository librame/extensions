## Librame Project

[![License](https://img.shields.io/badge/License-Apache%202.0-blue.svg)](https://github.com/librame/Librame/blob/master/LICENSE)
[![Available on NuGet https://www.nuget.org/packages?q=Librame.Extensions.Core](https://img.shields.io/nuget/v/Librame.Extensions.Core.svg?style=flat-square)](https://www.nuget.org/packages?q=Librame.Extensions.Core)

## Use

* Official releases are on [NuGet](https://www.nuget.org/packages?q=Librame).

### Librame.Extensions.Core

## Install Extension

    PM> Install-Package Librame.Extensions.Core

## Register Extension

    // Use DependencyInjection
    var services = new ServiceCollection();
    
    // Register Librame
    services.AddLibrame();
    
    // Build ServiceProvider
    var serviceProvider = services.BuildServiceProvider();
    ......

### Librame.Extensions.Data.EntityFrameworkCore

## Install Extension

    PM> Install-Package Librame.Extensions.Data.EntityFrameworkCore

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
        .AddAccessor<TestDbContextAccessor>((options, optionsBuilder) =>
        {
            optionsBuilder.UseSqlServer(options.LocalTenant.DefaultConnectionString,
                sql => sql.MigrationsAssembly("AssemblyName"));
        });
    
    // Use Store
    services.AddTransient<ITestStore, TestStore>();
    
    // Build ServiceProvider
    var serviceProvider = services.BuildServiceProvider();

## Test Extension

    // Accessor
    public class TestDbContextAccessor : DbContextAccessor, IAccessor
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

            modelBuilder.Entity<Category>(category =>
            {
                category.ToTable(new TableSchema<Category>());

                category.HasKey(x => x.Id);

                category.Property(x => x.Id).ValueGeneratedOnAdd();
                category.Property(x => x.Name).HasMaxLength(100).IsRequired();
                category.Property(x => x.Rank);
                category.Property(x => x.Status);

                category.HasMany(x => x.Articles).WithOne(x => x.Category).IsRequired().OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Article>(article =>
            {
                article.ToTable(TableSchema<Article>.BuildEveryYear(DateTime.Now));

                article.HasKey(x => x.Id);

                article.Property(x => x.Id).ValueGeneratedNever();
                article.Property(x => x.Title).HasMaxLength(200).IsRequired();
                article.Property(x => x.Descr).HasMaxLength(1000).IsRequired();
                article.Property(x => x.Rank);
                article.Property(x => x.Status);
            });
        }
    }
    
    // Store
    public interface ITestStore : IBaseStore
    {
        IList<Category> GetCategories();

        IPagingList<Article> GetArticles();


        ITestStore UseDefaultStore();

        ITestStore UseWriteStore();
    }
    public class TestStore : AbstractBaseStore, ITestStore
    {
        public TestStore(IAccessor accessor)
            : base(accessor)
        {
        }
        
        public IList<Category> GetCategories()
        {
            if (Accessor is TestDbContextAccessor dbContextAccessor)
                return dbContextAccessor.Categories.ToList();

            return null;
        }

        public IPagingList<Article> GetArticles()
        {
            if (Accessor is TestDbContextAccessor dbContextAccessor)
                return dbContextAccessor.Articles.AsPagingListByIndex(ordered => ordered.OrderBy(a => a.Id), 1, 10);

            return null;
        }


        public ITestStore UseDefaultStore()
        {
            Accessor.ChangeDbConnection(t => t.WriteConnectionString);
            return this;
        }

        public ITestStore UseWriteStore()
        {
            Accessor.ChangeDbConnection(t => t.DefaultConnectionString);
            return this;
        }
    }
    
    // Test
    public class DbContextAccessorTests
    {
        [Fact]
        public void AllTest()
        {
            var store = TestServiceProvider.Current.GetRequiredService<ITestStore>();

            var categories = store.GetCategories();
            Assert.Empty(categories);

            categories = store.UseWriteDbConnection().GetCategories();
            Assert.NotEmpty(categories);

            var articles = store.UseDefaultDbConnection().GetArticles();
            Assert.Empty(articles);

            articles = store.UseWriteDbConnection().GetArticles();
            Assert.NotEmpty(articles);
        }
    }

### Librame.Extensions.Drawing.SkiaSharp

## Install Extension

    PM> Install-Package Librame.Extensions.Drawing.SkiaSharp

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
            var imageFile = "test.jpg".AsDefaultFileLocator(TestServiceProvider.ResourcesPath);
            
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
            var imageFile = "test.jpg".AsDefaultFileLocator(TestServiceProvider.ResourcesPath);
            var saveFile = imageFile.NewFileName("test-watermark.png");
            
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
    public class InternalEmailServiceTests
    {
        private IEmailService _service;

        public InternalEmailServiceTests()
        {
            _service = TestServiceProvider.Current.GetRequiredService<IEmailService>();
        }
        
        [Fact]
        public async void SendAsyncTest()
        {
            async _service.SendAsync("receiver@domain.com",
                "Email Subject",
                "Email Body");
        }
    }
    
    // Message
    public class InternalSmsServiceTests
    {
        private ISmsService _service;

        public InternalSmsServiceTests()
        {
            _service = TestServiceProvider.Current.GetRequiredService<ISmsService>();
        }

        [Fact]
        public async void SendAsyncTest()
        {
            var result = async _service.SendAsync("TestData: 123456");
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
    services.AddLibrame(configureLogging: loggingBuilder =>
    {
        loggingBuilder.ClearProviders();
        loggingBuilder.SetMinimumLevel(LogLevel.Trace);

        loggingBuilder.AddConsole(options => options.IncludeScopes = false);
        loggingBuilder.AddFilter((str, level) => true);
    })
        .AddEncryption().AddGlobalSigningCredentials(new X509Certificate2(locator.ToString(), "password"))
        .AddNetwork().AddDotNetty();
    
    // Use DotNetty LoggerFactory
    services.TryReplace(InternalLoggerFactory.DefaultFactory);
    
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
    ......

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

    public class InternalPhysicalFileServiceTests
    {
        private IFileService _file;

        public InternalPhysicalFileServiceTests()
        {
            _file = TestServiceProvider.Current.GetRequiredService<IFileService>();
        }

        [Fact]
        public void GetProviderAsyncTest()
        {
            var provider = _file.GetProviderAsync(“root”).Result;
            var files = provider.GetDirectoryContents("subpath");
            Assert.NotEmpty(files);
        }
    }
    
    public class InternalFileUploadServiceTests
    {
        private IFileUploadService _fileUpload;

        public InternalFileUploadServiceTests()
        {
            _fileUpload = TestServiceProvider.Current.GetRequiredService<IFileUploadService>();
        }

        [Fact]
        public void UploadFileAsync()
        {
            var uploadApi = "https://domain.com/api/upload";
            var uploadFile = @"c:\temp.txt";

            var result = _fileUpload.UploadFileAsync(uploadApi, uploadFile).Result;
            Assert.NotEmpty(result);
        }
    }
