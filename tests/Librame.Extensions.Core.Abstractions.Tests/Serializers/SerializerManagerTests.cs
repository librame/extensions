using System.Text;
using Xunit;

namespace Librame.Extensions.Core.Tests
{
    using Serializers;

    public class SerializerManagerTests
    {
        [Fact]
        public void AllTest()
        {
            var oldSerializerType = typeof(EncodingStringSerializer);
            var oldSerializer = SerializerManager.GetBySource<Encoding>();
            Assert.Equal(oldSerializerType, oldSerializer.GetType());

            var newSerializerType = typeof(TestEncodingStringSerializer);
            var newSerializer = SerializerManager.Default.Replace(oldSerializer,
                new TestEncodingStringSerializer());
            Assert.Equal(newSerializerType, newSerializer.GetType());

            var serializer = SerializerManager.GetBySource<Encoding>();
            Assert.Equal(newSerializerType, serializer.GetType());
        }


        [NonRegistered]
        public class TestEncodingStringSerializer : AbstractStringSerializer<Encoding>
        {
            public TestEncodingStringSerializer()
                : base(f => f.AsName(), r => r.FromEncodingName())
            {
            }
        }

    }
}
