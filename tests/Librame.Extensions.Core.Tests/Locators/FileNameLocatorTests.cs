using Xunit;

namespace Librame.Extensions.Core.Tests
{
    public class FileNameLocatorTests
    {
        [Fact]
        public void AllTest()
        {
            var path = @"c:\test\file.ext";

            var locator = path.AsFileNameLocator();
            Assert.Equal("file", locator.BaseName);
            Assert.Equal(".ext", locator.Extension);
            Assert.Equal("file.ext", locator.ToString());

            Assert.Equal(locator, locator.ChangeBaseName("file1"));
            Assert.Equal(locator, locator.ChangeExtension(".txt"));

            Assert.NotEqual(locator, locator.NewBaseName("file"));
            Assert.NotEqual(locator, locator.NewExtension(".ext"));
        }
    }
}
