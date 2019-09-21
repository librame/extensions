using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Librame.Extensions.Network.Tests
{
    public class CrawlerServiceTests
    {
        private ICrawlerService _crawler;

        public CrawlerServiceTests()
        {
            _crawler = TestServiceProvider.Current.GetRequiredService<ICrawlerService>();
        }


        [Fact]
        public async void GetContentTest()
        {
            var content = await _crawler.GetContentAsync("https://www.cnblogs.com").ConfigureAwait(true);
            Assert.NotEmpty(content);
        }


        [Fact]
        public async void GetHyperLinksTest()
        {
            var links = await _crawler.GetHyperLinksAsync("https://www.baidu.com").ConfigureAwait(true);
            Assert.NotEmpty(links);
        }


        [Fact]
        public async void GetImageLinksTest()
        {
            var images = await _crawler.GetImageLinksAsync("https://www.baidu.com").ConfigureAwait(true);
            Assert.NotEmpty(images);
        }

    }
}
