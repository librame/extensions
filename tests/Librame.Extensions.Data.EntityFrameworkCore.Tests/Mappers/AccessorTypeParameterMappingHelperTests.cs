using Xunit;

namespace Librame.Extensions.Data.Tests
{
    using Data.Mappers;

    public class AccessorTypeParameterMappingHelperTests
    {
        [Fact]
        public void ParseMapperTest()
        {
            var implType = typeof(TestDbContextAccessor);

            var mapper = AccessorTypeParameterMappingHelper.ParseMapper(implType);

            Assert.NotNull(mapper.Accessor);

            Assert.NotNull(mapper.Audit);
            Assert.NotNull(mapper.AuditProperty);
            Assert.NotNull(mapper.Entity);
            Assert.NotNull(mapper.Migration);
            Assert.NotNull(mapper.Tenant);

            Assert.NotNull(mapper.GenId);
            Assert.NotNull(mapper.IncremId);
            Assert.NotNull(mapper.CreatedBy);
            Assert.NotNull(mapper.CreatedTime);
        }

    }
}
