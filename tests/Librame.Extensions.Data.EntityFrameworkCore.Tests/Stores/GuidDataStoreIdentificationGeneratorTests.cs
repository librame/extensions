using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace Librame.Extensions.Data.Tests
{
    public class GuidDataStoreIdentificationGeneratorTests
    {
        [Fact]
        public void AllTest()
        {
            var generator = TestServiceProvider.Current.GetRequiredService<TestGuidStoreIdentificationGenerator>();

            var id = generator.GenerateAuditId();
            Assert.NotEqual(Guid.Empty, id);

            id = generator.GenerateMigrationId();
            Assert.NotEqual(Guid.Empty, id);

            id = generator.GenerateTabulationId();
            Assert.NotEqual(Guid.Empty, id);

            id = generator.GenerateTenantId();
            Assert.NotEqual(Guid.Empty, id);

            id = generator.GenerateArticleId();
            Assert.NotEqual(Guid.Empty, id);
        }

    }
}
