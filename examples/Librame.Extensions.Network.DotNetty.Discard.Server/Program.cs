using Microsoft.Extensions.DependencyInjection;
using System;

namespace Librame.Extensions.Network.DotNetty.Discard.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = DotNettyServiceProvider.Current.GetRequiredService<IDiscardServer>();
            server.StartAsync(async channel =>
            {
                Console.ReadLine();

                await channel.CloseAsync();
            })
            .Wait();
        }
    }
}
