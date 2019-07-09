using Xunit;

namespace Librame.Extensions.Data.Tests
{
    public class TableSchemaTests
    {
        [Fact]
        public void GetEntityTypeNamesTest()
        {
            var entityNames = TableSchema.GetEntityNames<DataTenant>();
            Assert.Equal("BaseTenants", entityNames);

            entityNames = TableSchema.GetEntityNames<DataTenant<int, DataStatus>>();
            Assert.Equal("BaseTenants", entityNames);
        }

    }
}
