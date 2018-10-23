using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Librame.Builders.Tests
{
    public class BuilderExtensionsTests
    {

        [Fact]
        public void BuilderTest()
        {
            var services = new ServiceCollection();

            services.AsDefaultBuilder();

            var serviceProvider = services.BuildServiceProvider();

            var builder = serviceProvider.GetRequiredService<IBuilder>();
            Assert.NotNull(builder);
        }

    }
}
