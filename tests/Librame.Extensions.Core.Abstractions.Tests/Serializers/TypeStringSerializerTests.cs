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
            var serializer = new TypeStringSerializer();
            Assert.NotEmpty(serializer.Name);

            var test = typeof(TypeStringSerializerTests);

            var str = serializer.Serialize(test);
            Assert.NotEmpty(str);

            var deserialize = serializer.Deserialize<Type>(str);
            Assert.NotNull(deserialize);
            Assert.Equal(test, deserialize);
        }
    }
}
