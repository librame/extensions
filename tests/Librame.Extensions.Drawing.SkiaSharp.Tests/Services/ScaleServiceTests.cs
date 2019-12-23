using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Librame.Extensions.Drawing.Tests
{
    using Core.Combiners;
    using Services;

    public class ScaleServiceTests
    {
        private IScaleService _drawing = null;

        public ScaleServiceTests()
        {
            _drawing = TestServiceProvider.Current.GetRequiredService<IScaleService>();
        }


        [Fact]
        public void DrawScaleTest()
        {
            // 5K 2.21MB
            var imageFile = "eso1004a.jpg".AsFilePathCombiner(TestServiceProvider.ResourcesPath);
            var succeed = _drawing.DrawFile(imageFile);
            Assert.True(succeed);
        }

        [Fact]
        public async void DrawScalesByDirectoryTest()
        {
            // 5K 2.21MB
            var directory = TestServiceProvider.ResourcesPath.CombinePath(@"pictures");

            // Clear
            await _drawing.DeleteScalesByDirectoryAsync(directory).ConfigureAndResultAsync();

            var count = await _drawing.DrawFilesByDirectoryAsync(directory).ConfigureAndResultAsync();
            Assert.True(count > 0);
        }

    }
}
