using System.Linq;
using Xunit;

namespace Librame.Extensions.Core.Tests
{
    using Serializers;

    public class SerializerRegistrationTests
    {
        [Fact]
        public void AllTest()
        {
            var serializer = SerializerRegistration.AllSerializers.First();
            Assert.NotNull(serializer);

            var serializer1 = SerializerRegistration.GetObjectString(serializer.Name);
            Assert.Equal(serializer.Name, serializer1.Name);
            Assert.Same(serializer, serializer1);

            SerializerRegistration.Clear();
            Assert.False(SerializerRegistration.Count > 0);
        }
    }
}
