using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Librame.Extensions.Core.Tests
{
    public class CoreBuilderServiceCollectionExtensionsTests
    {
        [Fact]
        public void AddLibrameTest()
        {
            var builder = TestServiceProvider.Current.GetRequiredService<ICoreBuilder>();
            Assert.NotNull(builder);
        }

    }
}
