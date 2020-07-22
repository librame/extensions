using Xunit;

namespace Librame.Extensions.Core.Tests
{
    using Serializers;
    using Tokens;

    public class SecurityTokenStringSerializerTests
    {
        [Fact]
        public void AllTest()
        {
            var token = SecurityToken.New();

            var serializer = SerializerManager.GetBySource<SecurityToken>();

            var value = serializer.Serialize(token);
            Assert.NotEmpty(value);

            var source = serializer.Deserialize(value);
            Assert.NotNull(source);

            Assert.Equal(token, source);
        }
    }
}
