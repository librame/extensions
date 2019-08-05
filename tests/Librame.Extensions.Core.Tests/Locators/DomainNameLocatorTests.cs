using Xunit;

namespace Librame.Extensions.Core.Tests
{
    using Extensions;

    public class DomainNameLocatorTests
    {
        [Fact]
        public void AllTest()
        {
            var uriString = "https://1.2.3.4.developer.microsoft.com/en-us/fabric#/get-started";

            var locator = (DomainNameLocator)uriString.GetHost();

            Assert.Equal("com", locator.Root);
            Assert.Equal("microsoft.com", locator.TopLevel);
            Assert.Equal("developer.microsoft.com", locator.SecondLevel);
            Assert.Equal("4.developer.microsoft.com", locator.ThirdLevel);

            Assert.Equal(3, locator.OtherLevelSegments.Length); // { 1, 2, 3 }

            Assert.Equal("microsoft", locator.TopLevelSegment);
            Assert.Equal("developer", locator.SecondLevelSegment);
            Assert.Equal("4", locator.ThirdLevelSegment);

            locator.ChangeDomainName("google.org");

            Assert.NotEqual("com", locator.Root);
            Assert.NotEqual("microsoft.com", locator.TopLevel);
            Assert.Equal("org", locator.Root);
            Assert.Equal("google.org", locator.TopLevel);
            Assert.Equal("developer.google.org", locator.SecondLevel);
            Assert.Equal("4.developer.google.org", locator.ThirdLevel);

            Assert.Equal(3, locator.OtherLevelSegments.Length); // { 1, 2, 3 }

            Assert.Equal("google", locator.TopLevelSegment);
            Assert.Equal("developer", locator.SecondLevelSegment);
            Assert.Equal("4", locator.ThirdLevelSegment);

            var onlyTwoLevels = locator.GetOnlyTwoLevels();
            Assert.Equal("1.2.3.4.developer", onlyTwoLevels.Child);
            Assert.Equal("google.org", onlyTwoLevels.Parent);

            Assert.NotEqual(locator, locator.NewDomainName("microsoft.com"));
        }
    }
}
