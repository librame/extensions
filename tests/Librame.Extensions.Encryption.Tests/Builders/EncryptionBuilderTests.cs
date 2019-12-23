using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Librame.Extensions.Encryption.Tests
{
    using Builders;

    public class EncryptionBuilderTests
    {
        [Fact]
        public void AllTest()
        {
            var builder = TestServiceProvider.Current.GetRequiredService<IEncryptionBuilder>();
            Assert.NotNull(builder);
        }

    }
}
