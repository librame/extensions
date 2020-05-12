using Xunit;

namespace Librame.Extensions.Data.Tests
{
    using Builders;

    public class DataBuilderDependencyTests
    {
        [Fact]
        public void AllTest()
        {
            var mySqlDefaultConnection = "server=localhost;port=3306;database=librame_data_default;user=root;password=123456";
            var mySqlWritingConnection = "server=localhost;port=3306;database=librame_data_writing;user=root;password=123456";

            var encryptDefaultConnection = DataBuilderDependency.EncryptConnectionString(mySqlDefaultConnection);
            var encryptWritingConnection = DataBuilderDependency.EncryptConnectionString(mySqlWritingConnection);

            Assert.NotEqual(mySqlDefaultConnection, encryptDefaultConnection);
            Assert.NotEqual(mySqlWritingConnection, encryptWritingConnection);

            Assert.Equal(mySqlDefaultConnection, DataBuilderDependency.DecryptConnectionString(encryptDefaultConnection));
            Assert.Equal(mySqlWritingConnection, DataBuilderDependency.DecryptConnectionString(encryptWritingConnection));
        }

    }
}
