using Xunit;

namespace Librame.Locators.Tests
{
    public class DefaultFileLocatorTests
    {

        [Fact]
        public void FileLocatorTest()
        {
            var path = @"c:\test\file.ext";

            var locator = new DefaultFileLocator(path);
            Assert.Equal(@"c:\test", locator.BasePath);
            Assert.Equal("file.ext", locator.FileName);

            locator.ChangeBasePath(@"d:\123");
            Assert.Equal(@"d:\123\file.ext", locator.Source);
        }

    }
}
