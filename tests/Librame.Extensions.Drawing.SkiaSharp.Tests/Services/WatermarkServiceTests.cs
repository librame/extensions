using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Librame.Extensions.Drawing.Tests
{
    using Core;

    public class WatermarkServiceTests
    {
        private IWatermarkService _drawing = null;

        public WatermarkServiceTests()
        {
            _drawing = TestServiceProvider.Current.GetRequiredService<IWatermarkService>();
        }
        

        [Fact]
        public async void DrawWatermarkTest()
        {
            // 5K 2.21MB
            var imageFile = "eso1004a.jpg".AsFileLocator(TestServiceProvider.ResourcesPath);
            var saveFile = imageFile.NewFileName("eso1004a-watermark.png") as FileLocator;
            
            var succeed = await _drawing.DrawFileAsync(imageFile, saveFile);
            Assert.True(succeed);
        }

    }
}
