using Xunit;

namespace Librame.Extensions.Core.Tests
{
    using Combiners;

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
            Assert.True(combiner.Query.IsEmpty());
            Assert.Equal("#/get-started", combiner.Anchor);

            Assert.Equal("http", combiner.ChangeScheme("http").Scheme);
            Assert.Equal("www.microsoft.com", combiner.ChangeHost("www.microsoft.com").Host);
            Assert.Equal("/zh-cn/fabric", combiner.ChangePath("/zh-cn/fabric").Path);
            Assert.Equal("#/styles", combiner.ChangeAnchor("#/styles").Anchor);

            Assert.NotEqual(combiner, combiner.WithScheme("https"));
            Assert.NotEqual(combiner, combiner.WithHost("developer.microsoft.com"));
            Assert.NotEqual(combiner, combiner.WithPath("/en-us/fabric"));
            Assert.NotEqual(combiner, combiner.WithAnchor("#/get-started"));

            Assert.Equal("?query=testQuery", combiner.ChangeQuery("?query=testQuery").Query);
            Assert.NotEmpty(combiner.Queries);
            var newQueriesCombiner = combiner.WithQueries(queries =>
            {
                Assert.True(queries.ContainsKey("query"));
                queries["query"] = "newQuery";
            });
            Assert.NotEqual(combiner, newQueriesCombiner);
        }
    }
}
