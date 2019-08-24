using Microsoft.Extensions.DependencyInjection;
using System;

namespace Librame.Extensions.Network.DotNetty.HttpServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = DotNettyServiceProvider.Current.GetRequiredService<IHttpServer>();
            server.StartAsync(async channel =>
            {
                Console.ReadLine();

                await channel.CloseAsync();
            })
            .Wait();
        }
    }
}
