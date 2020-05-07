using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using Xunit;

namespace Librame.Extensions.Core.Tests
{
    using Builders;
    using Dependencies;

    public class CoreBuilderDependencyTests
    {
        [Fact]
        public void AllTest()
        {
            var message = $"Test value from {nameof(CoreBuilderDependencyTests)}";

            var root = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    { "CoreBuilderDependency:Test:Options:Message", message }
                })
                .Build();

            var services = new ServiceCollection();
            services.AddLibrame<TestCoreBuilderDependency>(dependency =>
            {
                dependency.ConfigurationRoot = root;
                //dependency.Configuration = root.GetSection("CoreBuilderDependency");
                //dependency.Test.Options.Message = message;
            });

            var provider = services.BuildServiceProvider();

            var optionsDependency = provider.GetRequiredService<TestCoreBuilderDependency>();
            var options = provider.GetRequiredService<IOptions<TestOptions>>().Value;

            Assert.Equal(message, options.Message);
        }


        public class TestCoreBuilderDependency : CoreBuilderDependency
        {
            public OptionsDependency<TestOptions> Test { get; }
                = new OptionsDependency<TestOptions>();
        }


        public class TestOptions
        {
            public string Message { get; set; }
        }
    }
}
