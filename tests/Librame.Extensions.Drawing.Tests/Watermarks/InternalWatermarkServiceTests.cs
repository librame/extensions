using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Librame.Extensions.Drawing.Tests
{
    using Locators;

    public class InternalWatermarkServiceTests
    {
        private IWatermarkService _drawing = null;

        public InternalWatermarkServiceTests()
        {
            _drawing = TestServiceProvider.Current.GetRequiredService<IWatermarkService>();
        }
        

        [Fact]
        public async void DrawWatermarkTest()
        {
            // 5K 2.21MB
            var imageFile = new DefaultFileLocator("eso1004a.jpg")
                .ChangeBasePath(TestServiceProvider.ResourcesPath);

            var saveFile = new DefaultFileLocator("eso1004a-watermark.png")
                .ChangeBasePath(imageFile.BasePath);
            
            var succeed = await _drawing.DrawFile(imageFile.ToString(), saveFile.ToString());
            Assert.True(succeed);
        }

    }
}
