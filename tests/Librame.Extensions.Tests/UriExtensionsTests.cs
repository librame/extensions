using System;
using System.Net;
using Xunit;

namespace Librame.Extensions.Tests
{
    public class UriExtensionsTests
    {
        [Fact]
        public void IsAbsoluteVirtualPathTest()
        {
            var path = "/path";
            Assert.True(path.IsAbsoluteVirtualPath());

            path = "~" + path;
            Assert.True(path.IsAbsoluteVirtualPath());

            var url = "http://localhost" + path;
            Assert.False(url.IsAbsoluteVirtualPath());
        }

        [Fact]
        public void AsAbsoluteUriTest()
        {
            var uri = @"http://www.domain.name/controller/action";
            Assert.NotNull(uri.AsAbsoluteUri());

            uri = "\\controller\\action";
            Assert.Throws<ArgumentException>(() => uri.AsAbsoluteUri());
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

        [Fact]
        public void GetHostTest()
        {
            var url = "http://localhost/path";
            var host = url.GetHost(out Uri result);
            Assert.Equal("localhost", host);
            Assert.NotNull(result);
        }

        [Fact]
        public void GetPathTest()
        {
            var url = "http://localhost/path";
            var path = url.GetPath(out Uri result);
            Assert.NotNull(result);

            var path1 = "~/path".GetPath();
            Assert.Equal(path, path1);
        }

        [Fact]
        public void GetQueryTest()
        {
            var url = "http://localhost/path?q=123#456";
            var query = url.GetQuery(out Uri result);
            Assert.Equal("?q=123", query);
            Assert.NotNull(result);
        }


        [Fact]
        public void IsNullOrNoneTest()
        {
            Assert.True(((IPAddress)null).IsNullOrNone());
            Assert.True(IPAddress.None.IsNullOrNone());
            Assert.True(IPAddress.IPv6None.IsNullOrNone());
        }

        [Fact]
        public void IsIPAddressTest()
        {
            var ipv4 = IPAddress.Loopback.ToString();
            var ipv6 = IPAddress.IPv6Loopback.ToString();

            Assert.True(ipv4.IsIPv4());
            Assert.True(ipv6.IsIPv6());
            Assert.True(ipv4.IsLocalIPv4());
            Assert.True(ipv6.IsLocalIPv6());
            Assert.True(ipv4.IsLocalIPAddress());
            Assert.True(ipv6.IsLocalIPAddress());
        }

    }
}
