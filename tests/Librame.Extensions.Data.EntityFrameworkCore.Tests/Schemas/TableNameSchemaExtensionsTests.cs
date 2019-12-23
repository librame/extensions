using Xunit;

namespace Librame.Extensions.Data.Tests
{
    using Schemas;

    public class TableNameSchemaExtensionsTests
    {
        public class TestEntity
        {
        }


        [Fact]
        public void AllTest()
        {
            var descriptor = new TableNameDescriptor<TestEntity>();

            var table = descriptor.AsSchema("dbo");
            Assert.Equal("dbo.TestEntities", table);

            var options = new TableNameSchemaOptions();

            descriptor.ChangeInternalPrefix(options);
            Assert.Equal("dbo.Internal_TestEntities", table);

            descriptor.ChangePrivatePrefix(options);
            Assert.Equal("dbo.__TestEntities", table);
        }
    }
}
