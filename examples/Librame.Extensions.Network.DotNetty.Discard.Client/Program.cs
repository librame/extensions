using Microsoft.Extensions.DependencyInjection;
using System;

namespace Librame.Extensions.Network.DotNetty.Discard.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = DotNettyServiceProvider.Current.GetRequiredService<IDiscardClient>();

            client.StartAsync(channel =>
            {
                Console.ReadLine();
            })
            .Wait();
        }
    }
}
