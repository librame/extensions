using Xunit;

namespace Librame.Locators.Tests
{
    using Extensions;

    public class FileLocatorTests
    {

        [Fact]
        public void ChangeBasePathTest()
        {
            var path = @"c:\test\file.ext";

            var locator = path.AsFileLocator();
            Assert.Equal(@"c:\test", locator.BasePath);
            Assert.Equal("file.ext", locator.FileName);

            locator.ChangeBasePath(@"d:\123");
            Assert.Equal(@"d:\123\file.ext", locator.Source);
        }

    }
}
