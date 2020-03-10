using System;
using Xunit;

namespace Librame.Extensions.Core.Tests
{
    using Serializers;

    public class TypeStringSerializerTests
    {
        [Fact]
        public void AllTest()
        {
            var type = typeof(TypeStringSerializerTests);

            var serializer = SerializerHelper.GetStringSerializer<Type>();

            var value = serializer.Serialize(type);
            Assert.NotEmpty(value);

            var source = serializer.Deserialize(value);
            Assert.NotNull(source);

            Assert.Equal(type, source);
        }
    }
}
