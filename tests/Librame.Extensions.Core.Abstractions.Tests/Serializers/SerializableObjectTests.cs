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
            var serializable = SerializableObjectHelper.CreateEncoding(Encoding.UTF8);
            Assert.NotEmpty(serializable.Value);

            var encoding = serializable.Value;
            serializable.ChangeSource(Encoding.ASCII);
            
            Assert.NotEmpty(serializable.Value);
            Assert.NotEqual(encoding, serializable.Value);
        }
    }
}
