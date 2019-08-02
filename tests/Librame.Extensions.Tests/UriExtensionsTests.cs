using System;
using Xunit;

namespace Librame.Extensions.Tests
{
    public class UriExtensionsTests
    {
        [Fact]
        public void AsAbsoluteUriTest()
        {
            var uri = @"http://www.domain.name/controller/action";
            Assert.NotNull(uri.AsAbsoluteUri());
        }

        [Fact]
        public void IsAbsoluteUriTest()
        {
            var uri = @"http://www.domain.name/controller/action";
            Assert.True(uri.IsAbsoluteUri(out Uri result));
            Assert.NotNull(result);
        }

        [Fact]
        public void SameHostTest()
        {
            var uri = @"http://www.domain.name/controller/action";
            Assert.True(uri.SameHost("www.domain.name"));
        }

        [Fact]
        public void CombineUriTest()
        {
            var result = @"http://www.domain.name/controller/action";
            var baseUri = "http://www.domain.name/";

            Assert.Equal(result, baseUri.CombineUriToString("controller/action"));
            Assert.Equal(result, $"{baseUri}webapi/entities".CombineUriToString("/controller/action"));
        }

    }
}
