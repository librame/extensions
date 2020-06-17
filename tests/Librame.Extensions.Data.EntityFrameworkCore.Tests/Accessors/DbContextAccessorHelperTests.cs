using Xunit;

namespace Librame.Extensions.Data.Tests
{
    using Accessors;

    public class DbContextAccessorHelperTests
    {
        [Fact]
        public void AllTest()
        {
            var descriptor = DbContextAccessorHelper.ParseMappingDescriptor(typeof(TestDbContextAccessor));

            Assert.NotNull(descriptor.Accessor);

            Assert.NotNull(descriptor.Audit);
            Assert.NotNull(descriptor.AuditProperty);
            Assert.NotNull(descriptor.Entity);
            Assert.NotNull(descriptor.Migration);
            Assert.NotNull(descriptor.Tenant);

            Assert.NotNull(descriptor.GenId);
            Assert.NotNull(descriptor.IncremId);
            Assert.NotNull(descriptor.CreatedBy);
            Assert.NotNull(descriptor.CreatedTime);
        }

    }
}
