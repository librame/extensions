using System.Text;
using Xunit;

namespace Librame.Extensions.Core.Tests
{
    using Serializers;

    public class EncodingStringSerializerTests
    {
        [Fact]
        public void AllTest()
        {
            var serializer = new EncodingStringSerializer();
            Assert.NotEmpty(serializer.Name);

            var test = Encoding.UTF8;

            var str = serializer.Serialize(test);
            Assert.NotEmpty(str);

            var deserialize = serializer.Deserialize<Encoding>(str);
            Assert.NotNull(deserialize);
            Assert.Equal(test, deserialize);
        }
    }
}
