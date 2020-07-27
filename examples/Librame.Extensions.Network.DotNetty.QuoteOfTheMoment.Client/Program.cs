using DotNetty.Buffers;
using DotNetty.Transport.Channels.Sockets;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Librame.Extensions.Network.DotNetty.QuoteOfTheMoment.Client
{
    using Demo;

    class Program
    {
        static void Main(string[] args)
        {
            var client = DotNettyServiceProvider.Current.GetRequiredService<IQuoteOfTheMomentClient>();
            client.StartAsync(async channel =>
            {
                Console.WriteLine("Sending broadcast QOTM");

                // Broadcast the QOTM request to port.
                var bytes = ExtensionSettings.Preference.DefaultEncoding.GetBytes("QOTM?");
                var buffer = Unpooled.WrappedBuffer(bytes);

                var endPoint = new IPEndPoint(IPAddress.Broadcast,
                    client.Options.QuoteOfTheMomentClient.Port);

                await channel.WriteAndFlushAsync(new DatagramPacket(buffer, endPoint));

                Console.WriteLine("Waiting for response.");

                await Task.Delay(5000);
                Console.WriteLine("Waiting for response time 5000 completed. Closing client channel.");

                await channel.CloseAsync();
            })
            .Wait();
        }
    }
}
