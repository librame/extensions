using Xunit;

namespace Librame.Extensions.Data.Tests
{
    public class TableSchemaTests
    {
        [Fact]
        public void GetEntityTypeNamesTest()
        {
            var entityNames = TableSchema.GetEntityNames<Tenant>();
            Assert.Equal("BaseTenants", entityNames);

            entityNames = TableSchema.GetEntityNames<Tenant<int, DataStatus>>();
            Assert.Equal("BaseTenants", entityNames);
        }

    }
}
