using Xunit;

namespace Librame.Extensions.Core.Tests
{
    public class FileLocatorTests
    {

        [Fact]
        public void ChangeBasePathTest()
        {
            var path = @"c:\test\file.ext";

            var locator = path.AsFileLocator();
            Assert.Equal(@"c:\test", locator.BasePath);
            Assert.Equal("file.ext", locator.FileName);
            Assert.Equal(path, locator.GetSource());
            Assert.Equal(path, locator.ToString());

            Assert.Equal(@"d:\123\file.ext", locator.NewBasePath(@"d:\123").GetSource());
        }

    }
}
