using Librame.Extensions.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Librame.Extensions.Network.Tests
{
    public class CrawlerServiceTests
    {
        public class TestCrawlerService : ICrawlerService
        {
            public IServicesManager<IUriRequester> Requesters { get; }

            public string[] ImageExtensions { get; set; }

            public ILoggerFactory LoggerFactory => throw new NotImplementedException();

            public Encoding Encoding { get; set; }

            public void Dispose()
            {
                throw new NotImplementedException();
            }

            public Task<IList<string>> GetHyperLinksAsync(string url, string pattern = null)
            {
                throw new NotImplementedException();
            }

            public Task<IList<string>> GetImageLinksAsync(string url, string pattern = null)
            {
                throw new NotImplementedException();
            }

            public Task<string> GetContentAsync(string url, string postData = null,
                CancellationToken cancellationToken = default)
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
            Assert.ThrowsAsync<NotImplementedException>(() => service.GetImageLinksAsync(null));
            Assert.ThrowsAsync<NotImplementedException>(() => service.GetContentAsync(null, null));
        }
    }
}
