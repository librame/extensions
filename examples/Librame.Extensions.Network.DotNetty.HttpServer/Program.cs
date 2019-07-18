using Microsoft.Extensions.DependencyInjection;
using System;

namespace Librame.Extensions.Network.DotNetty.HttpServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = DotNettyServiceProvider.Current.GetRequiredService<IHttpServer>();

            server.StartAsync(channel =>
            {
                Console.ReadLine();
            })
            .Wait();
        }
    }
}
