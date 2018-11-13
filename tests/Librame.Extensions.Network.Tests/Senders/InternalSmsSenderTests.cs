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
        public async void SendAsyncTest()
        {
            var result = await _sender.SendAsync("TestData: 123456");
            Assert.Empty(result);
        }

    }
}
