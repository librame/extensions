using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Librame.Extensions.Drawing.Tests
{
    using Core;

    public class InternalScaleServiceTests
    {
        private IScaleService _drawing = null;

        public InternalScaleServiceTests()
        {
            _drawing = TestServiceProvider.Current.GetRequiredService<IScaleService>();
        }


        [Fact]
        public void DrawScaleTest()
        {
            // 5K 2.21MB
            var imageFile = "eso1004a.jpg".AsFileLocator(TestServiceProvider.ResourcesPath);
            
            var succeed = _drawing.DrawFile(imageFile.ToString());
            Assert.True(succeed);
        }

        [Fact]
        public async void DrawScalesByDirectoryTest()
        {
            // 5K 2.21MB
            var directory = TestServiceProvider.ResourcesPath.CombinePath(@"pictures");

            // Clear
            await _drawing.DeleteScalesByDirectoryAsync(directory);

            int count = await _drawing.DrawFilesByDirectoryAsync(directory);
            Assert.True(count > 0);
        }

    }
}
