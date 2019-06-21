using Xunit;

namespace Librame.Extensions.Drawing.Tests
{
    public class ImageFormatTests
    {
        [Fact]
        public void AllTest()
        {
            var list = typeof(ImageFormat).AsEnumResults(f => f);
            Assert.NotEmpty(list);
        }

    }
}
