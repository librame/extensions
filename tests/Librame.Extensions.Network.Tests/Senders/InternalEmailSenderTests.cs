using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Librame.Extensions.Network.Tests
{
    public class InternalEmailSenderTests
    {
        private IEmailSender _sender;

        public InternalEmailSenderTests()
        {
            _sender = TestServiceProvider.Current.GetRequiredService<IEmailSender>();
        }


        [Fact]
        public async void SendAsyncTest()
        {
            await _sender.SendAsync("receiver@domain.com",
                    "Email Subject",
                    "Email Body");

            //var file = _sender.CreateAttachment("fileName");

            //await _sender.SendAsync("toAddress",
            //    "subject",
            //    "body",
            //    configureMessage: msg =>
            //    {
            //        msg.Attachments.Add(file);
            //    });
        }

    }
}
