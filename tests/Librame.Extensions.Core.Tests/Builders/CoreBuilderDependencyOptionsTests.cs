using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using Xunit;

namespace Librame.Extensions.Core.Tests
{
    public class CoreBuilderDependencyOptionsTests
    {
        [Fact]
        public void AllTest()
        {
            var message = "Test value from in memory collection";

            var root = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    { "CoreBuilderDependency:Test:Message", message }
                })
                .Build();

            var services = new ServiceCollection();
            services.AddLibrame<TestCoreBuilderDependencyOptions>(dependency =>
            {
                dependency.Configuration = root.GetSection(dependency.Name);
            });

            var provider = services.BuildServiceProvider();
            var options = provider.GetRequiredService<IOptions<TestOptions>>().Value;

            Assert.Equal(message, options.Message);
        }


        /// <example>
        /// ex. "appsettings.json"
        /// {
        ///     "CoreBuilderDependency": // TestCoreBuilderDependencyOptions
        ///     {
        ///         "CoreBuilder": // CoreBuilderDependencyOptions.Builder [OptionsConfigurator{TBuilderOptions}]
        ///         {
        ///             "IsUtcClock": false
        ///         },
        ///         "Localization": // CoreBuilderDependencyOptions.Localization [OptionsConfigurator{LocalizationOptions}]
        ///         {
        ///             "ResourcesPath": "Resources"
        ///         },
        ///         "Test": // TestCoreBuilderDependencyOptions.Test [OptionsConfigurator{TestOptions}]
        ///         {
        ///             "Message": "Test message"
        ///         },...
        ///     }
        /// }
        /// </example>
        public class TestCoreBuilderDependencyOptions : CoreBuilderDependencyOptions
        {
            //public TestCoreBuilderDependencyOptions()
            //    : base(GetOptionsName<TestCoreBuilderDependencyOptions>())
            //{
            //}


            public OptionsActionConfigurator<TestOptions> Test { get; }
                = new OptionsActionConfigurator<TestOptions>();
        }


        public class TestOptions
        {
            public string Message { get; set; }
        }
    }
}
