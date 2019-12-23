using System.Text;
using Xunit;

namespace Librame.Extensions.Core.Tests
{
    using Serializers;

    public class SerializableObjectTests
    {
        [Fact]
        public void AllTest()
        {
            var serializable = new SerializableObject<Encoding>(Encoding.UTF8, nameof(Encoding));
            Assert.NotEmpty(serializable.Value);

            var encoding = serializable.Value;

            serializable.ChangeSource(Encoding.ASCII);
            Assert.NotEmpty(serializable.Value);
            Assert.NotEqual(encoding, serializable.Value);
        }
    }
}
