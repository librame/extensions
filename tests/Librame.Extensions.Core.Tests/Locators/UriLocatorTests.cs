using Xunit;

namespace Librame.Extensions.Core.Tests
{
    public class UriLocatorTests
    {
        [Fact]
        public void AllTest()
        {
            var uriString = "https://developer.microsoft.com/en-us/fabric#/get-started";

            var locator = (UriLocator)uriString;
            Assert.Equal("https", locator.Scheme);
            Assert.Equal("developer.microsoft.com", locator.Host);
            Assert.Equal("/en-us/fabric", locator.Path);
            Assert.True(locator.Query.IsNullOrEmpty());
            Assert.Equal("#/get-started", locator.Anchor);

            Assert.Equal("http", locator.ChangeScheme("http").Scheme);
            Assert.Equal("www.microsoft.com", locator.ChangeHost("www.microsoft.com").Host);
            Assert.Equal("/zh-cn/fabric", locator.ChangePath("/zh-cn/fabric").Path);
            Assert.Equal("#/styles", locator.ChangeAnchor("#/styles").Anchor);

            Assert.NotEqual(locator, locator.NewScheme("https"));
            Assert.NotEqual(locator, locator.NewHost("developer.microsoft.com"));
            Assert.NotEqual(locator, locator.NewPath("/en-us/fabric"));
            Assert.False(locator == locator.NewAnchor("#/get-started")); // BUG: Assert.NotEqual

            Assert.Equal("?query=testQuery", locator.ChangeQuery("?query=testQuery").Query);
            var newQueriesLocator = locator.NewQueries(queries =>
            {
                Assert.True(queries.ContainsKey("query"));
                queries["query"] = "newQuery";
            });
            Assert.NotEqual(locator, newQueriesLocator);
        }
    }
}
