using Microsoft.Extensions.DependencyInjection;
using System;

namespace Librame.Extensions.Network.DotNetty.Factorial.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = DotNettyServiceProvider.Current.GetRequiredService<IFactorialServer>();

            server.StartAsync(channel =>
            {
                Console.ReadLine();
            })
            .Wait();
        }
    }
}
