using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Librame.Extensions.Encryption.Tests
{
    public class InternalEncryptionBuilderTests
    {
        [Fact]
        public void EncryptionBuilderTest()
        {
            var algorithmBuilder = TestServiceProvider.Current.GetRequiredService<IEncryptionBuilder>();
            Assert.NotNull(algorithmBuilder);
        }

    }
}
