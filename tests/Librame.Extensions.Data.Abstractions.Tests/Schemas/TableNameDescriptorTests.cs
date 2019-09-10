using System;
using Xunit;

namespace Librame.Extensions.Data.Tests
{
    public class TableNameDescriptorTests
    {
        [Fact]
        public void AllTest()
        {
            var descriptor = new TableNameDescriptor<AbstractEntity<int>>();
            Assert.Equal("AbstractEntities", descriptor);

            descriptor.ChangePrefix("Data");
            Assert.Equal("Data_AbstractEntities", descriptor);

            descriptor.ChangeSuffix("01");
            Assert.Equal("Data_AbstractEntities_01", descriptor);

            var year = DateTime.Now.ToString("yyyy");
            descriptor.ChangeDateSuffix(now => now.ToString("yyyy"));
            Assert.Equal($"Data_AbstractEntities_{year}", descriptor);

            descriptor.ChangeBodyName(names => names.TrimStart("Abstract"));
            Assert.Equal($"Data_Entities_{year}", descriptor);

            descriptor.ChangeConnector("-");
            Assert.Equal($"Data-Entities-{year}", descriptor);

            descriptor.Reset();
            Assert.Equal("AbstractEntities", descriptor);
        }

    }
}
