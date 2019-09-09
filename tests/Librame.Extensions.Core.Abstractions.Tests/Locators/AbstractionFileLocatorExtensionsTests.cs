using Xunit;

namespace Librame.Extensions.Core.Tests
{
    public class AbstractionFileLocatorExtensionsTests
    {
        [Fact]
        public void AllTest()
        {
            var files = new string[] { "1.txt", "2.txt", "3.txt", "4.txt", "5.txt" };
            var dir = @"d:\123";

            var locators = files.AsFileLocators(dir);
            foreach (var locator in locators)
                Assert.Equal(dir, locator.BasePath);

            dir = @"c:\test";
            locators.ChangeBasePath(dir);
            foreach (var locator in locators)
                Assert.Equal(dir, locator.BasePath);
        }
    }
}
