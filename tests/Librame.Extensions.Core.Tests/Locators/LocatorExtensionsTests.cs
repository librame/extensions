using Xunit;

namespace Librame.Extensions.Core.Tests
{
    public class LocatorExtensionsTests
    {
        [Fact]
        public void AllTest()
        {
            var file = @"c:\temp.txt";
            var dir = @"d:\123";
            var locator = file.AsFileLocator(dir);
            Assert.Equal(dir, locator.BasePath);
            Assert.NotEqual(file, locator.ToString());

            var newlocator = locator.NewFileName("abc.txt");
            Assert.NotEqual(locator.ToString(), newlocator.ToString());
        }
    }
}
