using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Linq;
using Xunit;

namespace Librame.Extensions.Core.Tests
{
    public class AbstractionOptionsServiceCollectionExtensionsTests
    {
        class TestOptions
        {
            public string Name { get; set; }
        }

        class TestConfigureOptions : IConfigureOptions<TestOptions>
        {
            public void Configure(TestOptions options)
            {
                options.Name = nameof(TestConfigureOptions);
            }
        }

        class TestReplaceConfigureOptions : IConfigureOptions<TestOptions>
        {
            public void Configure(TestOptions options)
            {
                options.Name = nameof(TestReplaceConfigureOptions);
            }
        }


        [Fact]
        public void AllTest()
        {
            var services = new ServiceCollection();

            services.ConfigureOptions<TestConfigureOptions>();

            var optionsType = typeof(TestConfigureOptions);
            var descriptors = services.TryGetConfigureOptions<TestConfigureOptions>();
            Assert.NotEmpty(descriptors);
            Assert.True(descriptors.All(d => d.ImplementationType == optionsType));

            services.TryReplaceConfigureOptions<TestReplaceConfigureOptions>();

            optionsType = typeof(TestReplaceConfigureOptions);
            descriptors = services.TryGetConfigureOptions<TestReplaceConfigureOptions>();
            Assert.NotEmpty(descriptors);
            Assert.True(descriptors.All(d => d.ImplementationType == optionsType));
        }
    }
}
