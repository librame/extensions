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
            var encoding = Encoding.UTF8;

            var serializer = SerializerManager.GetBySource<Encoding>();

            var value = serializer.Serialize(encoding);
            Assert.NotEmpty(value);

            var source = serializer.Deserialize(value);
            Assert.NotNull(source);

            Assert.Equal(encoding, source);
        }
    }
}
