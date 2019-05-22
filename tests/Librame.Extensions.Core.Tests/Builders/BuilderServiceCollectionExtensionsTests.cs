using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Librame.Extensions.Core.Tests
{
    public class BuilderServiceCollectionExtensionsTests
    {
        [Fact]
        public void AddLibrameTest()
        {
            var builder = TestServiceProvider.Current.GetRequiredService<IBuilder>();
            Assert.NotNull(builder);
        }

    }
}
