using Xunit;

namespace Librame.Extensions.Core.Tests
{
    public class UriCombinerTests
    {
        [Fact]
        public void AllTest()
        {
            var uriString = "https://developer.microsoft.com/en-us/fabric#/get-started";

            var combiner = uriString.AsUriCombiner();
            Assert.Equal("https", combiner.Scheme);
            Assert.Equal("developer.microsoft.com", combiner.Host);
            Assert.Equal("/en-us/fabric", combiner.Path);
            Assert.True(combiner.Query.IsNullOrEmpty());
            Assert.Equal("#/get-started", combiner.Anchor);

            Assert.Equal("http", combiner.ChangeScheme("http").Scheme);
            Assert.Equal("www.microsoft.com", combiner.ChangeHost("www.microsoft.com").Host);
            Assert.Equal("/zh-cn/fabric", combiner.ChangePath("/zh-cn/fabric").Path);
            Assert.Equal("#/styles", combiner.ChangeAnchor("#/styles").Anchor);

            Assert.NotEqual(combiner, combiner.NewScheme("https"));
            Assert.NotEqual(combiner, combiner.NewHost("developer.microsoft.com"));
            Assert.NotEqual(combiner, combiner.NewPath("/en-us/fabric"));
            Assert.False(combiner == combiner.NewAnchor("#/get-started")); // BUG: Assert.NotEqual

            Assert.Equal("?query=testQuery", combiner.ChangeQuery("?query=testQuery").Query);
            Assert.NotEmpty(combiner.Queries);
            var newQueriesCombiner = combiner.NewQueries(queries =>
            {
                Assert.True(queries.ContainsKey("query"));
                queries["query"] = "newQuery";
            });
            Assert.NotEqual(combiner, newQueriesCombiner);
        }
    }
}
