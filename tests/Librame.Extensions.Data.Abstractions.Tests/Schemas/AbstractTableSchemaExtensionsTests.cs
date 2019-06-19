using Xunit;

namespace Librame.Extensions.Data.Tests
{
    public class AbstractTableSchemaExtensionsTests
    {
        public class TestTableSchema : AbstractSchema, ITableSchema
        {
            public string Name => nameof(TestTableSchema);
        }


        [Fact]
        public void ApplySchemaTest()
        {
            var tableSchema = new TestTableSchema();
            Assert.NotEmpty(tableSchema.Name);

            tableSchema.ApplySchema(nameof(Librame));
            Assert.NotEmpty(tableSchema.Schema);
        }
    }
}
