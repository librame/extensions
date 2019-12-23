using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Librame.Extensions.Network.Tests
{
    using Services;

    public class SmsServiceTests
    {
        private ISmsService _service;

        public SmsServiceTests()
        {
            _service = TestServiceProvider.Current.GetRequiredService<ISmsService>();
        }


        [Fact]
        public void SendAsyncTest()
        {
            Assert.ThrowsAsync<System.Net.WebException>(() =>
            {
                return _service.SendAsync("13012345678", "TestData: 123456");
            });
        }

    }
}
