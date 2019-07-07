using Xunit;

namespace Librame.Extensions.Data.Tests
{
    public class TableSchemaTests
    {
        [Fact]
        public void GetEntityTypeNamesTest()
        {
            var entityNames = TableSchema.GetEntityNames<BaseTenant>();
            Assert.Equal("BaseTenants", entityNames);

            entityNames = TableSchema.GetEntityNames<BaseTenant<int, DataStatus>>();
            Assert.Equal("BaseTenants", entityNames);
        }

    }
}
