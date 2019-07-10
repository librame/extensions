using Xunit;

namespace Librame.Extensions.Data.Tests
{
    public class TableSchemaTests
    {
        [Fact]
        public void GetEntityTypeNamesTest()
        {
            var tableNames = nameof(Tenant).AsPluralize();

            var entityNames = TableSchema.GetEntityNames<Tenant>();
            Assert.Equal(tableNames, entityNames);

            entityNames = TableSchema.GetEntityNames<Tenant<int, DataStatus>>();
            Assert.Equal(tableNames, entityNames);
        }

    }
}
