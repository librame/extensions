using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Librame.Extensions.Network.Tests
{
    public class InternalSmsSenderTests
    {
        private ISmsSender _sender;

        public InternalSmsSenderTests()
        {
            _sender = TestServiceProvider.Current.GetRequiredService<ISmsSender>();
        }


        [Fact]
        public void SendAsyncTest()
        {
            var result = _sender.SendAsync("TestData: 123456").Result;
            Assert.Empty(result);
        }

    }
}
