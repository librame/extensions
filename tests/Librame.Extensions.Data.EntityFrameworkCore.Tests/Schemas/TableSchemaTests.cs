using Xunit;

namespace Librame.Extensions.Data.Tests
{
    public class TableSchemaTests
    {
        [Fact]
        public void GetEntityPluralNameTest()
        {
            var targetName = nameof(Tenant).AsPluralize();
            var pluralName = TableSchema.GetEntityPluralName<Tenant>();
            Assert.Equal(targetName, pluralName);

            pluralName = TableSchema.GetEntityPluralName<AbstractEntity<int>>();
            Assert.Equal("AbstractEntities", pluralName);
        }

    }
}
