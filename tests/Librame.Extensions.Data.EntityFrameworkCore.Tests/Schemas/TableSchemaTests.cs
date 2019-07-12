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

            entityNames = TableSchema.GetEntityNames<AbstractEntity<int>>();
            Assert.Equal("AbstractEntities", entityNames);
        }

    }
}
