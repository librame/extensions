using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Librame.Extensions.Network.Tests
{
    using Encryption;

    public class CrawlerServiceTests
    {
        public class TestCrawlerService : ICrawlerService
        {
            public string[] ImageExtensions { get; set; }

            public IHashService Hash => throw new NotImplementedException();

            public Encoding Encoding { get; set; }

            public void Dispose()
            {
                throw new NotImplementedException();
            }

            public Task<IList<string>> GetHyperLinksAsync(string url, string pattern = null)
            {
                throw new NotImplementedException();
            }

            public Task<IList<string>> GetImageHyperLinksAsync(string url, string pattern = null)
            {
                throw new NotImplementedException();
            }

            public Task<string> GetStringAsync(string url)
            {
                throw new NotImplementedException();
            }

            public Task<string> PostStringAsync(string url, string postData)
            {
                throw new NotImplementedException();
            }
        }


        [Fact]
        public void AllTest()
        {
            ICrawlerService service = new TestCrawlerService();

            Assert.Throws<NotImplementedException>(() => service.Dispose());
            Assert.ThrowsAsync<NotImplementedException>(() => service.GetHyperLinksAsync(null));
            Assert.ThrowsAsync<NotImplementedException>(() => service.GetImageHyperLinksAsync(null));
            Assert.ThrowsAsync<NotImplementedException>(() => service.GetStringAsync(null));
            Assert.ThrowsAsync<NotImplementedException>(() => service.PostStringAsync(null, null));
        }
    }
}
