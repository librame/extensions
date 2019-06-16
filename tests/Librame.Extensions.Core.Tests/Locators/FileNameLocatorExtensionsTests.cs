using Xunit;

namespace Librame.Extensions.Core.Tests
{
    public class FileNameLocatorExtensionsTests
    {
        [Fact]
        public void AllTest()
        {
            var files = new string[] { "1.txt", "2.txt", "3.txt", "4.txt", "5.txt" };
            var ext = ".txt";

            var locators = files.AsFileNameLocators();
            foreach (var locator in locators)
                Assert.Equal(ext, locator.Extension);

            ext = ".ext";
            locators.ChangeExtension(ext);
            foreach (var locator in locators)
                Assert.Equal(ext, locator.Extension);
        }
    }
}
