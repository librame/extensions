using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Librame.Extensions.Drawing.Tests
{
    using Core.Combiners;
    using Drawing.Services;

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
            var imageFile = "microsoft_edge.jpg".AsFilePathCombiner(TestServiceProvider.ResourcesPath);
            var saveFile = imageFile.WithFileName("microsoft_edge-watermark.png");
            
            var succeed = await _drawing.DrawFileAsync(imageFile, saveFile).ConfigureAndResultAsync();
            Assert.True(succeed);
        }

    }
}
