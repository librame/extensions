using Xunit;

namespace Librame.Extensions.Encryption.Tests
{
    using Core.Serializers;
    using Encryption.Identifiers;

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
