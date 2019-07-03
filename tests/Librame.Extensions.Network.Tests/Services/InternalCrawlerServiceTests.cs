﻿using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Librame.Extensions.Network.Tests
{
    public class InternalCrawlerServiceTests
    {
        private ICrawlerService _crawler;

        public InternalCrawlerServiceTests()
        {
            _crawler = TestServiceProvider.Current.GetRequiredService<ICrawlerService>();
        }


        [Fact]
        public async void GetStringTest()
        {
            var result = await _crawler.GetStringAsync("https://www.cnblogs.com");
            Assert.NotEmpty(result);
        }


        [Fact]
        public async void GetHyperLinksTest()
        {
            var result = await _crawler.GetHyperLinksAsync("https://www.baidu.com");
            Assert.True(result.Count > 0);
        }


        [Fact]
        public async void GetImageHyperLinksTest()
        {
            var result = await _crawler.GetImageLinksAsync("https://www.baidu.com");
            Assert.True(result.Count > 0);
        }

    }
}
