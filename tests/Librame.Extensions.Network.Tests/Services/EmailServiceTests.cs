using Microsoft.Extensions.DependencyInjection;
using System.Net.Sockets;
using Xunit;

namespace Librame.Extensions.Network.Tests
{
    using Services;

    public class EmailServiceTests
    {
        private IEmailService _service;

        public EmailServiceTests()
        {
            _service = TestServiceProvider.Current.GetRequiredService<IEmailService>();
        }


        [Fact]
        public void SendAsyncTest()
        {
            Assert.ThrowsAsync<SocketException>(() =>
            {
                return _service.SendAsync("receiver#domain.com",
                    "Email Subject",
                    "Email Body");

                //var file = _service.CreateAttachment("fileName");

                //await _service.SendAsync("toAddress",
                //    "subject",
                //    "body",
                //    configureMessage: msg =>
                //    {
                //        msg.Attachments.Add(file);
                //    });
            });
        }

    }
}
