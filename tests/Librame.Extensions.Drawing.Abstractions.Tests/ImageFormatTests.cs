using Xunit;

namespace Librame.Extensions.Drawing.Tests
{
    public class ImageFormatTests
    {

        [Fact]
        public void BaseTest()
        {
            var list = typeof(ImageFormat).AsEnumResults((f, v) => f);
            Assert.NotEmpty(list);
        }

    }
}
