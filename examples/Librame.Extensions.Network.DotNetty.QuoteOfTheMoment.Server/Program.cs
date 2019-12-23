using Microsoft.Extensions.DependencyInjection;
using System;

namespace Librame.Extensions.Network.DotNetty.QuoteOfTheMoment.Server
{
    using Demo;

    class Program
    {
        static void Main(string[] args)
        {
            var server = DotNettyServiceProvider.Current.GetRequiredService<IQuoteOfTheMomentServer>();
            server.StartAsync(async channel =>
            {
                Console.WriteLine("Press any key to terminate the server.");
                Console.ReadLine();

                await channel.CloseAsync();
            })
            .Wait();
        }
    }
}
