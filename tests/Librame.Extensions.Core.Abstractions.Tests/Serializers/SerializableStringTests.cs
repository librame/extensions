using System.Text;
using Xunit;

namespace Librame.Extensions.Core.Tests
{
    using Serializers;

    public class SerializableStringTests
    {
        [Fact]
        public void AllTest()
        {
            var encoding = new SerializableString<Encoding>(Encoding.UTF8);
            Assert.NotEmpty(encoding.Value);

            var rawEncoding = encoding.Value;
            encoding.ChangeSource(Encoding.ASCII);
            Assert.NotEmpty(encoding.Value);

            Assert.NotEqual(rawEncoding, encoding.Value);
        }
    }
}
