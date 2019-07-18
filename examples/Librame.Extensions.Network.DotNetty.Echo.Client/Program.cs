using Microsoft.Extensions.DependencyInjection;
using System;

namespace Librame.Extensions.Network.DotNetty.Echo.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = DotNettyServiceProvider.Current.GetRequiredService<IEchoClient>();

            client.StartAsync(channel =>
            {
                Console.ReadLine();
            })
            .Wait();
        }
    }
}
