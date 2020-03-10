using Xunit;

namespace Librame.Extensions.Encryption.Tests
{
    using Core.Serializers;
    using Encryption.KeyGenerators;

    public class KeyDescriptorStringSerializerTests
    {
        [Fact]
        public void AllTest()
        {
            var descriptor = KeyDescriptor.New();

            var serializer = SerializerHelper.GetStringSerializer<KeyDescriptor>();

            var value = serializer.Serialize(descriptor);
            Assert.NotEmpty(value);

            var source = serializer.Deserialize(value);
            Assert.NotNull(source);

            Assert.Equal(descriptor, source);
        }
    }
}
