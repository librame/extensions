using Xunit;

namespace Librame.Extensions.Core.Tests
{
    using Serializers;
    using Identifiers;

    public class SecurityIdentifierStringSerializerTests
    {
        [Fact]
        public void AllTest()
        {
            var identifier = SecurityIdentifier.New();

            var serializer = SerializerManager.GetBySource<SecurityIdentifier>();

            var value = serializer.Serialize(identifier);
            Assert.NotEmpty(value);

            var source = serializer.Deserialize(value);
            Assert.NotNull(source);

            Assert.Equal(identifier, source);
        }
    }
}
