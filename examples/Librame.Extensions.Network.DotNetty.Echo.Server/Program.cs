using Microsoft.Extensions.DependencyInjection;
using System;

namespace Librame.Extensions.Network.DotNetty.Echo.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = DotNettyServiceProvider.Current.GetRequiredService<IEchoServer>();

            server.StartAsync(channel =>
            {
                Console.ReadLine();
            })
            .Wait();
        }
    }
}
