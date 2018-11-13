using Xunit;

namespace Librame.Extensions.Drawing.Tests
{
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
            var imageFile = "eso1004a.jpg".AsDefaultFileLocator(TestServiceProvider.ResourcesPath);
            var saveFile = imageFile.NewFileName("eso1004a-watermark.png");
            
            var succeed = await _drawing.DrawFile(imageFile.ToString(), saveFile.ToString());
            Assert.True(succeed);
        }

    }
}
