using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Xunit;

namespace Librame.Extensions.Core.Tests
{
    public class AbstractionOptionsServiceCollectionExtensionsTests
    {
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
