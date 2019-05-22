using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Librame.Extensions.Network.Tests
{
    public class InternalSmsServiceTests
    {
        private ISmsService _service;

        public InternalSmsServiceTests()
        {
            _service = TestServiceProvider.Current.GetRequiredService<ISmsService>();
        }


        [Fact]
        public async void SendAsyncTest()
        {
            var result = await _service.SendAsync("TestData: 123456");
            Assert.Empty(result);
        }

    }
}
