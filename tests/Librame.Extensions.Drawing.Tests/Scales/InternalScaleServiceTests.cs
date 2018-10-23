using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace Librame.Extensions.Drawing.Tests
{
    using Locators;

    public class InternalScaleServiceTests
    {
        private IScaleService _drawing = null;

        public InternalScaleServiceTests()
        {
            _drawing = TestServiceProvider.Current.GetRequiredService<IScaleService>();
        }


        [Fact]
        public async void DrawScaleTest()
        {
            // 5K 2.21MB
            var imageFile = new DefaultFileLocator("eso1004a.jpg")
                .ChangeBasePath(TestServiceProvider.ResourcesPath);
            
            var succeed = await _drawing.DrawFile(imageFile.ToString());
            Assert.True(succeed);
        }

        [Fact]
        public async void DrawScalesByDirectoryTest()
        {
            // 5K 2.21MB
            var directory = TestServiceProvider.ResourcesPath.CombinePath(@"pictures");
            
            // Clear
            var count = _drawing.DeleteScalesByDirectory(directory);

            count = await _drawing.DrawFilesByDirectory(directory);
            Assert.True(count > 0);
        }

    }
}
