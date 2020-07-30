using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Librame.Extensions.Data.Tests
{
    public class StringDataStoreIdentityGeneratorTests
    {
        [Fact]
        public void AllTest()
        {
            var generator = TestServiceProvider.Current.GetRequiredService<TestStringStoreIdentityGenerator>();

            var id = generator.GenerateAuditId();
            Assert.NotEmpty(id);

            id = generator.GenerateMigrationId();
            Assert.NotEmpty(id);

            id = generator.GenerateTabulationId();
            Assert.NotEmpty(id);

            id = generator.GenerateTenantId();
            Assert.NotEmpty(id);

            id = generator.GenerateArticleId();
            Assert.NotEmpty(id);
        }

    }
}
