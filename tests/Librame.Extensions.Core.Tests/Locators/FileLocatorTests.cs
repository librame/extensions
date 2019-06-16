using Xunit;

namespace Librame.Extensions.Core.Tests
{
    public class FileLocatorTests
    {
        [Fact]
        public void AllTest()
        {
            var path = @"c:\test\file.ext";

            var locator = path.AsFileLocator();
            Assert.Equal(@"c:\test", locator.BasePath);
            Assert.Equal("file.ext", locator.FileName.ToString());
            Assert.Equal(path, locator.ToString());

            Assert.Equal(locator, locator.ChangeBasePath(@"d:\123"));
            Assert.Equal(locator, locator.ChangeFileName("newfile.ext"));

            Assert.NotEqual(locator, locator.NewBasePath(@"c:\test"));
            Assert.NotEqual(locator, locator.NewFileName("file.ext"));
        }

    }
}
